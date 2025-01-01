using MessagePublisher.Logic;
using MessagePublisher.Logic.Extensions;
using MessagePublisher.Logic.Models.Options;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<RabbitMqOption>(builder.Configuration.GetSection(nameof(RabbitMqOption)));
builder.Services.AddPublisherServices();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();