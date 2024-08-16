using System.Data;
using System.Data.SqlClient;
using WebApplication.SqlDataAccess.Contract.Entities;
using WebApplication.SqlDataAccess.Contract.Services;

namespace WebApplication.SqlDataAccess.Services
{
    public class ApplicantSkillDbService(SqlDataAccessor sqlDataAccessor) : IApplicantSkillDbService
    {
        public async Task<ApplicantSkill> SetApplicantSkillAsync(ApplicantSkill applicantSkill)
        {
            return await sqlDataAccessor.SetEntityAsync(applicantSkill);
        }

        public async Task DeleteApplicantSkillByApplicantIdAndSkillId(ApplicantSkill applicantSkill)
        {
            await sqlDataAccessor.DeleteEntityAsync(applicantSkill);
        }

        public Task<ApplicantSkill> GetApplicantSkillByApplicantIdAndSkillId(int applicantEntityId, int skillId)
        {
            const string dbCommand = "SELECT * FROM applicantskill " +
                                     "WHERE ask_app_id = @applicantId " +
                                     "AND ask_skl_id = @skillId";

            var parameters = new[]
            {
                new SqlParameter("@applicantId", SqlDbType.Int) { Value = applicantEntityId },
                new SqlParameter("@skillId", SqlDbType.Int) { Value = skillId },
            };

            return sqlDataAccessor.GetEntityAsync<ApplicantSkill>(dbCommand, parameters);
        }
    }
}
