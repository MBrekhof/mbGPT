﻿#nullable enable
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;


namespace mbGPT.Module.BusinessObjects;
/// <summary>
/// Has the question/answer text based on local knowledge and a call to the LLM
/// </summary>
[DefaultClassOptions]
[NavigationItem("mbGPT")]
public partial class Chat : BaseObjectNoID
{
    [System.ComponentModel.DataAnnotations.Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

    [VisibleInDetailView(false)]
    public virtual int ChatId { get; set; }

    [FieldSize(FieldSizeAttribute.Unlimited)]
    [VisibleInDetailView(true), VisibleInListView(true), VisibleInLookupListView(true)]
    [RuleRequiredField]
    public virtual string? Question { get; set; }

    [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
    [FieldSize(FieldSizeAttribute.Unlimited)]
    public virtual string? QuestionDataString { get; set; }  // vector!
    [RuleRequiredField]
     public virtual Prompt? Prompt { get; set; }

    [FieldSize(FieldSizeAttribute.Unlimited)]
    public virtual string? Answer { get; set; }

    [VisibleInLookupListView(false)]
    public virtual int? Tokens { get; set; }
    [VisibleInLookupListView(false)]
    [RuleRequiredField]
    public virtual ChatModel? ChatModel { get; set; }

    [VisibleInLookupListView(false)]
    public virtual DateTime? Created { get; set; } = DateTime.UtcNow;
    public virtual IList<UsedKnowledge> UsedKnowledge { get; set; } = new ObservableCollection<UsedKnowledge>();
}



