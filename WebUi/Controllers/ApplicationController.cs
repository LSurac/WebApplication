using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Application.Application.Queries.ApplicationListGet;

namespace WebApplication.WebUi.Controllers
{
    [ApiController]
    public class ApplicationController() : BaseController<ApplicationController>
    {
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApplicationListGetQueryResult), StatusCodes.Status200OK)]
        public async Task<ActionResult> ApplicationListGet(ApplicationListGetQuery applicationListGetQuery)
        {
            return Ok(
                await Mediator.Send(
                    applicationListGetQuery
                )
            );
        }
    }
}
