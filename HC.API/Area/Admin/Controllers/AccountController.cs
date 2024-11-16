using HC.Core.Admin.Interfaces;
using HC.Core.Helpers;
using HC.Core.Models;
using HC.DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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
    }
}
