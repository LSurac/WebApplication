using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using WebApplication.SqlDataAccess.Contract.Configurations;

namespace WebApplication.SqlDataAccess.Services
{
    public class SqlDataAccessor(MsSqlDataSettings msSqlDataSettings)
    {
        private readonly SqlConnection _connection = new(DbConnectionStringGet(msSqlDataSettings));

        public async Task<List<T>> GetEntityListAsync<T>(string sqlCommand, SqlParameter[]? parameters = null) where T : class, new()
        {
            var results = new List<T>();

            try
            {
                await using var command = new SqlCommand(sqlCommand, _connection);

                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }

                await _connection.OpenAsync();
                await using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    var dataModel = new T();

                    for (var i = 0; i < reader.FieldCount; i++)
                    {
                        var columnName = reader.GetName(i);
                        var property = typeof(T).GetProperties()
                            .FirstOrDefault(p => Attribute.IsDefined(p, typeof(ColumnAttribute)) &&
                                                 ((ColumnAttribute)Attribute.GetCustomAttribute(p, typeof(ColumnAttribute)))?.Name == columnName);

                        if (property == null || reader.IsDBNull(i))
                        {
                            continue;
                        }

                        var value = reader.GetValue(i);

                        if (property.PropertyType == typeof(DateOnly))
                        {
                            var dateValue = Convert.ToDateTime(value);
                            value = new DateOnly(dateValue.Year, dateValue.Month, dateValue.Day);
                        }
                        else if (property.PropertyType == typeof(DateOnly?))
                        {
                            var dateValue = reader.GetValue(i);
                            value = dateValue == DBNull.Value ? (DateOnly?)null : new DateOnly(((DateTime)dateValue).Year, ((DateTime)dateValue).Month, ((DateTime)dateValue).Day);
                        }
                        else if (property.PropertyType == typeof(DateTime))
                        {
                            value = Convert.ToDateTime(value);
                        }
                        else if (property.PropertyType == typeof(DateTime?))
                        {
                            value = value == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(value);
                        }
                        else
                        {
                            if (property.PropertyType != value.GetType())
                            {
                                value = Convert.ChangeType(value, property.PropertyType);
                            }
                        }

                        property.SetValue(dataModel, value);
                    }

                    results.Add(dataModel);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception in GetEntityListAsync. Couldn't get Data from SQL-Server", ex);
            }
            finally
            {
                await _connection.CloseAsync();
            }

            return results;
        }

        public async Task<T> GetEntityAsync<T>(string sqlCommand, SqlParameter[] parameters) where T : class, new()
        {
            try
            {
                await using var command = new SqlCommand(sqlCommand, _connection);

                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }

                await _connection.OpenAsync();

                var reader = await command.ExecuteReaderAsync();
                await reader.ReadAsync();

                var dataModel = new T();

                if (!reader.HasRows)
                {
                    return dataModel;
                }

                for (var i = 0; i < reader.FieldCount; i++)
                {
                    var columnName = reader.GetName(i);

                    var property = typeof(T).GetProperties()
                        .FirstOrDefault(p => Attribute.IsDefined(p, typeof(ColumnAttribute)) &&
                                              ((ColumnAttribute)Attribute.GetCustomAttribute(p, typeof(ColumnAttribute)))?.Name == columnName);

                    if (property == null)
                    {
                        continue;
                    }

                    var value = reader.IsDBNull(i) ? null : reader.GetValue(i);

                    if (property.PropertyType == typeof(DateOnly) && value is DateTime dateTimeValue)
                    {
                        value = new DateOnly(dateTimeValue.Year, dateTimeValue.Month, dateTimeValue.Day);
                    }
                    else if (property.PropertyType == typeof(DateOnly?) && value is DateTime nullableDateTimeValue)
                    {
                        value = nullableDateTimeValue == DateTime.MinValue
                            ? (DateOnly?)null
                            : new DateOnly(nullableDateTimeValue.Year, nullableDateTimeValue.Month, nullableDateTimeValue.Day);
                    }

                    property.SetValue(dataModel, value);
                }

                await reader.CloseAsync();
                await _connection.CloseAsync();

                return dataModel;
            }
            catch (Exception ex)
            {
                throw new Exception("Exception in GetEntityAsync. Couldn't get Data from SQL-Server", ex);
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async Task<T> SetEntityAsync<T>(T entity) where T : class
        {
            try
            {
                var primaryKeyProperty = typeof(T).GetProperties()
                    .FirstOrDefault(p => Attribute.IsDefined(p, typeof(KeyAttribute))) 
                                         ?? throw new Exception("Type needs to be an Entity with a primary Key");

                var tableName = typeof(T).GetCustomAttribute<TableAttribute>()?.Name
                                ?? throw new Exception("Type needs to be an Entity with a Table Attribute");

                var columnName = primaryKeyProperty.GetCustomAttribute<ColumnAttribute>()?.Name
                                 ?? throw new Exception("Type needs to be an Entity with a Column Attribute");

                var primaryKeyValue = primaryKeyProperty.GetValue(entity);

                var isPrimaryKeyNull = primaryKeyValue is null or 0;

                if (isPrimaryKeyNull)
                {
                    await InsertEntityAsync(entity, tableName);
                    return await EntityAsync<T>(tableName, columnName, primaryKeyProperty);
                }

                await _connection.OpenAsync();
                var sqlCommand = $"select * from {tableName} where {columnName} = '{primaryKeyValue}'";
                await using var command = new SqlCommand(sqlCommand, _connection);
                var reader = await command.ExecuteReaderAsync();

                var hasData = await reader.ReadAsync();
                await reader.CloseAsync();
                await _connection.CloseAsync();

                if (hasData)
                {
                    await UpdateEntityAsync(entity, tableName, columnName, primaryKeyValue.ToString());
                }
                else
                {
                    await InsertEntityAsync(entity, tableName);
                }

                return await EntityAsync<T>(tableName, columnName, primaryKeyProperty, primaryKeyValue);
            }
            catch (Exception ex)
            {
                throw new Exception("Exception in EntitySetAsync. Couldn't insert/update Data in SQL-Server", ex);
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        private async Task InsertEntityAsync<T>(T entity, string tableName) where T : class
        {
            try
            {
                var properties = entity.GetType().GetProperties().Where(p => !Attribute.IsDefined(p, typeof(KeyAttribute)));

                var columnNames = string.Join(", ", properties.Select(p => ((ColumnAttribute)Attribute.GetCustomAttribute(p, typeof(ColumnAttribute))).Name));

                var paramNames = string.Join(", ", properties.Select(p => "@" + p.Name));

                var insertCommand = $"INSERT INTO {tableName} ({columnNames}) VALUES ({paramNames})";

                await using var command = new SqlCommand(insertCommand, _connection);

                foreach (var property in properties)
                {
                    var value = property.GetValue(entity);
                    if (value is DateOnly dateOnlyValue)
                    {
                        value = dateOnlyValue.ToDateTime(TimeOnly.MinValue);
                    }

                    command.Parameters.AddWithValue("@" + property.Name, value ?? DBNull.Value);
                }

                await _connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Exception in InsertEntityAsync. Couldn't insert Data in SQL-Server", ex);
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        private async Task UpdateEntityAsync<T>(T entity, string tableName, string columnName, string primaryKeyValue) where T : class
        {
            try
            {
                var properties = entity.GetType().GetProperties().Where(p => !Attribute.IsDefined(p, typeof(KeyAttribute)));

                var setClause = string.Join(", ", properties.Select(p => $"{((ColumnAttribute)Attribute.GetCustomAttribute(p, typeof(ColumnAttribute))).Name} = @{p.Name}"));

                var updateCommand = $"UPDATE {tableName} SET {setClause} WHERE {columnName} = {primaryKeyValue}";

                await using var command = new SqlCommand(updateCommand, _connection);

                foreach (var property in properties)
                {
                    var value = property.GetValue(entity);
                    if (value is DateOnly dateOnlyValue)
                    {
                        value = dateOnlyValue.ToDateTime(TimeOnly.MinValue);
                    }

                    command.Parameters.AddWithValue("@" + property.Name, value ?? DBNull.Value);
                }

                await _connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Exception in UpdateEntityAsync. Couldn't update Data in SQL-Server", ex);
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async Task DeleteEntityAsync<T>(T entity) where T : class, new()
        {
            try
            {
                var tableName = typeof(T).GetCustomAttribute<TableAttribute>()?.Name
                                ?? throw new Exception("Type needs to be an Entity with a Table Attribute");

                var primaryKeyProperty = typeof(T).GetProperties()
                                             .FirstOrDefault(p => Attribute.IsDefined(p, typeof(KeyAttribute)))
                                         ?? throw new Exception("Type needs to be an Entity with a primary Key");

                var columnName = primaryKeyProperty.GetCustomAttribute<ColumnAttribute>()?.Name
                                 ?? throw new Exception("Type needs to be an Entity with a Column Attribute");

                var primaryKeyValue = primaryKeyProperty.GetValue(entity);

                var sqlCommand = $"DELETE FROM {tableName} WHERE {columnName} = @PrimaryKey";

                await using var command = new SqlCommand(sqlCommand, _connection);
                command.Parameters.AddWithValue("@PrimaryKey", primaryKeyValue ?? DBNull.Value);

                await _connection.OpenAsync();
                var rowsAffected = await command.ExecuteNonQueryAsync();

                if (rowsAffected == 0)
                {
                    throw new Exception("No rows affected. The entity might not exist.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception in DeleteEntityAsync. Couldn't delete Data from SQL-Server", ex);
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        private static string DbConnectionStringGet(MsSqlDataSettings msSqlDataSettings)
        {
            return $"Server={msSqlDataSettings.Server}; Database={msSqlDataSettings.Database}; User Id={msSqlDataSettings.UserId}; Password={msSqlDataSettings.Password};";
        }

        private async Task<T> EntityAsync<T>(string tableName, string columnName, PropertyInfo primaryKeyProperty, object? primaryKeyValue = null) where T : class
        {
            try
            {
                ArgumentNullException.ThrowIfNull(primaryKeyProperty);

                if (primaryKeyValue == null)
                {
                    await _connection.OpenAsync();
                    var identityQuery = $"SELECT IDENT_CURRENT('{tableName}')";
                    await using var identityCommand = new SqlCommand(identityQuery, _connection);
                    object? lastInsertedId;
                    try
                    {
                        lastInsertedId = await identityCommand.ExecuteScalarAsync();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Exception while executing scalar command.", ex);
                    }

                    if (lastInsertedId == null)
                    {
                        throw new Exception("Failed to retrieve the last inserted ID.");
                    }
                    await _connection.CloseAsync();

                    primaryKeyValue = ConvertToPrimaryKeyType(primaryKeyProperty.PropertyType, lastInsertedId);
                }

                var entity = Activator.CreateInstance<T>();
                primaryKeyProperty.SetValue(entity, primaryKeyValue);

                var sqlCommand = $"SELECT * FROM {tableName} WHERE {columnName} = @{columnName}";
                var parameters = new[]
                {
                    new SqlParameter($"@{columnName}", SqlDbType.Int) { Value = primaryKeyValue },
                };

                var getEntityMethod = typeof(SqlDataAccessor).GetMethod(
                    nameof(GetEntityAsync),
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
                    null,
                    new[] { typeof(string), typeof(SqlParameter[]) },
                    null
                );

                if (getEntityMethod == null)
                {
                    throw new Exception("Method 'GetEntityAsync' not found.");
                }

                var genericMethod = getEntityMethod.MakeGenericMethod(typeof(T));

                var result = await (Task<T>)genericMethod.Invoke(this, new object[] { sqlCommand, parameters });
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Exception in EntityAsync. Couldn't get Data from SQL-Server", ex);
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        private static object ConvertToPrimaryKeyType(Type keyType, object lastInsertedId)
        {
            if (lastInsertedId == null)
            {
                throw new ArgumentNullException(nameof(lastInsertedId));
            }

            if (keyType == typeof(int))
                return Convert.ToInt32(lastInsertedId);
            if (keyType == typeof(long))
                return Convert.ToInt64(lastInsertedId);
            if (keyType == typeof(Guid))
                return Guid.Parse(lastInsertedId.ToString());
            if (keyType == typeof(short))
                return Convert.ToInt16(lastInsertedId);
            if (keyType == typeof(byte))
                return Convert.ToByte(lastInsertedId);

            throw new InvalidOperationException($"Cannot convert value to type {keyType.Name}");
        }
    }
}
