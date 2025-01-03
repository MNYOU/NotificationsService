namespace SMSSender.SendLogic.Models.DTO.SendModels;

public class SendMessage
{
    public string Sender { get; set; } = string.Empty;
    public string Recipient { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public ICollection<SendAttachment> Attachments { get; set; } = [];
}