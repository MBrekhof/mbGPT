
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Data;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.EF;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace mbGPT.Module.BusinessObjects
{

    [DefaultProperty(nameof(FileName))]
    [DomainComponent]
    public partial class ChunkedKnowledge : BaseObjectNoID
    {
        [Browsable(false), Key]
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public virtual Guid ID { get; set; }
        public virtual string FileName { get; set; }

        public virtual string RealFileName { get; set; }
        public virtual Guid? FileId { get; set; }
        public virtual int? FileSize { get; set; }
        public virtual int ChunkSize { get; set; } = 500;
        [NotMapped]
        [VisibleInDetailView(false),VisibleInListView(false),VisibleInLookupListView(false)]
        public virtual ICollection<string> DocChunks { get; set; } = new ObservableCollection<string>();



    }
}
