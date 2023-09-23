using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using mbGPT.Module.BusinessObjects;
using mbGPT.Module.Services;
using Microsoft.Extensions.DependencyInjection;
using OpenAI;
using Pgvector;

namespace mbGPT.Module.Controllers
{
    // implement logging and error handling using Serilog
    public partial class VCEmbed : ViewController
    {
        private readonly IServiceProvider serviceProvider;
        [ActivatorUtilitiesConstructor]
        public VCEmbed(IServiceProvider serviceProvider) : this()
        {
            this.serviceProvider = serviceProvider;
        }

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
            var content = CO_Embed.CodeObjectContent;
            //// Create an instance of the OpenAI client
            if (content.Length > 0)
            {
                var settingsService = serviceProvider.GetService<SettingsService>();
                var settings = await settingsService.GetSettingsAsync();
                Application.ShowViewStrategy.ShowMessage(string.Format("Embedding started!"), displayInterval: 2000);
                var api = new OpenAIClient(new OpenAIAuthentication(settings.OpenAIKey));
                var model = await api.ModelsEndpoint.GetModelDetailsAsync(settings.EmbeddingModel.Name);
               if (CO_Embed.Tags.Count > 0)
                {
                    foreach(var tag in CO_Embed.Tags)
                    {
                        content += " " + tag.TagName + " ";
                    }
                }
                //IObjectSpace EmbedObjectSpace = Application.CreateObjectSpace(typeof(Cost));
                var embeddings = await api.EmbeddingsEndpoint.CreateEmbeddingAsync("Source : "+CO_Embed.Subject+"Category :"+CO_Embed.Category.Category+" "+ content, model);
                var x = new Vector("[" + String.Join(",", embeddings.Data[0].Embedding) + "]");
                CO_Embed.VectorDataString = x;
                CO_Embed.Tokens = (int)embeddings.Usage.TotalTokens;

                Cost cost = ObjectSpace.CreateObject<Cost>();
                cost.CodeObject = CO_Embed;
                cost.SourceType = SourceType.CodeObject;
                cost.PromptTokens = embeddings.Usage.PromptTokens;
                cost.CompletionTokens = embeddings.Usage.CompletionTokens;
                cost.TotalTokens = embeddings.Usage.TotalTokens;
                cost.LlmAction = LlmAction.embedding;

                //EmbedObjectSpace.CommitChanges();
                //ObjectSpace.CommitChanges();
                Application.ShowViewStrategy.ShowMessage(string.Format("Embedded!"), displayInterval: 2000);
            }
            else
            {
                Application.ShowViewStrategy.ShowMessage(string.Format("Nothing to embed.."),displayInterval:2000);
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
