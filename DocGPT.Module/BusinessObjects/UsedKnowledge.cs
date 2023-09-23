#nullable enable
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using System.ComponentModel.DataAnnotations.Schema;



namespace mbGPT.Module.BusinessObjects
{
    [DefaultClassOptions]
    public partial class UsedKnowledge : BaseObjectNoID
    {
        [System.ComponentModel.DataAnnotations.Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        [VisibleInDetailView(false)]
        public virtual int UsedKnowledgeId { get; set; }

        public virtual Chat? Chat { get; set; }
        public virtual ArticleDetail? Article { get; set; }
        public virtual CodeObject? Code { get; set; }
        public virtual double cosinedistance { get; set; } = 0;
    }
}
