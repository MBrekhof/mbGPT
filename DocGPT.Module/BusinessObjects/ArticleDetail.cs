using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using System.Collections.ObjectModel;
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

    [ForeignKey("ArticleId")]
    // [InverseProperty("ArticleDetail")]
    public virtual Article Article { get; set; }

    [FieldSize(FieldSizeAttribute.Unlimited)]
    public virtual string VectorDataString { get; set; }
    public virtual IList<ArticleVectorData> ArticleVectorData { get; set; } = new ObservableCollection<ArticleVectorData>();
}