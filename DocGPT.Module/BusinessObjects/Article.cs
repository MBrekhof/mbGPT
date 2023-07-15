
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.EF;

using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// https://github.com/DevExpress-Examples/XAF_how-to-store-file-attachments-in-the-file-system-instead-of-the-database

namespace DocGPT.Module.BusinessObjects;
[DefaultClassOptions]
[NavigationItem("Knowledge")]
[DefaultProperty("ArticleName")]
[FileAttachment(nameof(File))]
[Table("Article")]
public partial class Article : BaseObjectNoID
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("ArticleId")]
    public virtual int ArticleId { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    [Column("ArticleName")]
    public virtual string ArticleName { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    [Column("Description")]
    public virtual string Description { get; set; }


    //[ExpandObjectMembers(ExpandObjectMembers.Never)]
    //[FileTypeFilter("DocumentFiles", 1, "*.txt", "*.doc")]
    //[FileTypeFilter("AllFiles", 2, "*.*")]
    //[Column("File")]
    //public virtual FileData? File { get; set; }

    //[InverseProperty("Article")]
    public virtual IList<ArticleDetail> ArticleDetail { get; set; } = new ObservableCollection<ArticleDetail>();
}