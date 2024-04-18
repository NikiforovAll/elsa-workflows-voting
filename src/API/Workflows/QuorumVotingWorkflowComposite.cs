using System.Net.Mime;
using Elsa.Extensions;
using Elsa.Http;
using Elsa.Workflows;
using Elsa.Workflows.Activities;
using Elsa.Workflows.Contracts;
using Elsa.Workflows.Runtime.Activities;

namespace Elsa.Demo.API.Workflows;

public class QuorumVotingWorkflowComposite : WorkflowBase
{
    protected override void Build(IWorkflowBuilder builder)
    {
        builder.Version = 1;

        var board = builder.WithVariable<VotingBoard>().WithWorkflowStorage();
        var childOutput = builder.WithVariable<IDictionary<string, object>>();
        var currentVoteResult = builder.WithVariable<bool>();

        var votes = builder.WithVariable<Dictionary<string, bool>>("votes", []);

        var approved = builder.WithVariable<bool>();

        builder.Root = new Sequence
        {
            Activities =
            {
                new HttpEndpoint
                {
                    Path = new("/parallel-quorum-vote-composite"),
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
                    // doesn't work - NRE
                    Items = new(context => board.Get(context).Members),
                    Body = new Sequence
                    {
                        Activities =
                        {
                            new DispatchWorkflow
                            {
                                WorkflowDefinitionId = new(nameof(SimpleVoteWorkflow)),
                                Input = new(context => new Dictionary<string, object>
                                {
                                    ["User"] = context.GetVariable<User>("CurrentValue")
                                }),
                                WaitForCompletion = new(true),
                                Result = new(childOutput)
                            },
                            new SetVariable<bool>(
                                currentVoteResult,
                                context => (bool)childOutput.Get(context)["Vote"]
                            ),
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
