using WhatsappSender.Core.Common;
using WhatsappSender.SendLogic.Models.DTO.SendModels;
using WhatsappSender.SendLogic.Models.Requests.Send;

namespace WhatsappSender.Api.Consumers.Interfaces;

public interface IConsumerService
{
    public Task StartAsync();
    
    public Task StopAsync();
}