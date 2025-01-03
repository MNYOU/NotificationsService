using System.ComponentModel.DataAnnotations;

namespace Contracts.WhatsApp.Requests;

public class WhatsAppRecipientRequest
{
    [Required(ErrorMessage = "Номер телефона обязателен.")]
    [RegularExpression(@"^(\+7|7)\d{10}$", 
        ErrorMessage = "Неверный формат номера телефона. Должен начинаться с +7 или 7, а затем должен идти номер телефона с 8 и содержать 10 цифр после этого.")]
    public required string PhoneNumber { get; set; }
}