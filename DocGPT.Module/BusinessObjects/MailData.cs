#nullable enable

using DevExpress.Persistent.Base;

namespace DocGPT.Module.BusinessObjects
{
    [DefaultClassOptions]
    [NavigationItem("Email")]
    public class MailData : BaseObjectInt
    {

        public virtual List<string>? To { get; set; }
        public virtual List<string>? Bcc { get; set; }

        public virtual List<string>? Cc { get; set; }

        // Sender
        public virtual string? From { get; }

        public virtual string? DisplayName { get; }

        public virtual string? ReplyTo { get; set; }

        public virtual string? ReplyToName { get;   }

        // Content
        public virtual string? Subject { get; set; }

        public virtual string? Body { get; set; }
    }
}