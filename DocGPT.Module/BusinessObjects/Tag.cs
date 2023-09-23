using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using System.Collections.ObjectModel;
#nullable enable

using System.ComponentModel.DataAnnotations.Schema;

namespace mbGPT.Module.BusinessObjects
{

    [DefaultClassOptions]
    public class Tag : BaseObjectNoID
    {
        [System.ComponentModel.DataAnnotations.Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        [VisibleInDetailView(false)]
        public virtual int TagId { get; set; }
        [FieldSize(50)]
        [RuleRequiredField]
        public virtual string? TagName { get; set; }

        public virtual IList<CodeObject> CodeObjects { get; set; } = new ObservableCollection<CodeObject>();
    }
}