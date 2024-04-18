using Elsa.EntityFrameworkCore.Extensions;
using Elsa.EntityFrameworkCore.Modules.Management;
using Elsa.EntityFrameworkCore.Modules.Runtime;
using Elsa.Extensions;

namespace Elsa.Demo.API;

public static class ServiceCollectionExtensions
{
    public static void RegisterWorkflowServices(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("WorkflowDb")!;

        builder.Services.AddElsa(elsa =>
        {
            // Configure Management layer to use EF Core.
            elsa.UseWorkflowManagement(management =>
                management.UseEntityFrameworkCore(ef => ef.UseSqlServer(connectionString))
            );

            // Configure Runtime layer to use EF Core.
            elsa.UseWorkflowRuntime(runtime =>
                runtime.UseEntityFrameworkCore(ef => ef.UseSqlServer(connectionString))
            );

            // Default Identity features for authentication/authorization.
            elsa.UseIdentity(identity =>
            {
                identity.TokenOptions = options =>
                    options.SigningKey = "00000000-0000-0000-0000-000000000000"; // This key needs to be at least 256 bits long.
                identity.UseAdminUserProvider();
            });

            elsa.UseSasTokens();

            // Configure ASP.NET authentication/authorization.
            elsa.UseDefaultAuthentication(auth =>
            {
                auth.UseAdminApiKey();
            });

            // Expose Elsa API endpoints.
            elsa.UseWorkflowsApi();

            // Setup a SignalR hub for real-time updates from the server.
            elsa.UseRealTimeWorkflows();

            // Enable C# workflow expressions
            elsa.UseCSharp();

            // Enable HTTP activities.
            elsa.UseHttp(http =>
                http.ConfigureHttpOptions = options =>
                {
                    options.BasePath = "/workflows";
                    options.BaseUrl = new Uri("http://localhost:5239");
                }
            );

            // Use timer activities.
            elsa.UseScheduling();

            // Register custom activities from the application, if any.
            elsa.AddActivitiesFrom<Program>();

            // Register custom workflows from the application, if any.
            elsa.AddWorkflowsFrom<Program>();

            elsa.Services.AddHandlersFrom<Program>();
        });
    }

    public static void RegisterWorkflowEndpoints(this WebApplication app)
    {
        app.UseWorkflowsApi(); // Use Elsa API endpoints.
        app.UseWorkflows(); // Use Elsa middleware to handle HTTP requests mapped to HTTP Endpoint activities.
        app.UseWorkflowsSignalRHubs(); // Optional SignalR integration. Elsa Studio uses SignalR to receive real-time updates from the server.
    }
}
