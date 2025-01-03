using CoreLib.Common;
using MessagePublisher.Logic.Models.DTO.SendModels;
using MessagePublisher.Logic.Models.Requests.Send;

namespace MessagePublisher.Logic.Interfaces.Services;

public interface IPublisherService
{
    public Task StartAsync();

    public Task<OperationResult<SendMessage>> Publish(SendMessageRequest message);
    
    public Task StopAsync();
}