using System.Text;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MessageBroker.Helpers;

public class Serializer
{
    private readonly ILogger<Serializer> logger;

    public Serializer(ILogger<Serializer> logger)
    {
        this.logger = logger;
    }

    public byte[] GetBytes<TBody>(TBody body) where TBody : class
    {
        var stringBody = JsonConvert.SerializeObject(body);
        var bytes = Encoding.UTF8.GetBytes(stringBody);

        return bytes;
    }

    public TBody? Deserialize<TBody>(byte[] body) where TBody : class
    {
        var stringBody = Encoding.UTF8.GetString(body);
        try
        {
            return JsonConvert.DeserializeObject<TBody>(stringBody);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to deserialize body: {body}", stringBody);
            return null;
        }
    }
}