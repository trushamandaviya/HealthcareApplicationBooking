using HC.Core.Admin.Interfaces;
using HC.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace HC.API.Area.Admin.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {     
        private readonly IAccount _accountService;

        public AccountController(IAccount accountService)
        {
            _accountService = accountService;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserModel model)
        {
            var token = await _accountService.RegisterUserAsync(model);
            return Ok(new { Token = token });            
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var token = await _accountService.LoginAsync(model);
            return Ok(new { Token = token });            
        }
    }
}
