using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;
using WorkflowCore.Interface;

namespace App1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : AbpController
    {
        private readonly IWorkflowHost _host;

        public ValuesController(IWorkflowHost workHost)
        {
            _host = workHost;
        }

        [HttpGet]
        public async Task<string> AppMessage()
        {
            var workflowId = await _host.StartWorkflow("compensate-sample");
            return workflowId;
        }        
    }
}