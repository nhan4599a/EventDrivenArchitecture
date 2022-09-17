using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Controllers
{
    [ApiController]
    [Route("api/[controller]s")]
    public abstract class BaseController : ControllerBase
    {
        protected ILogger Logger { get; set; }

        protected BaseController(ILogger logger)
        {
            Logger = logger;
        }

        protected IActionResult CreateSucceedResult()
        {
            return Ok();
        }
    }
}
