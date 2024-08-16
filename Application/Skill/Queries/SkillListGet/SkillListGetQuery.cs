using MediatR;

namespace WebApplication.Application.Skill.Queries.SkillListGet
{
    public class SkillListGetQuery : IRequest<SkillListGetQueryResult>
    {
        public int? ApplicantId { get; set; }
    }
}
