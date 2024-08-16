using AutoMapper;
using MediatR;
using WebApplication.Application.Models.Dto;
using WebApplication.ApplicationData.Contract.Services;

namespace WebApplication.Application.Applicant.Queries.ApplicantGet
{
    public class ApplicantGetQueryHandler(
        IApplicantDataService applicantDataService,
        IMapper mapper) : IRequestHandler<ApplicantGetQuery, ApplicantGetQueryResult>
    {
        public async Task<ApplicantGetQueryResult> Handle(
            ApplicantGetQuery request,
            CancellationToken cancellationToken)
        {
            if (!request.ApplicantId.HasValue)
            {
                throw new Exception("ApplicantId is missing");
            }

            var result = new ApplicantGetQueryResult();
            var applicantDataModel = await applicantDataService.GetApplicantDataByApplicantIdAsync(request.ApplicantId.Value)
                                    ?? throw new Exception($"couldn't find applicant with Id = {request.ApplicantId}");

            result.Applicant = mapper.Map<ApplicantDto>(applicantDataModel);

            return result;
        }
    }
}
