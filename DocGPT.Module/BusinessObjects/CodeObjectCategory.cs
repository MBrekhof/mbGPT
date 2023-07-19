using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocGPT.Module.BusinessObjects
{
    // Register this entity in your DbContext (usually in the BusinessObjects folder of your project) with the "public DbSet<CodeObjectCategory> CodeObjectCategorys { get; set; }" syntax.
    [DefaultClassOptions]
    [NavigationItem("Code")]
    [DefaultProperty("Category")]
    public partial class CodeObjectCategory : BaseObjectNoID
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [VisibleInDetailView(false)]
        public virtual int CodeObjectCategoryId { get; set; }

        [FieldSize(50)]
        public virtual string Category { get; set; }
    }
}