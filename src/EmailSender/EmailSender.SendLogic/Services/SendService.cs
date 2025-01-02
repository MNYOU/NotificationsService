﻿using CoreLib.Common;
using EmailSender.Core.Common;
using EmailSender.Domain.Settings;
using EmailSender.SendLogic.Interfaces.Services;
using EmailSender.SendLogic.Models.DTO.SendModels;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace EmailSender.SendLogic.Services;

public sealed class SendService(IOptions<EmailSettings> settingProvider) : ISendService
{
    private readonly EmailSettings settings = settingProvider.Value;

    public async Task<OperationResult<SendMessage>> Send(SendMessage emailMessage)
    {
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

        return result;
    }

    public async Task<BatchOperationResult<SendMessage>> SendBulk(IEnumerable<SendMessage> messages)
    {
        var results = new List<OperationResult<SendMessage>>();
        using var client = await GetSetUpClient();
        foreach (var message in messages)
        {
            var sendResult = await SendSingleFromBulkAsync(message, client);
            results.Add(sendResult);
        }

        return BatchOperationResult<SendMessage>.FromOperationResults(results);
    }

    private async Task<OperationResult<SendMessage>> SendSingleFromBulkAsync(SendMessage emailMessage,
        SmtpClient client)
    {
        using var message = GetMimeMessage(emailMessage);
        try
        {
            await client.SendAsync(message);
        }
        catch (Exception ex)
        {
            return Error.BadRequest($"Ошибка при отправке сообщения пользователю {emailMessage.Recipient} " +
                                    $"с заголовком {emailMessage.Title} и телом {emailMessage.Content} {ex.Message}");
        }

        return emailMessage;
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