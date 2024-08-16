using WebApplication.SqlDataAccess.Contract.Entities;

namespace WebApplication.SqlDataAccess.Contract.Services
{
    public interface ISkillDbService
    {
        public Task<List<Skill>> GetSkillListByApplicantIdAsync(int applicantId);
        public Task<Skill> SetSkillAsync(Skill skillEntity);
        public Task<List<Skill>> GetSkillListAsync();
    }
}
