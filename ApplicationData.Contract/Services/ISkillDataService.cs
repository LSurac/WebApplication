using WebApplication.ApplicationData.Contract.Models.DataModels;

namespace WebApplication.ApplicationData.Contract.Services
{
    public interface ISkillDataService
    {
        public Task<List<SkillDataModel>> GetSkillDataListByApplicantIdAsync(int applicantId);
        public Task SetSkillByApplicantId(
            int requestApplicantId, 
            SkillDataModel requestSkill);
        public Task<List<SkillDataModel>> GetSkillDataListAsync();
        public Task DeleteApplicantSkillByApplicantIdAndSkillId(
            int applicantEntityId, 
            int skillId);
    }
}
