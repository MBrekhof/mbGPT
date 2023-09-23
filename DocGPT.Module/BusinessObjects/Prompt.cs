
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using Microsoft.EntityFrameworkCore;

namespace mbGPT.Module.BusinessObjects;

[DefaultClassOptions]
[NavigationItem("DocGPT")]
[DefaultProperty("Subject")]

public partial class Prompt : BaseObjectNoID
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [VisibleInListView(false),VisibleInDetailView(false),VisibleInLookupListView(false)]
    public virtual int PromptID { get; set; }

    [StringLength(50)]
    public virtual string Subject { get; set; }


    [Unicode(false)]
    [FieldSize(FieldSizeAttribute.Unlimited)]
    public virtual string SystemPrompt { get; set; }

    [Unicode(false)]
    [FieldSize(FieldSizeAttribute.Unlimited)]
    public virtual string AssistantPrompt { get; set; }
}