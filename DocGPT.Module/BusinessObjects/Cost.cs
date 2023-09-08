#nullable enable
using DevExpress.Persistent.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocGPT.Module.BusinessObjects
{
    [DefaultClassOptions]
    public partial class Cost : BaseObjectNoID
    {
        [System.ComponentModel.DataAnnotations.Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int CostId { get; set; }

       // public virtual LlmAction? Action { get; set; }
        public virtual SourceType? SourceType { get; set; }
        public virtual Article? Article { get; set; }
        public virtual ArticleDetail? ArticleDetail { get; set; }
        public virtual LlmAction? LlmAction { get; set; }
        public virtual CodeObject? CodeObject { get; set; }
        public virtual Chat? Chat { get; set; }
        public virtual int? PromptTokens { get; set; }
        public virtual int? CompletionTokens { get; set; }
        public virtual int? TotalTokens { get; set; }
    }

    public enum LlmAction { embedding, completion}
    public enum SourceType { Article, ArticleDetail, CodeObject, Chat, Summarize }
}
