using DocGPT.Module.BusinessObjects;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;


namespace DocGPT.Module.Services
{
    public class MailService // : IMailService
    {
        private  Settings _settings;
        private MailSettings mailSettings;

        public MailService()
        {
            
        }

        public  MailSettings Initialize(Settings settings)
        {
            if (_settings == null)
            {
                _settings = settings;
            }
            mailSettings = new MailSettings();
            mailSettings.SenderEmail = _settings.FromEmailName;
            mailSettings.SenderDisplayName = _settings.FromDisplayName;
            mailSettings.Host = _settings.SMTPHost;
            mailSettings.Port = _settings.SMTPPort;
            mailSettings.UseStartTls = _settings.UseStartTls;
            mailSettings.UseSSL = _settings.UseSSL;
            mailSettings.SenderPassword = _settings.EmailPassword;

            return mailSettings;
        }

        public MailSettings GetMailSettings()
        {
            return mailSettings;
        }
        public async Task<bool> SendBackGroundAsync(MailData mailData, Settings setting, CancellationToken ct = default)
        {
            Initialize(setting);
            var result = await SendAsync(mailData,ct);
            return result;
        }
        public async Task<bool> SendAsync(MailData mailData, CancellationToken ct = default)
        {
            try
            {

                // Initialize a new instance of the MimeKit.MimeMessage class
                var mail = new MimeMessage();

                #region Sender / Receiver
                // Sender
                mail.From.Add(new MailboxAddress(mailSettings.SenderDisplayName, mailSettings.SenderEmail));
                mail.Sender = new MailboxAddress(mailData.DisplayName ?? mailSettings.SenderDisplayName, mailData.From ?? mailSettings.SenderEmail);

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

                if (mailSettings.UseSSL)
                {
                    await smtp.ConnectAsync(mailSettings.Host, mailSettings.Port, SecureSocketOptions.SslOnConnect, ct);
                }
                else if (mailSettings.UseStartTls)
                {
                    await smtp.ConnectAsync(mailSettings.Host, mailSettings.Port, SecureSocketOptions.StartTls, ct);
                }
                await smtp.AuthenticateAsync(mailSettings.SenderEmail, mailSettings.SenderPassword, ct);
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
