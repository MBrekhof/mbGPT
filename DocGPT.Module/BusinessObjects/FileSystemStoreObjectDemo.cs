

using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.EF;
using DocGPT.Module.BusinessObjects.Base;

namespace DocGPT.Module.BusinessObjects
{
    [DefaultClassOptions]
    [FileAttachment("File")]
    [NavigationItem("Files")]
    public class FileSystemStoreObjectDemo : BaseObject
    {
        [ExpandObjectMembers(ExpandObjectMembers.Never), ImmediatePostData]
        public virtual FileSystemStoreObject File { get; set; }

        public virtual bool? Processed { get; set; }

        public override void OnCreated()
        {
            base.OnCreated();
            Processed = false;
        }
    }
}
