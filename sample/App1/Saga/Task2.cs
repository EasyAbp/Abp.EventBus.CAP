using System;
using WorkflowCore.Interface;
using WorkflowCore.Models;
using Volo.Abp.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace App1.Steps
{
    public class Task2 : StepBody,ITransientDependency
    {
        private ILogger logger;

        public Task2(ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.CreateLogger<Task2>();
        }

        public override ExecutionResult Run(IStepExecutionContext context)
        {
            logger.LogInformation("Doing Task 2");
            throw new Exception();
        }
    }
}
