using JetBrains.Annotations;

namespace Contracts.Queues;

[PublicAPI]
public static class QueueConstants
{
    public static string WhatsappQueueName => "whatsapp";
    public static string SmsQueueName => "sms";
    public static string EmailQueueName => "email";
}