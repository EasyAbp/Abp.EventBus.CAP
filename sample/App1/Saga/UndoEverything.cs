using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace App1.Saga
{
    public class UndoEverything : StepBody, ITransientDependency
    {
        private ILogger logger;

        public UndoEverything(ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.CreateLogger<UndoEverything>();
        }

        public string Message { get; set; }

        public override ExecutionResult Run(IStepExecutionContext context)
        {
            logger.LogInformation(Message);
            return ExecutionResult.Next();
        }
    }
}
