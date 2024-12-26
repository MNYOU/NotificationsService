using System.ComponentModel.DataAnnotations;

namespace EmailSender.Domain.Settings;

public class EmailSettings
{
    public string Name { get; set; }

    public string Address { get; set; }

    public string Password { get; set; }

    public string Host { get; set; }

    public int Port { get; set; }
}