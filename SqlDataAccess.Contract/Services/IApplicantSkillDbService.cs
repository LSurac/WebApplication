using WebApplication.SqlDataAccess.Contract.Entities;

namespace WebApplication.SqlDataAccess.Contract.Services
{
    public interface IApplicantSkillDbService
    {
        public Task<ApplicantSkill> SetApplicantSkillAsync(ApplicantSkill applicantSkill);
        public Task DeleteApplicantSkillByApplicantIdAndSkillId(ApplicantSkill applicantSkill);
        public Task<ApplicantSkill> GetApplicantSkillByApplicantIdAndSkillId(int applicantEntityId, int skillId);
    }
}
