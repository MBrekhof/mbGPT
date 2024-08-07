﻿#nullable enable

using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using Microsoft.EntityFrameworkCore;
using Pgvector;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mbGPT.Module.BusinessObjects
{
    [DefaultClassOptions]
    [NavigationItem("Code")]
    [DefaultProperty("Subject")]
    public partial class CodeObject : BaseObjectNoID
    {
        [System.ComponentModel.DataAnnotations.Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [VisibleInDetailView(false)]
        public virtual int CodeObjectId { get; set; }

        [FieldSize(200)]
        [RuleRequiredField]
        public virtual string? Subject { get; set; }

        //public virtual int CodeObjectCategoryId { get; set; }
        [ForeignKey("CodeObjectCategoryId")]
        [RuleRequiredField]
        public virtual required CodeObjectCategory Category { get; set; }

        [FieldSize(FieldSizeAttribute.Unlimited)]
        public virtual required string CodeObjectContent { get; set; }

        public virtual int Tokens { get; set; }

        [StringLength(250)]
        [Unicode(false)]
        public virtual string? EmbeddedWith { get; set; }

        //[FieldSize(FieldSizeAttribute.Unlimited)]
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        [Column(TypeName = "vector(1536)")]
        public virtual Vector? VectorDataString { get; set; }

        public virtual IList<Tag> Tags { get; set; } = new ObservableCollection<Tag>();
    }
}
