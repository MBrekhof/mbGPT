using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.EF;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace mbGPT.Module.BusinessObjects;
[DefaultClassOptions]
[NavigationItem("Knowledge")]
[DefaultProperty("ArticleName")]
[FileAttachment(nameof(File))]
[Table("article")]
public partial class Article : BaseObjectNoID
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public virtual int ArticleId { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    //[Column("articleName")]
    public virtual string ArticleName { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    public virtual string Description { get; set; }

    [FieldSize(FieldSizeAttribute.Unlimited)]
    [EditorAlias(DevExpress.ExpressApp.Editors.EditorAliases.HtmlPropertyEditor)]
    public virtual string Summary { get; set; }

    [Aggregated]
    public virtual IList<ArticleDetail> ArticleDetail { get; set; } = new ObservableCollection<ArticleDetail>();
}