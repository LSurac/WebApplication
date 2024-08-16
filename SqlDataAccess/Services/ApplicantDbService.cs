using System.Data;
using System.Data.SqlClient;
using WebApplication.SqlDataAccess.Contract.Entities;
using WebApplication.SqlDataAccess.Contract.Services;

namespace WebApplication.SqlDataAccess.Services
{
    public class ApplicantDbService(SqlDataAccessor sqlDataAccessor) : IApplicantDbService
    {
        public Task<List<Applicant>> GetApplicantListByApplicationIdAsync(int applicationId)
        {
            const string dbCommand = "SELECT applicant.* FROM applicant, applicantapplication " +
                                     "WHERE app_id = apa_app_id " +
                                     "AND apa_apl_id = @applicationId";

            var parameters = new[]
            {
                new SqlParameter("@applicationId", SqlDbType.Int) { Value = applicationId },
            };

            return sqlDataAccessor.GetEntityListAsync<Applicant>(dbCommand, parameters);
        }

        public Task<Applicant> GetApplicantByIdAsync(int applicantId)
        {
            const string dbCommand = "SELECT * FROM applicant WHERE app_id = @applicantId";

            var parameters = new[]
            {
                new SqlParameter("@applicantId", SqlDbType.Int) { Value = applicantId },
            };

            return sqlDataAccessor.GetEntityAsync<Applicant>(dbCommand, parameters);
        }

        public Task<Applicant> SetApplicantAsync(Applicant applicantEntity)
        {
            return sqlDataAccessor.SetEntityAsync(applicantEntity);
        }
    }
}
