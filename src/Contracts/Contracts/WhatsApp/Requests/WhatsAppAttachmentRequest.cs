namespace Contracts.WhatsApp.Requests;

public class WhatsAppAttachmentRequest
{
    public required string FileName { get; init; }
    public required string PublicUrl { get; init; }
}