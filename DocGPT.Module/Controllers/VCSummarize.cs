using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.Pdf.Native.BouncyCastle.Asn1.X509;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DocGPT.Module.BusinessObjects;
using OpenAI;
using OpenAI.Chat;
using OpenAI.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DocGPT.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class VCSummarize : ViewController
    {

        public VCSummarize()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
            TargetObjectType = typeof(Article);
            TargetViewType = ViewType.DetailView;

            SimpleAction SummarizeAction = new SimpleAction(this, "Summarize", "Edit")// PredefinedCategory.View)
            {
                //Specify the Action's button caption.
                Caption = "Summarize",
                //Specify the confirmation message that pops up when a user executes an Action.
                //ConfirmationMessage = "Are you sure you can handle the truth?",
                //Specify the icon of the Action's button in the interface.
                ImageName = "Action_Clear"
            };
            //This event fires when a user clicks the Simple Action control. Handle this event to execute custom code.
            SummarizeAction.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;
            SummarizeAction.Execute += SummarizeAction_Execute;
        }

        private async void SummarizeAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {


            var articleToSummarize = e.CurrentObject as Article;
            //// Create an instance of the OpenAI client
            var api = new OpenAIClient(new OpenAIAuthentication("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"));

            //// Get the model details
            var model = await api.ModelsEndpoint.GetModelDetailsAsync("text-embedding-ada-002");
            var summaryPrompt = $"You receive one or more chunks of text from the document called {articleToSummarize.ArticleName}, please provide a concise summary of the text (max 200 tokens). ###";
            if (articleToSummarize != null)
            {
                if(articleToSummarize.ArticleDetail.Count > 0)
                {
                    //string summary = summaryPrompt;
                    List<ArticleDetail> articles = new List<ArticleDetail>();
                    foreach (var chunk in articleToSummarize.ArticleDetail)
                    {
                        articles.Add(chunk);
                    }
                    List<string> summaries = new List<string>();
                    var teller = 0;
                    var tokencount = 0;
                    var partsummaries = "";
                    foreach (var article in articles)
                    {
                        tokencount += article.ArticleContent.Length;
                        partsummaries += partsummaries + " " + article.ArticleContent;
                        if (tokencount > 3000)
                        {
                            var result = await api.CompletionsEndpoint.CreateCompletionAsync(prompt: $"Vat deze tekst samen: {partsummaries}", maxTokens: 150, model: Model.Davinci);
                            summaries.Add(result.Completions[0].Text);
                            tokencount = 0;
                            partsummaries = " ";
                            Application.ShowViewStrategy.ShowMessage($"Finished summarizing upto # {teller} / {articles.Count}");
                        }
                        teller++;                        
                    }
                    if (partsummaries.Length > 0)
                    {
                        var result = await api.CompletionsEndpoint.CreateCompletionAsync(prompt: $"Vat deze tekst samen: {partsummaries}", maxTokens: 150, model: Model.Davinci);
                        summaries.Add(result.Completions[0].Text);
                    }

                    // Joining the individual summaries to form one final text for final summary
                    var joinedSummary = string.Join(" ", summaries);

                    // Creating the final summary
                    var finalSummaryResult = await api.CompletionsEndpoint.CreateCompletionAsync(prompt: $"Je bent een vaardige Nederlandse schrijver, schijf een goede samenvatting met betrekking tot deze tekst: {joinedSummary}", maxTokens: 500, model: Model.Davinci);

                    // Final summary
                    var finalSummary = finalSummaryResult.Completions.First().Text.Trim();


                    articleToSummarize.Summary = finalSummary;
                    ObjectSpace.CommitChanges();
                }
            }
            Application.ShowViewStrategy.ShowMessage(string.Format("Summarized!"));
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
    }
}
