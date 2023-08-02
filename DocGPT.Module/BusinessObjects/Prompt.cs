
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using Microsoft.EntityFrameworkCore;

namespace DocGPT.Module.BusinessObjects;

[DefaultClassOptions]
[NavigationItem("DocGPT")]
//[Table("Prompt")]
[DefaultProperty("Subject")]

public partial class Prompt
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    //[Column("PromptID")]
    [VisibleInListView(false),VisibleInDetailView(false),VisibleInLookupListView(false)]
    public virtual int PromptID { get; set; }

    [StringLength(50)]
    //[Column("Subject")]
    public virtual string Subject { get; set; }


    [Unicode(false)]
    //[Column("PromptBody")]
    [FieldSize(FieldSizeAttribute.Unlimited)]
    public virtual string PromptBody { get; set; }
}