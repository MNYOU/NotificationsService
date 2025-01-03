using CoreLib.Common;
using MessagePublisher.Logic.Models.DTO.SendModels;

namespace MessagePublisher.Logic.Interfaces.Managers;

public interface IPublisherManager
{
    public Task<OperationResult> Send(SendMessage sendMessage);
}