using WebApplication.SqlDataAccess.Contract.Entities;

namespace WebApplication.SqlDataAccess.Contract.Services
{
    public interface IApplicantDbService
    {
        public Task<List<Applicant>> GetApplicantListByApplicationIdAsync(int applicationId);
        public Task<Applicant> GetApplicantByIdAsync(int applicantId);
        public Task<Applicant> SetApplicantAsync(Applicant applicantEntity);
    }
}
