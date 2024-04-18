using Elsa.Extensions;
using Elsa.Workflows;
using Elsa.Workflows.Activities;
using Elsa.Workflows.Contracts;
using Elsa.Workflows.Management.Activities.SetOutput;
using Elsa.Workflows.Models;
using Elsa.Workflows.Runtime.Activities;

namespace Elsa.Demo.API.Workflows;

public class SimpleVoteWorkflow : WorkflowBase
{
    protected override void Build(IWorkflowBuilder builder)
    {
        var user = builder.WithInput<User>("user");
        var currentVoteResult = builder.WithVariable<bool>();

        builder.Root = new Sequence
        {
            Activities =
            {
                new RunTask("PromptVote")
                {
                    Result = new Output<object>(currentVoteResult),
                    Payload = new(context => new Dictionary<string, object>
                    {
                        ["User"] = context.GetInput<string>(user),
                    })
                },
                new SetOutput
                {
                    OutputName = new("Vote"),
                    OutputValue = new(context => currentVoteResult.Get(context))
                }
            },
        };
    }
}
