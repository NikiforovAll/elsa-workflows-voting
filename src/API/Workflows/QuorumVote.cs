using Elsa.Extensions;
using Elsa.Workflows;
using Elsa.Workflows.Models;

namespace Elsa.Demo.API.Workflows;

public class QuorumVote : CodeActivity<bool>
{
    public Input<IEnumerable<bool>> Votes { get; set; } = default!;

    protected override void Execute(ActivityExecutionContext context)
    {
        var votes = Votes.Get(context);

        var totalVotes = votes.Count();
        var yesVotes = votes.Count(vote => vote);
        var noVotes = totalVotes - yesVotes;
        var quorum = totalVotes / 2 + 1;

        var value = yesVotes >= quorum;

        context.SetResult(value);
    }
}
