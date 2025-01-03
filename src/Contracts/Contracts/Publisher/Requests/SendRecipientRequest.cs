using System.ComponentModel.DataAnnotations;

namespace Contracts.Publisher.Requests;

public class SendRecipientRequest
{
    [Required(ErrorMessage = "Номер телефона обязателен.")]
    [RegularExpression(@"^8d{10}$", ErrorMessage = "Неверный формат номера телефона. Должен начинаться с 8 и содержать 10 цифр после этого.")]
    public required string PhoneNumber { get; set; }
    
    [Required(ErrorMessage = "Электронная почта обязательна.")]
    [EmailAddress(ErrorMessage = "Неверный формат электронной почты.")]
    public required string Email { get; set; }
}