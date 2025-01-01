using MessagePublisher.Logic.Extensions;
using MessagePublisher.Logic.Models.Options;

namespace MessagePublisher.Api;

public class Startup(IConfiguration configuration)
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddProblemDetails();
        services.AddRouting(options => { options.LowercaseUrls = true; });
        services.AddSwaggerGen();
        services.AddEndpointsApiExplorer();
        services.AddControllers();

        services.AddCors(options =>
        {
            options.AddPolicy("default", policy =>
            {
                policy
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin();
            });
        });
        services.AddHttpLogging(_ => { });
        services.AddLogging(config => config.AddConsole());
        services.Configure<RabbitMqOption>(configuration.GetSection(nameof(RabbitMqOption)));
        services.AddPublisherServices();
        services.AddSingleton<IHostedService, PublisherServiceInitializer>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseHsts();
        app.UseHttpLogging();
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseCors("default");
        app.UseRouting();
        app.UseResponseCaching();

        app.UseEndpoints(routeBuilder => routeBuilder.MapControllers());
    }
}