﻿using EmailSender.Domain.Settings;
using EmailSender.SendLogic.Extensions;

namespace EmailSender.Api;

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
        services.Configure<EmailSettings>(configuration.GetSection(nameof(EmailSettings)));
        services.AddSendLogic();
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