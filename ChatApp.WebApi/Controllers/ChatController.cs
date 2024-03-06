﻿using ChatApp.Application.Interfaces.Services;
using ChatApp.WebApi.Controllers.Common;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : BaseApiController
    {
        private readonly IEmailService _emailService;

        public ChatController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("sendEmail")]
        public async Task<IActionResult> SendEmail(Guid userId)
        {
            await _emailService.SendUserActivationEmail(userId);
            return Ok();
        }
    }
}
