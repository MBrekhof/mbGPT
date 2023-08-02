
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.EF;

using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DocGPT.Module.BusinessObjects;
[DefaultClassOptions]
[NavigationItem("Knowledge")]
[DefaultProperty("ArticleName")]
[FileAttachment(nameof(File))]
[Table("article")]
public partial class Article : BaseObjectNoID
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    //[Column("articleId")]
    public virtual int ArticleId { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    //[Column("articleName")]
    public virtual string ArticleName { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    [Column("description")]
    public virtual string Description { get; set; }

    [FieldSize(FieldSizeAttribute.Unlimited)]
    [Column("summary")]
    public virtual string Summary { get; set; }

     [InverseProperty("Article")]
    public virtual IList<ArticleDetail> ArticleDetail { get; set; } = new ObservableCollection<ArticleDetail>();
}