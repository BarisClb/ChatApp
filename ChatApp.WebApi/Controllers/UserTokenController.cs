using ChatApp.Application.Interfaces.Services;
using ChatApp.Application.Models.Requests;
using ChatApp.WebApi.Controllers.Common;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserTokenController : BaseApiController
    {
        private readonly ITokenService _tokenService;

        public UserTokenController(ITokenService tokenService)
        {
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        }


        [HttpPost("refreshAccessToken")]
        public async Task<IActionResult> RefreshAccessToken()
        {
            return Ok(await _tokenService.RefreshAccessToken());
        }
    }
}
