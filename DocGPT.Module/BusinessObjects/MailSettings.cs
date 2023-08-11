#nullable enable

namespace DocGPT.Module.BusinessObjects
{
    public class MailSettings
    {
        public string? SenderDisplayName { get; set; }
        public string? SenderEmail { get; set; }
        public string? ReceiverEmail { get; set; }
        public string? SenderPassword { get; set; }
        public string? Host { get; set; }
        public int Port { get; set; }
        public bool UseSSL { get; set; }
        public bool UseStartTls { get; set; }
    }
}
