using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace App1.Steps
{
    public class Task1 : StepBody, ITransientDependency
    {
        private readonly IApp1MessagingService _app1Message;

        public Task1(IApp1MessagingService app1Message)
        {
            _app1Message = app1Message;
        }

        public override ExecutionResult Run(IStepExecutionContext context)
        {
            _app1Message.RunAsync($"App1.Message { DateTime.Now}");

            return ExecutionResult.Next();
        }
    }
}
