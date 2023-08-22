

using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.EF;
using System.ComponentModel;

namespace DocGPT.Module.BusinessObjects
{
    [DefaultClassOptions]
    [FileAttachment("File")]
    [NavigationItem("Files")]
    //[DefaultProperty(nameof(FileNameToSave))]
    public class FileSystemStoreObject : BaseObjectInt
    {
        [ExpandObjectMembers(ExpandObjectMembers.Never), ImmediatePostData]
        public virtual Base.FileSystemStoreObjectBase File { get; set; }

        //public string FileNameToSave
        //{
        //    get { return File.FileName; }
        //}

        public virtual bool? Processed { get; set; }

        public override void OnCreated()
        {
            base.OnCreated();
            Processed = false;
        }
    }
}
