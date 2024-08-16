using WebApplication.Application.Models.Dto;

namespace WebApplication.Application.Skill.Queries.SkillListGet
{
    public class SkillListGetQueryResult
    {
        public List<SkillDto> SkillList { get; set; } = new();
    }
}
