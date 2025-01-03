using System.ComponentModel.DataAnnotations;

namespace MessagePublisher.Logic.Models.DTO.SendModels;

public class Recipient
{
    [Required(ErrorMessage = "Электронная почта обязательна.")]
    [EmailAddress(ErrorMessage = "Неверный формат электронной почты.")]
    public required string Email { get; set; }
    
    [Required(ErrorMessage = "Номер телефона обязателен.")]
    [RegularExpression(@"^(\+7|8)\d{10}$", 
        ErrorMessage = "Неверный формат номера телефона. Должен начинаться с +7 или 7 и содержать 10 цифр после этого.")]
    public required string PhoneNumber { get; set; }
    
    [Required(ErrorMessage = "Номер телефона обязателен.")]
    [RegularExpression(@"^(\+7|7)(8|9)\d{10}$", 
        ErrorMessage = "Неверный формат номера телефона. Должен начинаться с +7 или 7, а затем должен идти номер телефона с 8 и содержать 10 цифр после этого.")]
    public required string WhatsappNumber { get; set; }
}