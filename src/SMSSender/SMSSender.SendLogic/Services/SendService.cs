using CoreLib.Common;
using SMSSender.Domain.Settings;
using SMSSender.SendLogic.Interfaces.Services;
using SMSSender.SendLogic.Models.DTO.SendModels;
using Microsoft.Extensions.Options;
using SMSC.Types;
using SMSC.Http;
using SMSC.Types.Enums;

namespace SMSSender.SendLogic.Services;

public sealed class SendService(IOptions<SMSSettings> settingProvider) : ISendService
{
    private readonly SMSSettings settings = settingProvider.Value;

    public async Task<OperationResult<SendMessage>> Send(SendMessage smsMessage)
    {
        var providerConfig = new ProviderConfiguration(settings.ApiKey);
        var httpSmsSender = new HttpSms(providerConfig);
        var smsConfig = new SmsConfiguration()
        {
            SmsType = SmsType.Default,
            Sender = settings.Sender,
        };
        OperationResult<SendMessage> result;
        try
        {
            var httpSmsResponse = await httpSmsSender.SendSms(smsMessage.Recipients, smsMessage.Message, smsConfig);
            result = smsMessage;
        }
        catch (Exception ex)
        {
            result = Error.BadRequest($"Ошибка при отправке сообщения на номер {smsMessage.Recipients} " +
                $"с телом {smsMessage.Message}. {ex.Message}");
        }

        return result;
    }

    public async Task<BatchOperationResult<SendMessage>> SendBulk(IEnumerable<SendMessage> messages)
    {
        var sendResults = new List<OperationResult<SendMessage>>();
        foreach (var message in messages)
        {
            var result = await Send(message);
            sendResults.Add(result);
        }

        return BatchOperationResult<SendMessage>.FromOperationResults(sendResults);
    }

    public async Task<string> GetBalance(ProviderConfiguration providerConfig)
    {
        var httpSmsBalance = new HttpSmsBalance(providerConfig);
        var smsBalanceConfig = new SmsBalanceConfiguration();
        string result = string.Empty;
        try
        {
            var balanceResult = await httpSmsBalance.CheckBalance(smsBalanceConfig);
            result = balanceResult.Balance.ToString();
        }
        catch (Exception ex)
        {
            result = $"Ошибка при получении баланса. {ex.Message}";
        }
        return result;
    }
}