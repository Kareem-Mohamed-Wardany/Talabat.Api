using Talabat.APIs.Errors;
using Microsoft.AspNetCore.Mvc;

namespace Talabat.APIs.Controllers
{
    [Route("errors/{code}")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorsController : ControllerBase
    {
        public ActionResult Error(int code)
        {
            if (code == 404)
                return NotFound(new ApiResponse(400));
            else if (code == 401)
                return Unauthorized(new ApiResponse(401));
            else
                return StatusCode(code);
        }
    }
}
