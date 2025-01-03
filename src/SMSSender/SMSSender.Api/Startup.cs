﻿using MessageBroker.Extensions;
using SMSSender.Api.Consumers;
using SMSSender.Api.Consumers.Interfaces;
using SMSSender.SendLogic.Extensions;

namespace SMSSender.Api;

public class Startup(IConfiguration configuration)
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddProblemDetails();
        services.AddRouting(options => { options.LowercaseUrls = true; });
        services.AddEndpointsApiExplorer();
        services.AddControllers();
        services.AddSwaggerGen();

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
        services.AddSendLogic();
        services.AddMessageBrokerServices(configuration);
        services.AddSimpleConsumer(configuration);
        services.AddSingleton<IConsumerService, RabbitMqConsumer>();
        services.AddHostedService<ConsumerServiceInitializer>();
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