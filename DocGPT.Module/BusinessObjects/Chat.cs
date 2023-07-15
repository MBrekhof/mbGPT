
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using System.ComponentModel.DataAnnotations.Schema;


namespace DocGPT.Module.BusinessObjects;
[DefaultClassOptions]
[NavigationItem("DocGPT")]
[Table("Chat")]

public partial class Chat : BaseObjectNoID
{
    [System.ComponentModel.DataAnnotations.Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("ChatId")]
    [VisibleInDetailView(false)]
    public virtual int ChatId { get; set; }

    [FieldSize(FieldSizeAttribute.Unlimited)]
    public virtual string Question { get; set; }

    [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
    [FieldSize(FieldSizeAttribute.Unlimited)]
    public virtual string QuestionDataString { get; set; }

    [Column("PromptID")]
    public virtual Prompt Prompt { get; set; }

    [FieldSize(FieldSizeAttribute.Unlimited)]
    public virtual string Answer { get; set; }
}

