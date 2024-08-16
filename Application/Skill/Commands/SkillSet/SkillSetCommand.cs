using MediatR;
using WebApplication.Application.Models.Dto;

namespace WebApplication.Application.Skill.Commands.SkillSet
{
    public class SkillSetCommand : IRequest<SkillSetCommandResult>
    {
        public int ApplicantId { get; set; }
        public required SkillDto Skill { get; set; }
    }
}
