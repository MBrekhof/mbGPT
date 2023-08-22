#nullable enable
using DevExpress.Persistent.Base;

namespace DocGPT.Module.BusinessObjects
{
    [DefaultClassOptions]
    public class ChatModel: BaseObjectInt
    {
        public virtual string Name { get; set; } = "ChatModel";
        public virtual int? Size { get; set; }
        public virtual float? Tokencost { get; set; }
    }
}
