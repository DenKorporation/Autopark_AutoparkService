using Asp.Versioning.ApiExplorer;
using AutoparkService.Application.Extensions;
using AutoparkService.Infrastructure.Extensions;
using AutoparkService.Presentation.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationLayer(builder.Configuration);
builder.Services.AddInfrastructureLayer(builder.Configuration);
builder.Services.AddPresentationLayer(builder.Configuration);

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Docker"))
{
    app.UseSwagger();
    app.UseSwaggerUI(
        options =>
        {
            IReadOnlyList<ApiVersionDescription> descriptions = app.DescribeApiVersions();

            foreach (var description in descriptions)
            {
                string url = $"/swagger/{description.GroupName}/swagger.json";
                string name = description.GroupName.ToUpperInvariant();

                options.SwaggerEndpoint(url, name);
            }

            options.OAuthClientId("dev");
        });

    await app.InitializeDatabaseAsync();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program
{
}
