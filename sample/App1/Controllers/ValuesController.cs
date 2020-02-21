using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;

namespace App1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : AbpController
    {
        private readonly IApp1MessagingService _app1Message;

        public ValuesController(IApp1MessagingService app1Message)
        {
            _app1Message = app1Message;
        }

        [HttpGet]
        public async Task<IActionResult> AppMessage()
        {
            await _app1Message.RunAsync($"App1.Message { DateTime.Now}");

            return Ok();
        }

    }
}