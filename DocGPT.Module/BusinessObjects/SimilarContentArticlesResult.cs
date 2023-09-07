using DevExpress.ExpressApp.Data;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using System.ComponentModel;

namespace DocGPT.Module.BusinessObjects
{
    [DomainComponent]
    public partial class SimilarContentArticlesResult : BaseObjectNoID
    {
        [Browsable(false), Key]
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public virtual long id { get; set; }
        public virtual string articlename { get; set; }
        public virtual string articlecontent { get; set; }
        public virtual int? articlesequence { get; set; }
        public virtual char articletype { get; set; } = 'U';  // unknown, article, code
        public virtual double cosine_distance { get; set; } = 0;
    }
}
