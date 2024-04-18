using System.Net;
using System.Net.Mime;
using System.Text.Json;
using Elsa.Extensions;
using Elsa.Http;
using Elsa.Scheduling.Activities;
using Elsa.Workflows;
using Elsa.Workflows.Activities;
using Elsa.Workflows.Contracts;
using Elsa.Workflows.Models;

namespace Elsa.Demo.API.Workflows;

public class SequentialQuorumVotingWorkflow : WorkflowBase
{
    protected override void Build(IWorkflowBuilder builder)
    {
        builder.Version = 1;

        var board = builder.WithVariable<VotingBoard>("board", new([])).WithWorkflowStorage();
        var currentVote = builder.WithVariable<bool>();
        var currentUser = builder.WithVariable<User>();
        var votes = builder.WithVariable<Dictionary<string, bool>>();

        var approved = builder.WithVariable<bool>();

        builder.Root = new Sequence
        {
            Activities =
            {
                new HttpEndpoint
                {
                    Path = new("/quorum-vote"),
                    CanStartWorkflow = true,
                    SupportedMethods = new([HttpMethods.Post]),
                    ParsedContent = new Output<object?>(board)
                },
                new WriteHttpResponse
                {
                    Content = new("<h1>Request for Approval Sent</h1>"),
                    ContentType = new(MediaTypeNames.Text.Html),
                    StatusCode = new(HttpStatusCode.OK),
                },
                new ForEach<User>(context => board.Get(context).Members)
                {
                    CurrentValue = new Output<User>(currentUser),
                    Body = new Sequence
                    {
                        Activities =
                        {
                            new RandomVote() { User = new(currentUser), Result = new(currentVote) },
                            new SetVariable<Dictionary<string, bool>>(
                                votes,
                                context =>
                                {
                                    var user = currentUser.Get(context);
                                    var localVotes = votes.Get(context) ?? [];

                                    localVotes[user.Id] = currentVote.Get(context);

                                    return localVotes;
                                }
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
