using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DocGPT.Module.BusinessObjects;
using DocGPT.Module.Services;
using Markdig;
using Microsoft.Extensions.DependencyInjection;
using OpenAI;
using OpenAI.Chat;

namespace DocGPT.Module.Controllers
{

    public partial class VCSummarize : ViewController
    {
        private readonly IServiceProvider serviceProvider;

        [ActivatorUtilitiesConstructor]
        public VCSummarize(IServiceProvider serviceProvider) : this()
        {
            this.serviceProvider = serviceProvider;
        }
        public VCSummarize()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
            TargetObjectType = typeof(Article);
            TargetViewType = ViewType.ListView;

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
            if (articleToSummarize.ArticleDetail.Count < 1)
            {
                Application.ShowViewStrategy.ShowMessage(string.Format("Nothing to summarize, remember to split and emmbed first!"));
                return;
            }
            Application.ShowViewStrategy.ShowMessage(string.Format("Starting with the summary!"));
            var settingsService = serviceProvider.GetService<SettingsService>();
            //// Create an instance of the OpenAI client
            var settings = await settingsService.GetSettingsAsync();
            var apiKey = settings.OpenAIKey;
            var api = new OpenAIClient(new OpenAIAuthentication(apiKey));

            var chatMessages = new List<Message>();
            var summaryPrompt = $"You receive one or more chunks of text from the document called {articleToSummarize.ArticleName}, please provide a complete summary of the text. ###";
            var assistantPrompt = "The summary should be formatted using Markdown";
            var totalTokens = 0;
            var gptmodel = settings.ChatModel.Name; // Model.GPT3_5_Turbo_16K;
            var maxTokens = settings.ChatModel.Size;
            chatMessages.Add(new Message(Role.System, summaryPrompt));
            chatMessages.Add(new Message(Role.Assistant, assistantPrompt));
            string SummaryResult = "";
            var teller = 1;
            foreach (var snippet in articleToSummarize.ArticleDetail)
            {
                // Add the existing knowledge to the chatMessages list
                chatMessages.Add(new Message(Role.User, snippet.ArticleContent + "###"));
                totalTokens += snippet.ArticleContent.Length;
                if (totalTokens > maxTokens)
                {
                    var summaryRequest = new ChatRequest(chatMessages, temperature: 0.0, topP: 1, frequencyPenalty: 0, presencePenalty: 0, model: gptmodel);
                    SummaryResult = await api.ChatEndpoint.GetCompletionAsync(summaryRequest);

                    chatMessages.Clear();
                    chatMessages.Add(new Message(Role.System, summaryPrompt));
                    chatMessages.Add(new Message(Role.User, SummaryResult));  // taking previous summary into consideration
                    chatMessages.Add(new Message(Role.Assistant,assistantPrompt));
                    totalTokens = summaryPrompt.Length + SummaryResult.ToString().Length+summaryPrompt.Length;
                    Application.ShowViewStrategy.ShowMessage($"Finished summarizing upto # {teller} / {articleToSummarize.ArticleDetail.Count}");
                }
                teller++;
            }
            // laatsten
            var nrCheck = articleToSummarize.ArticleDetail.Count > 1 ? 3 : 2;
            if (chatMessages.Count > nrCheck)
            {
                chatMessages.Add(new Message(Role.Assistant, "Geef aub de finale conclusie in de taal van het document."));
                var summaryRequest = new ChatRequest(chatMessages, temperature: 0.0, topP: 1, frequencyPenalty: 0, presencePenalty: 0, model: gptmodel);
                SummaryResult = await api.ChatEndpoint.GetCompletionAsync(summaryRequest);
            }

            articleToSummarize.Summary = Markdown.Parse(SummaryResult).ToHtml();
            ObjectSpace.CommitChanges();
                    
            Application.ShowViewStrategy.ShowMessage(string.Format("Summarized (but NOT embedded) !"));
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
