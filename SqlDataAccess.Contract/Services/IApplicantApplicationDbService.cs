using WebApplication.SqlDataAccess.Contract.Entities;

namespace WebApplication.SqlDataAccess.Contract.Services
{
    public interface IApplicantApplicationDbService
    {
        public Task<ApplicantApplication> GetApplicantApplicationByApplicantIdAndApplicationIdAsync(
            int applicantId,
            int applicationId);

        public Task<ApplicantApplication> SetApplicantApplicationAsync(ApplicantApplication applicantApplicationEntity);
    }
}
