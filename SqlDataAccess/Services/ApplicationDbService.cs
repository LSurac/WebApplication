using WebApplication.SqlDataAccess.Contract.Entities;
using WebApplication.SqlDataAccess.Contract.Services;

namespace WebApplication.SqlDataAccess.Services
{
    public class ApplicationDbService(SqlDataAccessor sqlDataAccessor) : IApplicationDbService
    {
        public Task<List<Application>> GetApplicationListAsync()
        {
            const string dbCommand = "SELECT * FROM application";

            return sqlDataAccessor.GetEntityListAsync<Application>(dbCommand);
        }
    }
}
