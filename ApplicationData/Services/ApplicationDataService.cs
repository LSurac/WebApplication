using WebApplication.ApplicationData.Contract.Models.DataModels;
using WebApplication.ApplicationData.Contract.Services;
using WebApplication.SqlDataAccess.Contract.Entities;
using WebApplication.SqlDataAccess.Contract.Services;

namespace WebApplication.ApplicationData.Services
{
    public class ApplicationDataService(
        IApplicationDbService applicationDbService,
        IApplicantDataService applicantDataService) : IApplicationDataService
    {
        public async Task<List<ApplicationDataModel>> GetApplicationListAsync()
        {
            var applicationList = await applicationDbService.GetApplicationListAsync();

            var applicationDataModelList = new List<ApplicationDataModel>();

            foreach (var application in applicationList)
            {
                var applicantList = await applicantDataService.GetApplicantDataListByApplicationIdAsync(application.Id);
                applicationDataModelList.Add(await MapApplicationToDataModel(application, applicantList));
            }

            return applicationDataModelList;
        }

        private static Task<ApplicationDataModel> MapApplicationToDataModel(
            Application application,
            List<ApplicantDataModel> applicantList)
        {
            return Task.FromResult(
                new ApplicationDataModel
                {
                    Id = application.Id,
                    Description = application.Description,
                    ApplicantList = applicantList,
                });
        }
    }
}