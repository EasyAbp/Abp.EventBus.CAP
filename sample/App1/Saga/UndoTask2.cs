using Microsoft.Extensions.Logging;
using System;
using Volo.Abp.DependencyInjection;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace App1.Steps
{
    public class UndoTask2 : StepBody, ITransientDependency
    {
        private ILogger logger;

        public UndoTask2(ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.CreateLogger<UndoTask2>();
        }

        public override ExecutionResult Run(IStepExecutionContext context)
        {
            logger.LogInformation("Undoing Task 2");
            return ExecutionResult.Next();
        }
    }
}
