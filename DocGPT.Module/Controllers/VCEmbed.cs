using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DocGPT.Module.BusinessObjects;
using OpenAI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocGPT.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class VCEmbed : ViewController
    {
        // Use CodeRush to create Controllers and Actions with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/403133/
        public VCEmbed()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
            TargetObjectType = typeof(CodeObject);
            TargetViewType = ViewType.DetailView;
            SimpleAction EmbedAction = new SimpleAction(this, "Embed", "Edit")// PredefinedCategory.View)
            {
                //Specify the Action's button caption.
                Caption = "Embed",
                //Specify the confirmation message that pops up when a user executes an Action.
                //ConfirmationMessage = "Are you sure you can handle the truth?",
                //Specify the icon of the Action's button in the interface.
                ImageName = "Action_Clear"
            };
            //This event fires when a user clicks the Simple Action control. Handle this event to execute custom code.
            EmbedAction.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;
            EmbedAction.Execute += EmbedAction_Execute;
        }

        private async void EmbedAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            var CO_Embed = e.CurrentObject as CodeObject;
            //// Create an instance of the OpenAI client
            if (CO_Embed.CodeObjectContent.Length > 0)
            {
                Application.ShowViewStrategy.ShowMessage(string.Format("Embedding started!"), displayInterval: 50000);
                var api = new OpenAIClient(new OpenAIAuthentication("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"));
                var model = await api.ModelsEndpoint.GetModelDetailsAsync("text-embedding-ada-002");
                //IObjectSpace VectorDataObjectSpace = Application.CreateObjectSpace(typeof(ArticleVectorData));
                var embeddings = await api.EmbeddingsEndpoint.CreateEmbeddingAsync("Source : "+CO_Embed.Subject+"Category :"+CO_Embed.Category.Category+" "+ CO_Embed.CodeObjectContent, model);

                CO_Embed.VectorDataString = "[" + String.Join(",", embeddings.Data[0].Embedding) + "]";
                CO_Embed.Tokens = (int)embeddings.Usage.TotalTokens;
                // Get Embedding Vectors for this chunk
                var EmbeddingVectors = embeddings.Data[0].Embedding.Select(d => (float)d).ToArray();
                // Instert all Embedding Vectors
                for (int i = 0; i < EmbeddingVectors.Length; i++)
                {
                    var embeddingVector = ObjectSpace.CreateObject<ArticleVectorData>();

                    embeddingVector.VectorValueId = i;
                    embeddingVector.VectorValue = EmbeddingVectors[i];

                    CO_Embed.ArticleVectorData.Add(embeddingVector);
                }
                ObjectSpace.CommitChanges();
                Application.ShowViewStrategy.ShowMessage(string.Format("Embedded!"), displayInterval: 50000);
            }
            else
            {
                Application.ShowViewStrategy.ShowMessage(string.Format("Nothing to embed.."),displayInterval:50000);
            }
            ObjectSpace.CommitChanges();
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
