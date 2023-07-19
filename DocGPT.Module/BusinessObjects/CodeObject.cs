using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocGPT.Module.BusinessObjects
{
    [DefaultClassOptions]
    [NavigationItem("Code")]
    public partial class CodeObject : BaseObjectNoID
    {
        [System.ComponentModel.DataAnnotations.Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [VisibleInDetailView(false)]
        public virtual int CodeObjectId { get; set; }

        [FieldSize(200)]
        public virtual string Subject { get; set; }

        //public virtual int CodeObjectCategoryId { get; set; }
        [ForeignKey("CodeObjectCategoryId")]
        // [InverseProperty("ArticleDetail")]
        public virtual CodeObjectCategory Category { get; set; }

        [FieldSize(FieldSizeAttribute.Unlimited)]
        public virtual string CodeObjectContent { get; set; }

        public virtual int Tokens { get; set; }

        [FieldSize(FieldSizeAttribute.Unlimited)]
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public virtual string VectorDataString { get; set; }

        public virtual IList<ArticleVectorData> ArticleVectorData { get; set; } = new ObservableCollection<ArticleVectorData>();
    }
}
