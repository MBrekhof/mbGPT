using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using mbGPT.Module.BusinessObjects;
using mbGPT.Module.Services;
using Microsoft.Extensions.DependencyInjection;
using OpenAI;
using Pgvector;

namespace mbGPT.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class VCArticleDetailEmbed : ViewController
    {

        private readonly IServiceProvider serviceProvider;
        [ActivatorUtilitiesConstructor]
        public VCArticleDetailEmbed(IServiceProvider serviceProvider) : this()
        {
            this.serviceProvider = serviceProvider;
        }

        public VCArticleDetailEmbed()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
            TargetObjectType = typeof(ArticleDetail);

            SimpleAction EmbedAction = new SimpleAction(this, "(re)Embed", "Edit")// PredefinedCategory.View)
            {
                //Specify the Action's button caption.
                Caption = "Embed",
                //Specify the confirmation message that pops up when a user executes an Action.
                //ConfirmationMessage = "Are you sure you can handle the truth?",
                //Specify the icon of the Action's button in the interface.
                ImageName = "Action_Clear"
            };
            //This event fires when a user clicks the Simple Action control. Handle this event to execute custom code.
            EmbedAction.SelectionDependencyType = SelectionDependencyType.RequireMultipleObjects;
            EmbedAction.Execute += EmbedAction_Execute;
        }

        private async void EmbedAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            var settingsService = serviceProvider.GetService<SettingsService>();
            var settings = await settingsService.GetSettingsAsync();
            var api = new OpenAIClient(new OpenAIAuthentication(settings.OpenAIKey));
            var model = await api.ModelsEndpoint.GetModelDetailsAsync(settings.EmbeddingModel.Name);

            foreach (var selectedObject in e.SelectedObjects)
            {
                var CO_Embed = selectedObject as ArticleDetail;
                if (CO_Embed == null) continue;

                var content = CO_Embed.ArticleContent;

                if (content.Length > 0)
                {
                    Application.ShowViewStrategy.ShowMessage($"Embedding started for {CO_Embed.ArticleDetailName}!", displayInterval: 2000);


                    var embeddings = await api.EmbeddingsEndpoint.CreateEmbeddingAsync(
                        $"Source : {CO_Embed.Article.ArticleName}/{CO_Embed.ArticleDetailName}  {content}",
                        model,
                        dimensions: 1536
                    );

                    var x = new Vector("[" + String.Join(",", embeddings.Data[0].Embedding) + "]");
                    CO_Embed.VectorDataString = x;
                    CO_Embed.Tokens = (int)embeddings.Usage.TotalTokens;
                    CO_Embed.EmbeddedWith = settings.EmbeddingModel.Name;

                    Cost cost = ObjectSpace.CreateObject<Cost>();
                    cost.ArticleDetail = CO_Embed;
                    cost.SourceType = SourceType.CodeObject;
                    cost.PromptTokens = embeddings.Usage.PromptTokens;
                    cost.CompletionTokens = embeddings.Usage.CompletionTokens;
                    cost.TotalTokens = embeddings.Usage.TotalTokens;
                    cost.LlmAction = LlmAction.embedding;

                    Application.ShowViewStrategy.ShowMessage($"Embedded {CO_Embed.ArticleDetailName}!", displayInterval: 2000);
                }
                else
                {
                    Application.ShowViewStrategy.ShowMessage($"Nothing to embed for {CO_Embed.ArticleDetailName}..", displayInterval: 2000);
                }
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
