#nullable enable
using DevExpress.Persistent.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
