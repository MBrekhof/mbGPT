#nullable enable

using DevExpress.Persistent.Base;

namespace mbGPT.Module.BusinessObjects
{
    [DefaultClassOptions]
    [NavigationItem("Email")]
    public class MailData : BaseObjectInt
    {

        public virtual List<string>? To { get; set; }
        public virtual List<string>? Bcc { get; set; }

        public virtual List<string>? Cc { get; set; }

        /// <summary>
        /// Sender email
        /// </summary>
        public virtual string? From { get; set; }
        /// <summary>
        /// Sender pretty name
        /// </summary>
        public virtual string? DisplayName { get; set; }
        /// <summary>
        /// Reply address
        /// </summary>
        public virtual string? ReplyTo { get; set; }
        /// <summary>
        /// Reply pretty name
        /// </summary>
        public virtual string? ReplyToName { get; set; }

        // Content
        public virtual string? Subject { get; set; }

        public virtual string? Body { get; set; }
    }
}