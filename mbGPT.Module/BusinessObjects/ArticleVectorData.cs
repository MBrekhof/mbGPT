
using DevExpress.Persistent.Base;
using System.CodeDom;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocGPT.Module.BusinessObjects;
[DefaultClassOptions]
[NavigationItem("Knowledge")]
public partial class ArticleVectorData : BaseObjectNoID
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public virtual int ArticleVectorDataId { get; set; }

    public virtual int ArticleDetailId { get; set; } = 0;
    public virtual int CodeObjectId { get; set; } = 0;

    [Column("vector_value_id")]
    public virtual int VectorValueId { get; set; }

    [Column("vector_value")]
    public virtual double VectorValue { get; set; }

    //[ForeignKey("ArticleDetailId")]
    //public virtual ArticleDetail ArticleDetail { get; set; }
    //[ForeignKey("CodeObjectId")]
    //public virtual CodeObject CodeObject { get; set; }
}