
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Data;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.EF;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocGPT.Module.BusinessObjects
{

    [DefaultProperty(nameof(Text))]
    [ImageName("BO_Document")]
    [DomainComponent]
    public class SplitAndEmbed : BaseObjectNoID
    {
        [Browsable(false), Key]
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public virtual Guid ID { get; set; }
        public virtual string FileName { get; set; }

        public virtual string RealFileName { get; set; }
        public virtual Guid? FileId { get; set; }

        [FieldSize(FieldSizeAttribute.Unlimited)]
        public virtual string Text { get; set; }
        public virtual int? FileSize { get; set; }
        public virtual int ChunkSize { get; set; } = 500;
        public virtual int OverlapSize { get; set; } = 50;
        // public ICollection<string> Chunks { get; set; } = new List<string>();
        [NotMapped]
        public virtual ICollection<string> DocChunks { get; set; } = new ObservableCollection<string>();

        //public override void OnCreated()
        //{
        //    base.OnCreated();
        //    ChunkSize = 500;
        //    OverlapSize = 50;

        //}

    }
}
