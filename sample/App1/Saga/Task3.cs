using Microsoft.Extensions.Logging;
using System;
using Volo.Abp.DependencyInjection;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace App1.Steps
{
    public class Task3 : StepBody, ITransientDependency
    {
        private ILogger logger;

        public Task3(ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.CreateLogger<Task3>();
        }

        public override ExecutionResult Run(IStepExecutionContext context)
        {
            logger.LogInformation("Doing Task 3");
            return ExecutionResult.Next();
        }
    }
}
