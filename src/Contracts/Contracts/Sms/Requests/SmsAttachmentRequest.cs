namespace Contracts.Sms.Requests;

public class SmsAttachmentRequest
{
    public required string FileName { get; init; }
    public required string PublicUrl { get; init; }
}