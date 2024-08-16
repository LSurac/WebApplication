using AutoMapper;
using MediatR;
using WebApplication.Application.Models.Dto;
using WebApplication.ApplicationData.Contract.Services;

namespace WebApplication.Application.Skill.Queries.SkillListGet
{
    public class SkillListGetQueryHandler(
        ISkillDataService skillDataService,
        IMapper mapper) : IRequestHandler<SkillListGetQuery, SkillListGetQueryResult>
    {
        public async Task<SkillListGetQueryResult> Handle(
            SkillListGetQuery request,
            CancellationToken cancellationToken)
        {
            var skillDataList = await skillDataService.GetSkillDataListAsync();

            var skillDtoList = skillDataList.Select(mapper.Map<SkillDto>).ToList();

            if (!request.ApplicantId.HasValue)
            {
                return new SkillListGetQueryResult
                {
                    SkillList = skillDtoList
                };
            }

            var skillDataCurrentList = await skillDataService.GetSkillDataListByApplicantIdAsync(request.ApplicantId.Value)
                                       ?? throw new Exception($"Invalid ApplicantId {request.ApplicantId}");

            foreach (var skill in skillDtoList.Where(skill => skillDataCurrentList
                         .Any(s => s.Description != null && s.Description
                             .Equals(skill.Description))))
            {
                skill.IsCurrent = true;
            }

            return new SkillListGetQueryResult
            {
                SkillList = skillDtoList
            };
        }
    }
}
