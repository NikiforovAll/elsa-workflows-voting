using System.Net.Mime;
using Elsa.Extensions;
using Elsa.Http;
using Elsa.Workflows;
using Elsa.Workflows.Activities;
using Elsa.Workflows.Contracts;

namespace Elsa.Demo.API.Workflows;

public class QuorumVotingWorkflowCompositeActivity : WorkflowBase
{
    protected override void Build(IWorkflowBuilder builder)
    {
        builder.Version = 1;

        var board = builder.WithVariable<VotingBoard>().WithWorkflowStorage();
        var currentVoteResult = builder.WithVariable<bool>();

        var votes = builder.WithVariable<Dictionary<string, bool>>("votes", []);

        var approved = builder.WithVariable<bool>();

        builder.Root = new Sequence
        {
            Activities =
            {
                new HttpEndpoint
                {
                    Path = new("/parallel-quorum-vote-composite-activity"),
                    CanStartWorkflow = true,
                    SupportedMethods = new([HttpMethods.Post]),
                    ParsedContent = new(board)
                },
                new WriteHttpResponse
                {
                    ContentType = new(MediaTypeNames.Application.Json),
                    Content = new(board.Get),
                },
                new ParallelForEach<User>()
                {
                    Items = new(context => board.Get(context).Members),
                    Body = new Sequence
                    {
                        Activities =
                        {
                            new SimpleVoteActivity
                            {
                                User = new(context => context.GetVariable<User>("CurrentValue")),
                                Result = new(currentVoteResult)
                            },
                            new SetVariable<Dictionary<string, bool>>(
                                votes,
                                WorkflowExtensions.SetVotes(currentVoteResult, votes)
                            )
                        }
                    }
                },
                new QuorumVote()
                {
                    Votes = new(context => votes.Get(context).Values),
                    Result = new(approved),
                },
                new If(approved.Get<bool>)
                {
                    Then = new WriteLine(context => $"Approved!"),
                    Else = new WriteLine(context => $"Rejected!")
                }
            },
        };
    }
}
