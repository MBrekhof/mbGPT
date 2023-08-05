#nullable enable

using DevExpress.Persistent.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocGPT.Module.BusinessObjects
{
    [DefaultClassOptions]

    public partial class Settings : BaseObjectNoID
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int SettingsID { get; set; }
        public virtual string? OpenAIOrganization { get; set; }
        public virtual string? OpenAIKey { get; set; }
        public virtual ChatModel? ChatModel { get; set; }
        public virtual EmbeddingModel? EmbeddingModel { get; set; }

        public virtual string? FromDisplayName { get; set; }
        public virtual string? FromEmailName { get; set; }
        public virtual string? EmailUserName { get; set; }
        public virtual string? EmailPassword { get; set; }
        public virtual string? SMTPHost { get; set; }
        public virtual int SMTPPort { get; set; }
        public virtual bool UseSSL { get; set; }
        public virtual bool UseStartTls { get; set; }
    }
}
