namespace EmailSender.SendLogic.Models.DTO.SendModels;

public class SendMessage
{
    public string Recipient { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public IEnumerable<SendAttachment> Attachments { get; set; } = [];
}