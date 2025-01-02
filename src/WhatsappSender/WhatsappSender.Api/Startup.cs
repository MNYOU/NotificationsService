﻿using Serilog;
using WhatsappSender.Api.Subscriptions;
using WhatsappSender.Api.Consumers;
using WhatsappSender.Api.Consumers.Interfaces;
using WhatsappSender.Api.Consumers.Options;
using WhatsappSender.SendLogic.Extensions;

namespace WhatsappSender.Api;

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
        services.AddSerilog((serviceProvider, lc) => lc
            .ReadFrom.Configuration(configuration)
            .ReadFrom.Services(serviceProvider)
            .Enrich.FromLogContext()
            .WriteTo.Console());
        services.AddSendLogic(configuration);
        services.AddScoped<IRabbitMqSubscriber, RabbitMqSubscriber>();
        services.Configure<RabbitMqOption>(configuration.GetSection(nameof(RabbitMqOption)));
        services.AddScoped<IConsumerService, RabbitMqConsumer>();
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
        app.UseSerilogRequestLogging();
        app.UseHttpsRedirection();
        app.UseCors("default");
        app.UseRouting();
        app.UseResponseCaching();

        app.UseEndpoints(routeBuilder => routeBuilder.MapControllers());
    }
}