using MailKit.Net.Smtp;
using MimeKit;

namespace RazorEmails.Mailers;

public class MailData
{
    public required string To { get; set; }
    public required string ToName { get; set; }
    public required string Subject { get; set; }
    public required string Body { get; set; }
}

public class Mailer
{
    private readonly ILogger<Mailer> _logger;
    private readonly IConfiguration _config;

    public Mailer(IConfiguration config, ILogger<Mailer> logger)
    {
        _logger = logger;
        _config = config;
    }

    // Not much to see here, just a method for sending emails using MailKit.
    // Docs are here: https://github.com/jstedfast/MailKit
    public async Task<bool> SendMailAsync(MailData mailData)
    {
        try
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress(MailSenderName, MailSenderEmail));
            emailMessage.To.Add(new MailboxAddress(mailData.ToName, mailData.To));

            emailMessage.Subject = mailData.Subject;

            var emailBodyBuilder = new BodyBuilder
            {
                TextBody = mailData.Body,
                HtmlBody = mailData.Body
            };

            emailMessage.Body = emailBodyBuilder.ToMessageBody();

            using var smtpClient = new SmtpClient();

            await smtpClient.ConnectAsync(
                MailServer,
                MailPort,
                MailKit.Security.SecureSocketOptions.StartTls
            );

            await smtpClient.AuthenticateAsync(MailUserName, MailPassword);
            await smtpClient.SendAsync(emailMessage);
            await smtpClient.DisconnectAsync(true);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email to {To}", mailData.To);
            return false;
        }
    }

    private string MailSenderName => _config["MailSettings:SenderName"]!;
    private string MailSenderEmail => _config["MailSettings:SenderEmail"]!;
    private string MailServer => _config["MailSettings:Server"]!;
    private int MailPort => int.Parse(_config["MailSettings:Port"]!);
    private string MailUserName => _config["MailSettings:UserName"]!;
    private string MailPassword => _config["MailSettings:Password"]!;
}
