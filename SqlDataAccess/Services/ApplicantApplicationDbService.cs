using System.Data;
using System.Data.SqlClient;
using WebApplication.SqlDataAccess.Contract.Entities;
using WebApplication.SqlDataAccess.Contract.Services;

namespace WebApplication.SqlDataAccess.Services
{
    public class ApplicantApplicationDbService(SqlDataAccessor sqlDataAccessor) : IApplicantApplicationDbService
    {
        public Task<ApplicantApplication> GetApplicantApplicationByApplicantIdAndApplicationIdAsync(
            int applicantId,
            int applicationId)
        {
            const string dbCommand = "SELECT * FROM applicantapplication " +
                                     "WHERE apa_app_id = @applicantId " +
                                     "AND apa_apl_id = @applicationId";

            var parameters = new[]
            {
                new SqlParameter("@applicationId", SqlDbType.Int) { Value = applicationId },
                new SqlParameter("@applicantId", SqlDbType.Int) { Value = applicantId },
            };

            return sqlDataAccessor.GetEntityAsync<ApplicantApplication>(dbCommand, parameters);
        }

        public Task<ApplicantApplication> SetApplicantApplicationAsync(ApplicantApplication applicantApplicationEntity)
        {
            return sqlDataAccessor.SetEntityAsync(applicantApplicationEntity);
        }
    }
}
