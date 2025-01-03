namespace Contracts.Email.Requests;

public class EmailAttachmentRequest
{
    public required string FileName { get; init; }
    public required string PublicUrl { get; init; }
}