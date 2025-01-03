namespace WhatsappSender.Api.Consumers.Interfaces;

public interface IConsumerService
{
    public Task StartAsync();
    
    public Task StopAsync();
}