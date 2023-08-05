using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using Pgvector;
using System.ComponentModel.DataAnnotations.Schema;


namespace DocGPT.Module.BusinessObjects;
[DefaultClassOptions]
[NavigationItem("Knowledge")]

public partial class ArticleDetail : BaseObjectNoID
{
    [System.ComponentModel.DataAnnotations.Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public virtual int ArticleDetailId { get; set; }

    public virtual int ArticleId { get; set; }

    public virtual int ArticleSequence { get; set; }
    [FieldSize(FieldSizeAttribute.Unlimited)]
    public virtual string ArticleContent { get; set; }

    [ForeignKey("articleid")]
     [InverseProperty("ArticleDetail")]
    public virtual Article Article { get; set; }

    public virtual int Tokens { get; set; }

    [VisibleInDetailView(false),VisibleInListView(false),VisibleInLookupListView(false)]
    [Column(TypeName = "vector(1536)")]
    public virtual Vector? VectorDataString { get; set; }

}