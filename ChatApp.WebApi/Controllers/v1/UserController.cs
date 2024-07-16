using Asp.Versioning;
using ChatApp.Application.Commands.User;
using ChatApp.Application.Queries.User;
using ChatApp.WebApi.Controllers.Common;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.WebApi.Controllers.v1
{
    [Route("api/v{version:apiVersion}/user")]
    [ApiController]
    [ApiVersion("1.0")]
    public class UserController : BaseApiController
    {
        [HttpPost("register")]
        public async Task<IActionResult> UserRegister(UserRegisterCommand userRegisterCommand)
        {
            return Ok(await _mediator.Send(userRegisterCommand));
        }

        [HttpPost("logIn")]
        public async Task<IActionResult> UserLogIn(UserLogInCommand userLogInCommand)
        {
            return Ok(await _mediator.Send(userLogInCommand));
        }

        [HttpPost("logInWithSession")]
        public async Task<IActionResult> UserLogInWithSession(UserLogInWithSessionCommand userLogInWithSessionCommand)
        {
            return Ok(await _mediator.Send(userLogInWithSessionCommand));
        }

        [HttpPost("logOut")]
        public async Task<IActionResult> UserLogOut()
        {
            return Ok(await _mediator.Send(new UserLogOutCommand()));
        }

        [HttpGet("userByUsername")]
        public async Task<IActionResult> GetUserByUsername([FromQuery] string username)
        {
            return Ok(await _mediator.Send(new GetUserByUsernameQuery() { Username = username }));
        }

        [HttpGet("testtt")]
        public async Task<IActionResult> Testt()
        {
            return Ok(await _mediator.Send(new GetUserAndUserRoleByUserIdQuery() { UserId = Guid.Empty }));
        }
    }
}
