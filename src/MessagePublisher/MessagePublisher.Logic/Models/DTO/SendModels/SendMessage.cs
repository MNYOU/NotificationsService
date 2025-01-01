namespace MessagePublisher.Logic.Models.DTO.SendModels;

public class SendMessage
{
    public string Recipient { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public ICollection<SendAttachment> Attachments { get; set; } = [];
}