using WebApplication.ApplicationData.Contract.Models.DataModels;
using WebApplication.ApplicationData.Contract.Services;
using WebApplication.SqlDataAccess.Contract.Entities;
using WebApplication.SqlDataAccess.Contract.Services;

namespace WebApplication.ApplicationData.Services
{
    public class SkillDataService(
        ISkillDbService skillDbService,
        IApplicantSkillDbService applicantSkillDbService) : ISkillDataService
    {
        public async Task<List<SkillDataModel>> GetSkillDataListByApplicantIdAsync(int applicantId)
        {
            var skillList = await skillDbService.GetSkillListByApplicantIdAsync(applicantId);

            var skillDataModelList = new List<SkillDataModel>();

            foreach (var skill in skillList)
            {
                skillDataModelList.Add(await MapSkillToDataModel(skill, true));
            }

            return skillDataModelList;
        }

        public async Task SetSkillByApplicantId(
            int requestApplicantId, 
            SkillDataModel requestSkill)
        {
            var skillEntity = MapSkillDataModelToEntity(requestSkill);
            skillEntity = await skillDbService.SetSkillAsync(skillEntity);

            var applicantSkill = new ApplicantSkill
            {
                ApplicantId = requestApplicantId,
                SkillId = skillEntity.Id,
                FirstEditDate = DateTime.Now,
                LastEditDate = DateTime.Now
            };

            await applicantSkillDbService.SetApplicantSkillAsync(applicantSkill);
        }

        public async Task<List<SkillDataModel>> GetSkillDataListAsync()
        {
            var skillList = await skillDbService.GetSkillListAsync();

            var skillDataModelList = new List<SkillDataModel>();

            foreach (var skill in skillList)
            {
                skillDataModelList.Add(await MapSkillToDataModel(skill));
            }

            return skillDataModelList;
        }

        public async Task DeleteApplicantSkillByApplicantIdAndSkillId(
            int applicantEntityId, 
            int skillId)
        {
            var applicantSkill = await applicantSkillDbService.GetApplicantSkillByApplicantIdAndSkillId(applicantEntityId, skillId);

            await applicantSkillDbService.DeleteApplicantSkillByApplicantIdAndSkillId(applicantSkill);
        }

        private static Task<SkillDataModel> MapSkillToDataModel(
            Skill skill, 
            bool isCurrent = false)
        {
            return Task.FromResult(
                new SkillDataModel()
                {
                    Id = skill.Id,
                    Description = skill.Description,
                    IsCurrent = isCurrent
                });
        }

        private static Skill MapSkillDataModelToEntity(SkillDataModel skillDataModel)
        {
            var isInsert = skillDataModel.Id == 0;

            var skillEntity = new Skill
                {
                    Id = skillDataModel.Id,
                    Description = skillDataModel.Description,
                    LastEditDate = DateTime.Now
                };

            if (isInsert)
            {
                skillEntity.FirstEditDate = DateTime.Now;
            }

            return skillEntity;
        }
    }
}
