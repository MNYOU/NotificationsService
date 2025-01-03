using CoreLib.Common;
using CoreLib.Logging.Extensions;
using EmailSender.Domain.Settings;
using EmailSender.SendLogic.Interfaces.Services;
using EmailSender.SendLogic.Models.DTO.SendModels;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace EmailSender.SendLogic.Services;

public sealed class SendService(IOptions<EmailSettings> settingProvider, ILogger<SendService> logger) : ISendService
{
    private readonly EmailSettings settings = settingProvider.Value;

    public async Task<OperationResult<SendMessage>> Send(SendMessage emailMessage)
    {
        logger.LogInformation("Sending message to: {Recipient}", emailMessage.Recipient);
        using var client = await GetSetUpClient();
        using var message = GetMimeMessage(emailMessage);
        OperationResult<SendMessage> result;
        try
        {
            await client.SendAsync(message);
            result = emailMessage;
        }
        catch (Exception ex)
        {
            result = Error.BadRequest($"Ошибка при отправке сообщения пользователю {emailMessage.Recipient} " +
                                      $"с заголовком {emailMessage.Title} и телом {emailMessage.Content} {ex.Message}");
        }
        logger.LogResult(result);

        return result;
    }

    public async Task<BatchOperationResult<SendMessage>> SendBulk(IReadOnlyCollection<SendMessage> messages)
    {
        logger.LogInformation("Sending bulk messages. Total messages: {Count}", messages.Count);
        var results = new List<OperationResult<SendMessage>>();
        using var client = await GetSetUpClient();
        foreach (var message in messages)
        {
            var sendResult = await SendSingleFromBulkAsync(message, client);
            results.Add(sendResult);
        }
        var batchResult = BatchOperationResult<SendMessage>.FromOperationResults(results);
        logger.LogBatchResult(batchResult);
        return batchResult;
    }

    private async Task<OperationResult<SendMessage>> SendSingleFromBulkAsync(SendMessage emailMessage,
        SmtpClient client)
    {
        using var message = GetMimeMessage(emailMessage);
        OperationResult<SendMessage> result = emailMessage;
        try
        {
            await client.SendAsync(message);
        }
        catch (Exception ex)
        {
            result = Error.BadRequest($"Ошибка при отправке сообщения пользователю {emailMessage.Recipient} " +
                                      $"с заголовком {emailMessage.Title} и телом {emailMessage.Content} {ex.Message}");
        }
        
        logger.LogResult(result);
        return result;
    }

    private MimeMessage GetMimeMessage(SendMessage emailMessage)
    {
        var message = new MimeMessage();
        message.From.Add(GetSenderAddress());
        message.Subject = emailMessage.Title;
        message.Body = GetMessageBody(emailMessage.Content, emailMessage.Attachments);
        message.To.Clear();
        message.To.Add(new MailboxAddress(string.Empty, emailMessage.Recipient));
        return message;
    }

    private MailboxAddress GetSenderAddress()
    {
        var from = new MailboxAddress(settings.Name, settings.Address);
        return from;
    }

    private MimeEntity GetMessageBody(string content, IEnumerable<SendAttachment> attachments)
    {
        var builder = new BodyBuilder
        {
            HtmlBody = content
        };

        foreach (var attachment in attachments)
        {
            builder.Attachments.Add(attachment.FileUrl);
        }

        var body = builder.ToMessageBody();
        return body;
    }

    private async Task<SmtpClient> GetSetUpClient()
    {
        var client = new SmtpClient();
        await client.ConnectAsync(settings.Host, settings.Port);
        await client.AuthenticateAsync(settings.Address, settings.Password);
        return client;
    }
}