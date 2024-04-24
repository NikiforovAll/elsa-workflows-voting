using System.Net.Mime;
using Elsa.Extensions;
using Elsa.Http;
using Elsa.Workflows;
using Elsa.Workflows.Activities;
using Elsa.Workflows.Contracts;
using Elsa.Workflows.Models;
using Elsa.Workflows.Runtime.Activities;

namespace Elsa.Demo.API.Workflows;

public class QuorumVotingWorkflow : WorkflowBase
{
    protected override void Build(IWorkflowBuilder builder)
    {
        builder.Version = 1;

        var board = builder.WithVariable<VotingBoard>().WithWorkflowStorage();

        var currentVoteResult = builder.WithVariable<bool>().WithWorkflowStorage();

        var votes = builder.WithVariable<Dictionary<string, bool>>().WithWorkflowStorage();

        var approved = builder.WithVariable<bool>().WithWorkflowStorage();

        builder.Root = new Sequence
        {
            Activities =
            {
                new HttpEndpoint
                {
                    Path = new("/parallel-quorum-vote"),
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
                            new RunTask("PromptVote")
                            {
                                Result = new Output<object>(currentVoteResult),
                                Payload = new(context => new Dictionary<string, object>
                                {
                                    ["User"] = context.GetVariable<User>("CurrentValue"),
                                })
                            },
                            new SetVariable<Dictionary<string, bool>>(
                                votes,
                                WorkflowExtensions.SetVotes(currentVoteResult, votes)
                            )
                        }
                    },
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
