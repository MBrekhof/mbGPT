using DevExpress.ExpressApp.Data;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using System.ComponentModel;

namespace DocGPT.Module.BusinessObjects
{
    //[DomainComponent]
    public partial class SimilarContentArticlesResult //: BaseObjectNoID
    {
        //[Browsable(false), Key]
        //[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public virtual long ID { get; set; }
        public virtual string ArticleName { get; set; }
        public virtual string ArticleContent { get; set; }
        public virtual int? ArticleSequence { get; set; }
        public virtual double cosine_distance { get; set; } = 0;
    }
}
