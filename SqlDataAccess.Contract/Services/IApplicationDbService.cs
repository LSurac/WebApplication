using WebApplication.SqlDataAccess.Contract.Entities;

namespace WebApplication.SqlDataAccess.Contract.Services
{
    public interface IApplicationDbService
    {
        public Task<List<Application>> GetApplicationListAsync();
    }
}
