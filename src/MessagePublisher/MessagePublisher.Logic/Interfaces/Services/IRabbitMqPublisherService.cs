using CoreLib.Common;
using MessagePublisher.Logic.Models.DTO.SendModels;

namespace MessagePublisher.Logic.Interfaces.Services;

public interface IPublisherService<in TBody>
    where TBody : class
{
    public Task<OperationResult> Publish(TBody body);
}