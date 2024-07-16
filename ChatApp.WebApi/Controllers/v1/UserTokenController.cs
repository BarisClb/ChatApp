using Asp.Versioning;
using ChatApp.Application.Interfaces.Services;
using ChatApp.WebApi.Controllers.Common;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.WebApi.Controllers.v1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
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
