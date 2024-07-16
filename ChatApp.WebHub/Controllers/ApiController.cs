using Microsoft.AspNetCore.Mvc;

namespace ChatApp.WebHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {

        [HttpGet("test")]
        public async Task<IActionResult> Test()
        {
            return Ok(new { IsSuccess = true, Message = "DECLINE" });
        }
    }
}
