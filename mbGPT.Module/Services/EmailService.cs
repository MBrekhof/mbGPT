using mbGPT.Module.BusinessObjects;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace mbGPT.Module.Services
{
    public class MailService // : IMailService
    {
        private  MailSettings _settings;
        private  SettingsService _settingsService;
        public MailService(SettingsService settingsService)
        { 
            _settingsService = settingsService;
        }
        //public static async Task<MailService> CreateAsync(SettingsService settingsService)
        //{
        //    var mailService = new MailService(settingsService);
        //    await mailService.InitializeAsync();
        //    return mailService;
        //}

        private async Task InitializeAsync()
        {
            if (_settings == null)
            {
                _settings = new MailSettings();
                var settings = await _settingsService.GetSettingsAsync();
                _settings.SenderEmail = settings.FromEmailName;
                _settings.SenderDisplayName = settings.FromDisplayName;
                _settings.Host = settings.SMTPHost;
                _settings.Port = settings.SMTPPort;
                _settings.UseStartTls = settings.UseStartTls;
                _settings.UseSSL = settings.UseSSL;
                //_settings.UserName = settings.EmailUserName;
                _settings.SenderPassword = settings.EmailPassword;
            }
        }

        public async  Task<MailSettings> GetMailSettings()
        {
            if (_settings == null)
            {
                await InitializeAsync();
            }
            return _settings;
        }
        public async Task<bool> SendAsync(MailData mailData, CancellationToken ct = default)
        {
            if (_settings == null)
            {
                await InitializeAsync();
            }
            try
            {
                // Initialize a new instance of the MimeKit.MimeMessage class
                var mail = new MimeMessage();

                #region Sender / Receiver
                // Sender
                mail.From.Add(new MailboxAddress(_settings.SenderDisplayName, _settings.SenderEmail));
                //mail.Sender = new MailboxAddress(mailData.DisplayName ?? _settings.DisplayName, mailData.From ?? _settings.From);

                // Receiver
                foreach (string mailAddress in mailData.To)
                    mail.To.Add(MailboxAddress.Parse(mailAddress));

                // Set Reply to if specified in mail data
                if (!string.IsNullOrEmpty(mailData.ReplyTo))
                    mail.ReplyTo.Add(new MailboxAddress(mailData.ReplyToName, mailData.ReplyTo));

                // BCC
                // Check if a BCC was supplied in the request
                if (mailData.Bcc != null)
                {
                    // Get only addresses where value is not null or with whitespace. x = value of address
                    foreach (string mailAddress in mailData.Bcc.Where(x => !string.IsNullOrWhiteSpace(x)))
                        mail.Bcc.Add(MailboxAddress.Parse(mailAddress.Trim()));
                }

                // CC
                // Check if a CC address was supplied in the request
                if (mailData.Cc != null)
                {
                    foreach (string mailAddress in mailData.Cc.Where(x => !string.IsNullOrWhiteSpace(x)))
                        mail.Cc.Add(MailboxAddress.Parse(mailAddress.Trim()));
                }
                #endregion

                #region Content

                // Add Content to Mime Message
                var body = new BodyBuilder();
                mail.Subject = mailData.Subject;
                body.HtmlBody = mailData.Body;
                mail.Body = body.ToMessageBody();

                #endregion

                #region Send Mail

                using var smtp = new SmtpClient();

                if (_settings.UseSSL)
                {
                    await smtp.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.SslOnConnect, ct);
                }
                else if (_settings.UseStartTls)
                {
                    await smtp.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.StartTls, ct);
                }
                await smtp.AuthenticateAsync(_settings.SenderEmail, _settings.SenderPassword, ct);
                await smtp.SendAsync(mail, ct);
                await smtp.DisconnectAsync(true, ct);

                #endregion

                return true;

            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
