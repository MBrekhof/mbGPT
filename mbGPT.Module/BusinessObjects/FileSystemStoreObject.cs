

using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.EF;

namespace mbGPT.Module.BusinessObjects
{
    [DefaultClassOptions]
    [FileAttachment("File")]
    [NavigationItem("Files")]
    public class FileSystemStoreObject : BaseObjectInt
    {
        [ExpandObjectMembers(ExpandObjectMembers.Never), ImmediatePostData]
        public virtual Base.FileSystemStoreObjectBase File { get; set; }

        public virtual bool? Processed { get; set; }

        public virtual DateTime? Created { get; set; } = DateTime.UtcNow;
        public override void OnCreated()
        {
            base.OnCreated();
            Processed = false;
        }
    }
}
