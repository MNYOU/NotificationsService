namespace Contracts.Publisher.Requests;

public class SendAttachmentRequest
{
    public required string FileName { get; init; } 
    public required string PublicUrl { get; init; } 
}