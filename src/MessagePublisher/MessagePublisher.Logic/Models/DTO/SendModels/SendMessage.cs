namespace MessagePublisher.Logic.Models.DTO.SendModels;

public class SendMessage
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required Recipient Recipient { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public ICollection<SendAttachment> Attachments { get; set; } = [];
}