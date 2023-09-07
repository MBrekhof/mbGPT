#nullable enable
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using Pgvector;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;


namespace DocGPT.Module.BusinessObjects;
[DefaultClassOptions]
[NavigationItem("Knowledge")]
[DefaultProperty("ArticleDetailName")]

public partial class ArticleDetail : BaseObjectNoID
{
    [System.ComponentModel.DataAnnotations.Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public virtual int ArticleDetailId { get; set; }

    public virtual int ArticleId { get; set; }
    public virtual string? ArticleDetailName
    {
        get { return Article?.ArticleName + "(" + ArticleSequence.ToString() + ")"; }
    }

    public virtual int ArticleSequence { get; set; }
    [FieldSize(FieldSizeAttribute.Unlimited)]
    public virtual string? ArticleContent { get; set; }

    public virtual Article? Article { get; set; }

    public virtual int Tokens { get; set; }

    [VisibleInDetailView(false),VisibleInListView(false),VisibleInLookupListView(false)]
    [Column(TypeName = "vector(1536)")]
    public virtual Vector? VectorDataString { get; set; }

}