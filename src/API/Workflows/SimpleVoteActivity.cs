using Elsa.Extensions;
using Elsa.Workflows;
using Elsa.Workflows.Activities;
using Elsa.Workflows.Memory;
using Elsa.Workflows.Models;
using Elsa.Workflows.Runtime.Activities;

namespace Elsa.Demo.API.Workflows;

public class SimpleVoteActivity : Composite<bool>
{
    private Variable<bool> _currentVoteResult = new();
    public Input<User> User { get; set; }

    public SimpleVoteActivity()
    {
        Root = new Sequence
        {
            Activities =
            {
                new RunTask("PromptVote")
                {
                    Result = new Output<object>(_currentVoteResult),
                    Payload = new(context => new Dictionary<string, object>
                    {
                        // doesn't work - NRE
                        ["User"] = User.Get(context),
                    })
                },
            }
        };
    }

    protected override void OnCompleted(ActivityCompletedContext context)
    {
        context.TargetContext.Set(Result, _currentVoteResult.Get(context.TargetContext));
    }
}
