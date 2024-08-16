using AutoMapper;
using MediatR;
using WebApplication.Application.Models.Dto;
using WebApplication.ApplicationData.Contract.Services;

namespace WebApplication.Application.Applicant.Queries.ApplicantListGet
{
    public class ApplicantListGetQueryHandler(
        IApplicantDataService applicantDataService,
        IMapper mapper) : IRequestHandler<ApplicantListGetQuery, ApplicantListGetQueryResult>
    {
        public async Task<ApplicantListGetQueryResult> Handle(
            ApplicantListGetQuery request,
            CancellationToken cancellationToken)
        {
            if (!request.ApplicationId.HasValue)
            {
                throw new Exception("ApplicantId is missing");
            }

            var result = new ApplicantListGetQueryResult();
            var applicantDataModelList = await applicantDataService.GetApplicantDataListByApplicationIdAsync(request.ApplicationId.Value)
                                    ?? throw new Exception($"couldn't find applicant for Application with Id = {request.ApplicationId}");
            var applicantList = applicantDataModelList.Select(mapper.Map<ApplicantDto>).ToList();

            result.ApplicantList = applicantList;

            return result;
        }
    }
}
