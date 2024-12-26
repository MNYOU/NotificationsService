namespace WhatsappSender.SendLogic.Models.DTO.SendModels;

public class SendAttachment
{
    public string FileName { get; set; } = string.Empty;
    public string FileUrl { get; set; } = string.Empty;

    public override string ToString()
    {
        return $"Имя файла: {FileName}, путь к файлу: {FileUrl}";
    }
}