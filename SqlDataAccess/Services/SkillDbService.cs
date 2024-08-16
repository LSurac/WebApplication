using System.Data;
using System.Data.SqlClient;
using WebApplication.SqlDataAccess.Contract.Entities;
using WebApplication.SqlDataAccess.Contract.Services;

namespace WebApplication.SqlDataAccess.Services
{
    public class SkillDbService(SqlDataAccessor sqlDataAccessor) : ISkillDbService
    {
        public Task<List<Skill>> GetSkillListByApplicantIdAsync(int applicantId)
        {
            const string dbCommand = "SELECT skill.* FROM skill, applicantskill " +
                                     "WHERE skl_id = ask_skl_id " +
                                     "AND ask_app_id = @applicantId";

            var parameters = new[]
            {
                new SqlParameter("@applicantId", SqlDbType.Int) { Value = applicantId }
            };

            return sqlDataAccessor.GetEntityListAsync<Skill>(dbCommand, parameters);
        }

        public Task<Skill> SetSkillAsync(Skill skillEntity)
        {
            return sqlDataAccessor.SetEntityAsync(skillEntity);
        }

        public Task<List<Skill>> GetSkillListAsync()
        {
            const string dbCommand = "SELECT * FROM skill ";

            return sqlDataAccessor.GetEntityListAsync<Skill>(dbCommand);
        }
    }
}
