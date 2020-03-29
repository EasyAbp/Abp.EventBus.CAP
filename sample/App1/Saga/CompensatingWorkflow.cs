using App1.Steps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkflowCore.Interface;

namespace App1.Saga
{
    public class CompensatingWorkflow : IWorkflow
    {
        public string Id => "compensate-sample";
        public int Version => 1;

        public void Build(IWorkflowBuilder<object> builder)
        {
            builder
                .StartWith(context => Console.WriteLine("Begin"))
                .Saga(saga => saga
                    .StartWith<Task1>()
                        .CompensateWith<UndoTask1>(compensate => {
                            compensate.Input(step => step.Message, data => "undoing...");
                            })
                    .Then<Task2>()
                        .CompensateWith<UndoTask2>()
                    .Then<Task3>()
                        .CompensateWith<UndoTask3>()
                )
                    .CompensateWith<UndoEverything>(compensate => {
                        compensate.Input(step => step.Message, data => "Undo everything");
                    })
                .Then(context => Console.WriteLine("End"));
        }
    }
}
