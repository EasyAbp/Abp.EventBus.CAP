using Microsoft.Extensions.Logging;
using System;
using Volo.Abp.DependencyInjection;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace App1.Steps
{
    public class UndoTask3 : StepBody, ITransientDependency
    {
        private ILogger logger;

        public UndoTask3(ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.CreateLogger<UndoTask3>();
        }

        public override ExecutionResult Run(IStepExecutionContext context)
        {
            logger.LogInformation("Undoing Task 3");
            return ExecutionResult.Next();
        }
    }
}
