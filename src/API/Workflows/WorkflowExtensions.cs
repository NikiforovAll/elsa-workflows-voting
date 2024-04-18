using Elsa.Expressions.Models;
using Elsa.Extensions;
using Elsa.Workflows.Memory;

namespace Elsa.Demo.API.Workflows;

public static class WorkflowExtensions
{
    public static Func<ExpressionExecutionContext, Dictionary<string, bool>> SetVotes(
        Variable<bool> currentVoteResult,
        Variable<Dictionary<string, bool>> initialVotes
    )
    {
        return context =>
        {
            var votes = initialVotes.Get(context);
            var vote = currentVoteResult.Get(context);
            var index = context.GetVariable<int>("CurrentIndex").ToString();

            votes[index] = vote;

            return votes;
        };
    }
}
