using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using System.ComponentModel.DataAnnotations;

// preferred way to define a Key
namespace mbGPT.Module.BusinessObjects
{
    public abstract class BaseObjectInt : IXafEntityObject, IObjectSpaceLink
    {
        protected IObjectSpace ObjectSpace;

        //
        // Summary:
        //     The key property for the DevExpress.Persistent.BaseImpl.EF.BaseObject class.
        [Key]
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        public virtual int ID { get; set; }

        IObjectSpace IObjectSpaceLink.ObjectSpace
        {
            get
            {
                return ObjectSpace;
            }
            set
            {
                ObjectSpace = value;
            }
        }

        //
        // Summary:
        //     Partially implements the DevExpress.ExpressApp.IXafEntityObject interface in
        //     the DevExpress.Persistent.BaseImpl.EF.BaseObject class.
        public virtual void OnCreated()
        {
        }

        //
        // Summary:
        //     Partially implements the DevExpress.ExpressApp.IXafEntityObject interface in
        //     the DevExpress.Persistent.BaseImpl.EF.BaseObject class.
        public virtual void OnSaving()
        {
        }

        //
        // Summary:
        //     Partially implements the DevExpress.ExpressApp.IXafEntityObject interface in
        //     the DevExpress.Persistent.BaseImpl.EF.BaseObject class.
        public virtual void OnLoaded()
        {
        }
    }
}
