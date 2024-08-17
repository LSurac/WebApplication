using WebApplication.ApplicationData.Contract.Models.DataModels;
using WebApplication.ApplicationData.Contract.Models.Enums;
using WebApplication.ApplicationData.Contract.Services;
using WebApplication.SqlDataAccess.Contract.Entities;
using WebApplication.SqlDataAccess.Contract.Services;

namespace WebApplication.ApplicationData.Services
{
    public class ApplicantDataService(
        IApplicantDbService applicantDbService,
        IApplicantApplicationDbService applicantApplicationDbService,
        ISkillDataService skillDataService) : IApplicantDataService
    {
        public async Task<List<ApplicantDataModel>> GetApplicantDataListByApplicationIdAsync(int applicationId)
        {
            var applicantList = await applicantDbService.GetApplicantListByApplicationIdAsync(applicationId);

            var applicantDataModelList = new List<ApplicantDataModel>();

            foreach (var applicant in applicantList)
            {
                var applicantState = await GetApplicationStateAsync(applicant.Id, applicationId);
                var skillList = await skillDataService.GetSkillDataListByApplicantIdAsync(applicant.Id);
                applicantDataModelList.Add(MapApplicantToDataModel(applicant, applicantState, skillList));
            }

            return applicantDataModelList;
        }

        public async Task<ApplicantDataModel> GetApplicantDataByApplicantIdAsync(int applicantId)
        {
            var applicantEntity = await applicantDbService.GetApplicantByIdAsync(applicantId);

            return MapApplicantToDataModel(applicantEntity, null, null);
        }

        public async Task SetApplicantDataAsync(
            ApplicantDataModel applicantDataModel, 
            int applicationId)
        {
            var isInsert = applicantDataModel.Id == 0;

            var applicantEntity = new Applicant
            {
                Id = applicantDataModel.Id,
                FirstName = applicantDataModel.FirstName,
                LastName = applicantDataModel.LastName,
                BirthDate = applicantDataModel.BirthDate,
                LastEditDate = DateTime.Now
            };

            if (isInsert)
            {
                applicantEntity.FirstEditDate = DateTime.Now;
            }

            applicantEntity = await applicantDbService.SetApplicantAsync(applicantEntity);

            var currentSkillList = await skillDataService.GetSkillDataListByApplicantIdAsync(applicantEntity.Id);

            if (applicantDataModel.SkillList != null)
            {
                var currentSkillDictionary = currentSkillList.ToDictionary(skill => skill.Id);
                var incomingSkillDictionary = applicantDataModel.SkillList.ToDictionary(skill => skill.Id);

                foreach (var incomingSkill in applicantDataModel.SkillList.Where(incomingSkill => !currentSkillDictionary.ContainsKey(incomingSkill.Id)))
                {
                    await skillDataService.SetSkillByApplicantId(applicantEntity.Id, incomingSkill);
                }

                foreach (var currentSkill in currentSkillList.Where(currentSkill => !incomingSkillDictionary.ContainsKey(currentSkill.Id)))
                {
                    await skillDataService.DeleteApplicantSkillByApplicantIdAndSkillId(applicantEntity.Id, currentSkill.Id);
                }
            }

            await SetApplicantApplicationAsync(applicantDataModel, applicationId, isInsert, applicantEntity);
        }

        private async Task SetApplicantApplicationAsync(
            ApplicantDataModel applicantDataModel, 
            int applicationId,
            bool isInsert, 
            Applicant applicantEntity)
        {
            ApplicantApplication applicantApplicationEntity;

            if (isInsert)
            {
                applicantApplicationEntity = new ApplicantApplication
                {
                    ApplicantId = applicantEntity.Id,
                    ApplicationId = applicationId,
                    FirstEditDate = DateTime.Now
                };
            }
            else
            {
                applicantApplicationEntity = await applicantApplicationDbService.GetApplicantApplicationByApplicantIdAndApplicationIdAsync(
                    applicantDataModel.Id,
                    applicationId);
            }

            applicantApplicationEntity.LastEditDate = DateTime.Now;
            applicantApplicationEntity.State = applicantDataModel.ApplicationState.ToString();

            await applicantApplicationDbService.SetApplicantApplicationAsync(applicantApplicationEntity);
        }

        private async Task<EApplicationState?> GetApplicationStateAsync(
            int applicantId, 
            int applicationId)
        {
            var applicantApplicationEntity = await applicantApplicationDbService.GetApplicantApplicationByApplicantIdAndApplicationIdAsync(applicantId, applicationId);

            var parseSucceed = Enum.TryParse<EApplicationState>(applicantApplicationEntity.State, out var applicantStateEnum);

            return parseSucceed ? applicantStateEnum : null;
        }

        private static ApplicantDataModel MapApplicantToDataModel(
            Applicant applicantEntity, 
            EApplicationState? applicantState,
            List<SkillDataModel>? skillList)
        {
            var employeeDataModel = new ApplicantDataModel
            {
                Id = applicantEntity.Id,
                FirstName = applicantEntity.FirstName,
                LastName = applicantEntity.LastName,
                BirthDate = applicantEntity.BirthDate,
                ApplicationState = applicantState,
                SkillList = skillList,

            };
            return employeeDataModel;
        }
    }
}
