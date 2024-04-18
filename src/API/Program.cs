using Elsa.Demo.API;
using Elsa.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks();
builder.RegisterWorkflowServices();
builder.Services.AddCors(cors =>
    cors.AddDefaultPolicy(policy =>
        policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().WithExposedHeaders("*")
    )
);

builder.Services.AddProblemDetails(options =>
    options.CustomizeProblemDetails = ctx =>
    {
        ctx.ProblemDetails.Extensions.Add("trace-id", ctx.HttpContext.TraceIdentifier);
        ctx.ProblemDetails.Instance =
            $"{ctx.HttpContext.Request.Method} {ctx.HttpContext.Request.Path}";
    }
);
var app = builder.Build();

app.UseRouting();

app.UseStatusCodePages();
app.UseExceptionHandler();

app.UseCors();
app.UseHttpsRedirection();
app.MapHealthChecks("/");
app.UseAuthentication();
app.UseAuthorization();

app.RegisterWorkflowEndpoints();

app.Run();
