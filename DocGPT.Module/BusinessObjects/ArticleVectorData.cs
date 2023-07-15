
using DevExpress.Persistent.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocGPT.Module.BusinessObjects;
[DefaultClassOptions]
[NavigationItem("LabwareAI")]
public partial class ArticleVectorData : BaseObjectNoID
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public virtual int ArticleVectorDataId { get; set; }

    public virtual int ArticleDetailId { get; set; }

    [Column("vector_value_id")]
    public virtual int VectorValueId { get; set; }

    [Column("vector_value")]
    public virtual double VectorValue { get; set; }

    [ForeignKey("ArticleDetailId")]
    // [InverseProperty("ArticleVectorData")]
    public virtual ArticleDetail ArticleDetail { get; set; }
}