using MediatR;

namespace WebApplication.Application.Applicant.Queries.ApplicantGet
{
    public class ApplicantGetQuery : IRequest<ApplicantGetQueryResult>
    {
        public int? ApplicantId { get; set; }
    }
}
