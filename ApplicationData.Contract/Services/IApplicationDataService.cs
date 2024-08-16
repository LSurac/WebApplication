using WebApplication.ApplicationData.Contract.Models.DataModels;

namespace WebApplication.ApplicationData.Contract.Services
{
    public interface IApplicationDataService
    {
        public Task<List<ApplicationDataModel>> GetApplicationListAsync();
    }
}
