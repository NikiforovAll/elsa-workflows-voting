using System.Text.Json;
using Elsa.Extensions;
using Elsa.Mediator.Contracts;
using Elsa.Workflows.Runtime.Contracts;
using Elsa.Workflows.Runtime.Notifications;

namespace Elsa.Samples.AspNet.RunTaskIntegration.Handlers;

public class RunTaskHandler(ITaskReporter taskReporter, ILogger<RunTaskHandler> logger)
    : INotificationHandler<RunTaskRequest>
{
    private readonly ITaskReporter _taskReporter = taskReporter;

    public async Task HandleAsync(RunTaskRequest notification, CancellationToken cancellationToken)
    {
        if (notification.TaskName != "PromptVote")
            return;

        var args = notification.TaskPayload!;

        logger.LogInformation(
            "TaskPayload - {Payload}, TaskId - {TaskId}",
            JsonSerializer.Serialize(args),
            notification.TaskId
        );

        // await _taskReporter.ReportCompletionAsync(notification.TaskId, null, cancellationToken);
    }
}
