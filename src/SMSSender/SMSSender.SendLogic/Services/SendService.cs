using CoreLib.Common;
using Microsoft.Extensions.Logging;
using SMSSender.Domain.Settings;
using SMSSender.SendLogic.Interfaces.Services;
using SMSSender.SendLogic.Models.DTO.SendModels;
using Microsoft.Extensions.Options;
using SMSC.Types;
using SMSC.Http;
using SMSC.Types.Enums;
using SMSSender.SendLogic.Managers;
using System.Text;
using System.Net.Mail;

namespace SMSSender.SendLogic.Services;

public sealed class SendService(IOptions<SMSSettings> settingProvider, ILogger<SendManager> logger) : ISendService
{
    private readonly SMSSettings settings = settingProvider.Value;

    public async Task<OperationResult<SendMessage>> Send(SendMessage smsMessage)
    {
        logger.LogInformation("Sending message to: {Recipient}", smsMessage.Recipient);
        var providerConfig = new ProviderConfiguration(settings.ApiKey);
        var httpSmsSender = new HttpSms(providerConfig);
        var smsConfig = new SmsConfiguration()
{
            Sender = settings.Sender,
        };
        var message = GetMessageBody(smsMessage.Content, smsMessage.Attachments);

        OperationResult<SendMessage> result;

        try
        {
            var httpSmsResponse = await httpSmsSender.SendSms(smsMessage.Recipient, message, smsConfig);
            result = smsMessage;
            logger.LogInformation("Successfully sent message to: {Recipient}", smsMessage.Recipient);
        }
        catch (Exception ex)
        {
            result = Error.BadRequest($"Ошибка при отправке сообщения на номер {smsMessage.Recipient} " +
                $"с телом {message}. {ex.Message}");
            logger.LogError(ex, "Error sending SMS message to recipients: {Recipient} with message {Content}", 
                smsMessage.Recipient, result.Error!.Message);
        }

        return result;
    }

    public async Task<BatchOperationResult<SendMessage>> SendBulk(IReadOnlyCollection<SendMessage> messages)
    {
        logger.LogInformation("Start sending bulk SMS message. Total messages: {MessageCount}", messages.Count);
        var sendResults = new List<OperationResult<SendMessage>>();
        foreach (var message in messages)
        {
            var result = await Send(message);
            sendResults.Add(result);
        }

        var batchResult = BatchOperationResult<SendMessage>.FromOperationResults(sendResults);
        if(batchResult.IsSuccess)
        {
            logger.LogInformation("Successfully sent bulk SMS message. Total messages: {MessageCount}", messages.Count);
        }
        else
        {
            logger.LogWarning("Failed to send bulk SMS message. Error: {ErrorMessage}", 
                string.Join(", ", batchResult.Errors.Select(x => x.Message)));
        }

        return batchResult;
    }

    public async Task<string> GetBalance(ProviderConfiguration providerConfig)
    {
        logger.LogInformation("Start checking SMS balance.");
        var httpSmsBalance = new HttpSmsBalance(providerConfig);
        var smsBalanceConfig = new SmsBalanceConfiguration();
        string result;
        try
        {
            var balanceResult = await httpSmsBalance.CheckBalance(smsBalanceConfig);
            result = balanceResult.Balance.ToString() ?? "Ошибка при получении баланса. Баланс был null";
            logger.LogInformation("Successfully received SMS balance: {Balance}", result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while receiving balance.");
            result = $"Ошибка при получении баланса. {ex.Message}";
        }
        return result;
    }

    private string GetMessageBody(string content, IEnumerable<SendAttachment> attachments)
    {
        var builder = new StringBuilder(content);

        foreach (var attachment in attachments)
        {
            builder.Append("\r\n");
            builder.Append(attachment);
        }

        return builder.ToString();
    }
}