using MediatR;
using WebApplication.Application.Models.Dto;

namespace WebApplication.Application.Applicant.Commands.ApplicantSet
{
    public class ApplicantSetCommand : IRequest<ApplicantSetCommandResult>
    {
        public required ApplicantDto Applicant { get; set; }
        public int ApplicationId { get; set; }
    }
}
