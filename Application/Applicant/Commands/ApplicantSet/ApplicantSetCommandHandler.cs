using AutoMapper;
using MediatR;
using WebApplication.ApplicationData.Contract.Models.DataModels;
using WebApplication.ApplicationData.Contract.Services;

namespace WebApplication.Application.Applicant.Commands.ApplicantSet
{
    public class ApplicantSetCommandHandler(
        IMapper mapper,
        IApplicantDataService applicantDataService) : IRequestHandler<ApplicantSetCommand, ApplicantSetCommandResult>
    {
        public async Task<ApplicantSetCommandResult> Handle(
            ApplicantSetCommand request,
            CancellationToken cancellationToken)
        {
            var applicantDataModel = new ApplicantDataModel
            {
                Id = request.Applicant.Id,
                FirstName = request.Applicant.FirstName,
                LastName = request.Applicant.LastName,
                BirthDate = request.Applicant.BirthDate,
                ApplicationState = request.Applicant.ApplicationState,
                SkillList = new List<SkillDataModel>()
            };

            if (request.Applicant.SkillList != null)
            {
                foreach (var skill in request.Applicant.SkillList)
                {
                    applicantDataModel.SkillList.Add(new SkillDataModel
                    {
                        Id = skill.Id,
                        Description = skill.Description,
                        IsCurrent = skill.IsCurrent
                    });
                }
            }

            await applicantDataService.SetApplicantDataAsync(applicantDataModel, request.ApplicationId);

            return new ApplicantSetCommandResult();
        }
    }
}
