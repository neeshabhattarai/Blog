using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;

namespace Blog.Services;

public interface IEmailService
{
    Task SendEmailAsync(string email, string subject, string message);
}

public class EmailService:IEmailService
{
    private readonly IOptions<SMTPConfigure> _smtpConfigure;

    public EmailService(IOptions<SMTPConfigure> smtpConfigure)
    {
        _smtpConfigure = smtpConfigure;
    }
    public async Task SendEmailAsync(string email, string subject, string message)
    {
        var mailMessage = new MailMessage(_smtpConfigure.Value.UserName,email, subject, message);
        using (var smpt = new SmtpClient())
        {
            smpt.Credentials=new NetworkCredential(_smtpConfigure.Value.UserName,_smtpConfigure.Value.Password);
            smpt.Host=_smtpConfigure.Value.Host;
            smpt.Port = _smtpConfigure.Value.Port;
            smpt.EnableSsl = true;
            await  smpt.SendMailAsync(mailMessage);
            
        }
    }
}