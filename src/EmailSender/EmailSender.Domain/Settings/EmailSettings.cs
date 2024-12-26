using System.ComponentModel.DataAnnotations;

namespace EmailSender.Domain.Settings;

public class EmailSettings
{
    [Display(Name = "Имя")]
    public string Name { get; set; }

    [Display(Name = "Адрес")]
    public string Address { get; set; }

    [Display(Name = "Пароль")]
    public string Password { get; set; }

    [Display(Name = "Хост")]
    public string Host { get; set; }

    [Display(Name = "Порт")]
    public int Port { get; set; }
}