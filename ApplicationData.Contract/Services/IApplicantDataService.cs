using WebApplication.ApplicationData.Contract.Models.DataModels;

namespace WebApplication.ApplicationData.Contract.Services
{
    public interface IApplicantDataService
    {
        public Task<List<ApplicantDataModel>> GetApplicantDataListByApplicationIdAsync(int applicationId);
        public Task<ApplicantDataModel> GetApplicantDataByApplicantIdAsync(int applicantId);
        public Task SetApplicantDataAsync(ApplicantDataModel applicantDataModel, int applicationId);
    }
}
