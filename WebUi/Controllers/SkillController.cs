using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Application.Skill.Commands.SkillSet;
using WebApplication.Application.Skill.Queries.SkillListGet;

namespace WebApplication.WebUi.Controllers
{
    [ApiController]
    public class SkillController() : BaseController<SkillController>
    {
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(SkillListGetQueryResult), StatusCodes.Status200OK)]
        public async Task<ActionResult> SkillListGet(SkillListGetQuery skillListGetQuery)
        {
            return Ok(
                await Mediator.Send(
                    skillListGetQuery
                )
            );
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(SkillSetCommandResult), StatusCodes.Status200OK)]
        public async Task<ActionResult> SkillSet(SkillSetCommand skillSetCommand)
        {
            return Ok(
                await Mediator.Send(
                    skillSetCommand
                )
            );
        }
    }
}
