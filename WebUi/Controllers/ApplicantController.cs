using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Application.Applicant.Commands.ApplicantSet;
using WebApplication.Application.Applicant.Queries.ApplicantGet;
using WebApplication.Application.Applicant.Queries.ApplicantListGet;

namespace WebApplication.WebUi.Controllers
{
    [ApiController]
    public class ApplicantController() : BaseController<ApplicantController>
    {
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApplicantGetQueryResult), StatusCodes.Status200OK)]
        public async Task<ActionResult> ApplicantGet(ApplicantGetQuery applicantGetQuery)
        {
            return Ok(
                await Mediator.Send(
                    applicantGetQuery
                )
            );
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApplicantListGetQueryResult), StatusCodes.Status200OK)]
        public async Task<ActionResult> ApplicantListGet(ApplicantListGetQuery applicantListGetQuery)
        {
            return Ok(
                await Mediator.Send(
                    applicantListGetQuery
                )
            );
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApplicantSetCommandResult), StatusCodes.Status200OK)]
        public async Task<ActionResult> ApplicantSet(ApplicantSetCommand applicantSetCommand)
        {
            return Ok(
                await Mediator.Send(
                    applicantSetCommand
                )
            );
        }
    }
}
