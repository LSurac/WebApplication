using AutoMapper;
using MediatR;
using WebApplication.ApplicationData.Contract.Models.DataModels;
using WebApplication.ApplicationData.Contract.Services;

namespace WebApplication.Application.Skill.Commands.SkillSet
{
    public class SkillSetCommandHandler(
        IApplicantDataService applicantDataService,
        ISkillDataService skillDataService,
        IMapper mapper) : IRequestHandler<SkillSetCommand, SkillSetCommandResult>
    {
        public async Task<SkillSetCommandResult> Handle(
            SkillSetCommand request,
            CancellationToken cancellationToken)
        {
            var applicantDataModel = await applicantDataService.GetApplicantDataByApplicantIdAsync(request.ApplicantId)
                ?? throw new Exception($"Invalid ApplicantId {request.ApplicantId}");

            var skillDataModel = mapper.Map<SkillDataModel>(request.Skill);

            await skillDataService.SetSkillByApplicantId(applicantDataModel.Id, skillDataModel);

            return new SkillSetCommandResult();
        }
    }
}
