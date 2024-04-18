using Elsa.Extensions;
using Elsa.Workflows;
using Elsa.Workflows.Models;

namespace Elsa.Demo.API.Workflows;

public class RandomVote : CodeActivity<bool>
{
    public Input<User> User { get; set; } = default!;

    protected override void Execute(ActivityExecutionContext context)
    {
        var logger = context.GetRequiredService<ILogger<RandomVote>>();
        var value = Random.Shared.Next(0, 2) > 0;

        logger.LogInformation(
            "User {UserName} has voted {VoteResult}",
            context.Get(User).Name,
            value
        );

        context.SetResult(value);
    }
}
