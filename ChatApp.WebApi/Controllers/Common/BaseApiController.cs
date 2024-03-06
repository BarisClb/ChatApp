using ChatApp.Application.Models.Auth;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.WebApi.Controllers.Common
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        internal IMediator _mediator => HttpContext.RequestServices.GetRequiredService<IMediator>();
        internal CurrentUser _currentUser => HttpContext.RequestServices.GetRequiredService<CurrentUser>();
    }
}
