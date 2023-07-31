
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using System.ComponentModel.DataAnnotations;
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
    [VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
    public virtual string Question { get; set; }

    [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
    [FieldSize(FieldSizeAttribute.Unlimited)]
    public virtual string QuestionDataString { get; set; }

    [Column("PromptID")]
    public virtual Prompt Prompt { get; set; }

    [FieldSize(FieldSizeAttribute.Unlimited)]
    //[EditorAlias(DevExpress.ExpressApp.Editors.EditorAliases.HtmlPropertyEditor)]
    public virtual string Answer { get; set; }

    [VisibleInLookupListView(false)]
    public virtual int? Tokens { get; set; }
    [VisibleInLookupListView(false)]
    public virtual ChatModel? ChatModel { get; set; }

    [VisibleInLookupListView(false)]
    //[Column(TypeName ="DateTime")]

    public virtual DateTime? Created { get; set; }
}

public enum ChatModel { GPT3, GPT4 };

