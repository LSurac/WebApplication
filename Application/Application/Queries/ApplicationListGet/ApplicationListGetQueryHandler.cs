using AutoMapper;
using MediatR;
using WebApplication.Application.Models.Dto;
using WebApplication.ApplicationData.Contract.Services;

namespace WebApplication.Application.Application.Queries.ApplicationListGet
{
    public class ApplicationListGetQueryHandler(
        IApplicationDataService applicationDataService,
        IMapper mapper) : IRequestHandler<ApplicationListGetQuery, ApplicationListGetQueryResult>
    {
        public async Task<ApplicationListGetQueryResult> Handle(
            ApplicationListGetQuery request,
            CancellationToken cancellationToken)
        {
            var result = new ApplicationListGetQueryResult();
            var applicationDataModelList = await applicationDataService.GetApplicationListAsync();
            var applicationList = applicationDataModelList.Select(mapper.Map<ApplicationDto>).ToList();

            result.ApplicationList = applicationList;

            return result;
        }
    }
}
