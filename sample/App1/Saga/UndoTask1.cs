using Microsoft.Extensions.Logging;
using System;
using Volo.Abp.DependencyInjection;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace App1.Steps
{
    public class UndoTask1 : StepBody, ITransientDependency
    {
        private ILogger logger;

        public UndoTask1(ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.CreateLogger<UndoTask1>();
        }

        public string Message { get; set; }

        public override ExecutionResult Run(IStepExecutionContext context)
        {
            logger.LogInformation(Message );
            return ExecutionResult.Next();
        }
    }
}
