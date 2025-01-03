using System.ComponentModel.DataAnnotations;

namespace Contracts.Email.Requests;

public class EmailRecipientRequest
{
    [Required(ErrorMessage = "Электронная почта обязательна.")]
    [EmailAddress(ErrorMessage = "Неверный формат электронной почты.")]
    public required string Email { get; set; }
}