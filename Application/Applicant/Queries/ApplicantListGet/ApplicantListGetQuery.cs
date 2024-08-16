using MediatR;

namespace WebApplication.Application.Applicant.Queries.ApplicantListGet
{
    public class ApplicantListGetQuery : IRequest<ApplicantListGetQueryResult>
    {
        public int? ApplicationId { get; set; }
    }
}
