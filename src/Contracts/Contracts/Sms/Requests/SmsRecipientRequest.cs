using System.ComponentModel.DataAnnotations;

namespace Contracts.Sms.Requests;

public class SmsRecipientRequest
{
    [Required(ErrorMessage = "Номер телефона обязателен.")]
    [RegularExpression(@"^(\+7|7)\d{10}$", 
        ErrorMessage = "Неверный формат номера телефона. Должен начинаться с +7 или 7 и содержать 10 цифр после этого.")]
    public required string PhoneNumber { get; set; }
}