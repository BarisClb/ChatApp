using ChatApp.Application.Interfaces.Services;
using ChatApp.Application.Models.Auth;
using ChatApp.Application.Models.Responses.Common;
using ChatApp.Application.Resources;
using ChatApp.WebApi.Controllers.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace ChatApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : BaseApiController
    {
        private readonly ICacheService _cacheService;
        private readonly IStringLocalizer<LocalizationResources> _localizer;

        public TestController(ICacheService cacheService, IStringLocalizer<LocalizationResources> localizer)
        {
            _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
            _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
        }


        [HttpPost("add")]
        public async Task<IActionResult> AddValue(string key, string value)
        {
            return Ok(await _cacheService.SetValueAsync(key, value, null));
        }

        [HttpPost("addUnderFolder")]
        public async Task<IActionResult> AddValueUnderFolder(string key, string folderName, string value)
        {
            return Ok(await _cacheService.SetValueUnderFolderAsync(key, folderName, value, null));
        }

        [HttpPost("get")]
        public async Task<IActionResult> GetValue(string key)
        {
            return Ok(await _cacheService.GetValueAsync<string>(key));
        }

        [HttpPost("remove")]
        public async Task<IActionResult> RemoveValue(string key)
        {
            return Ok(await _cacheService.DeleteValueAsync(key));
        }

        [HttpPost("removeByPattern")]
        public async Task<IActionResult> RemoveByPattern(string pattern)
        {
            await _cacheService.DeleteByPatternAsync(pattern);
            return Ok();
        }

        [HttpGet("getCurrentUser")]
        public async Task<IActionResult> GetCurrentUser()
        {
            return Ok(_currentUser);
        }

        [HttpPost("getCurrentUserWithPost")]
        public async Task<IActionResult> GetCurrentUserWithPost()
        {
            ApiResponse<CurrentUser> response = new() { Data = _currentUser, IsSuccess = true, StatusCode = 200 };
            return Ok(response);
        }

        [HttpPost("getCurrentUserWithRequest")]
        public async Task<IActionResult> GetCurrentUserWithRequest(TestRequestModel request)
        {
            if (request == null || request.Field1 != "Value1" || request.Field2 != "Value2")
                return BadRequest();

            return Ok(_currentUser);
        }

        [HttpPost("getLocalization")]
        public async Task<IActionResult> GetLocalization(string keyword)
        {
            var value = _localizer[keyword];
            return Ok(value);
        }

        [HttpPost("throwError")]
        public async Task<IActionResult> ThrowError()
        {
            string? guidTest = null;
            if (Guid.TryParse(guidTest, out Guid parsedGuidTest))
                await Console.Out.WriteLineAsync("true");
            else
                await Console.Out.WriteLineAsync("false");

            throw new Exception();
        }

        [HttpPost("test")]
        public async Task<IActionResult> Test()
        {
            List<string> list1 = new() { "en", "tr" };
            List<(string name, string code)> list2 = new() { ("english", "en"), ("turkish", "tr") };

            list2 = list2.Where(x => list1.Contains(x.code)).ToList();

            return Ok();
        }

        public class TestRequestModel
        {
            public string Field1 { get; set; }
            public string Field2 { get; set; }
        }
    }
}
