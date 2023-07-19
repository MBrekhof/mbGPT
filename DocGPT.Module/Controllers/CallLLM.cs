using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;
using DocGPT.Module.BusinessObjects;
using DocGPT.Module.Services;
using Microsoft.Extensions.DependencyInjection;
using OpenAI;
using OpenAI.Chat;
using OpenAI.Models;

namespace DocGPT.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class CallLLM : ViewController
    {
        private readonly IServiceProvider serviceProvider;

        public CallLLM()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
            TargetObjectType = typeof(Chat);
            TargetViewType = ViewType.DetailView;

            SimpleAction AskAction = new SimpleAction(this, "Ask a question", "Edit" )// PredefinedCategory.View)
            {
                //Specify the Action's button caption.
                Caption = "Ask",
                //Specify the confirmation message that pops up when a user executes an Action.
                //ConfirmationMessage = "Are you sure you can handle the truth?",
                //Specify the icon of the Action's button in the interface.
                ImageName = "Action_Clear"
            };
            //This event fires when a user clicks the Simple Action control. Handle this event to execute custom code.
            AskAction.Execute += AskAction_Execute;
        }

        [ActivatorUtilitiesConstructor]
        public CallLLM(IServiceProvider serviceProvider) : this()
        {
            this.serviceProvider = serviceProvider;
        }

        private async void AskAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            IObjectSpace newObjectSpace = Application.CreateObjectSpace(typeof(Chat));

            Application.ShowViewStrategy.ShowMessage(string.Format("Looking for the answer!"),displayInterval:5000, position:InformationPosition.Top);
            var target = (Chat)e.CurrentObject;
            target.Answer = string.Empty;
            //// Create an instance of the OpenAI client
            var api = new OpenAIClient(new OpenAIAuthentication("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"));

            //// Get the model details
            var model = await api.ModelsEndpoint.GetModelDetailsAsync("text-embedding-ada-002");

            // two step text insertion/replacement
            var text = target.Prompt.PromptBody;
            var TheQuestion = text.Replace("{{question}}", target.Question);

            var embeddings = await api.EmbeddingsEndpoint.CreateEmbeddingAsync(TheQuestion, model);
            target.QuestionDataString = "[" + String.Join(",", embeddings.Data[0].Embedding) + "]";

            var serviceOne = serviceProvider.GetRequiredService<VectorService>();
            var SimilarContentArticles = serviceOne.GetSimilarCodeContent(target.QuestionDataString);
            var codeHits = serviceOne.GetSimilarContentArticles(target.QuestionDataString);
            SimilarContentArticles.AddRange(codeHits);

            var aantal = SimilarContentArticles.Count;
            if (aantal > 0) {
                SimilarContentArticles.Sort((a, b) => b.cosine_distance.CompareTo(a.cosine_distance));
                //SimilarContentArticles = SimilarContentArticles.Take(10).ToList();
            }


            // Create a new list of chatMessages objects
            var chatMessages = new List<Message>();
            //var chatMessages = new List<string>();
            // Add the user input to the chatMessages list
            var totalTokens = 0;
  
            foreach (var snippet in SimilarContentArticles)
            {
                // Add the existing knowledge to the chatMessages list
                chatMessages.Add(new Message(Role.System, snippet.ArticleContent+"###"));
                //chatMessages.Add( snippet.ArticleContent + " ### ");
                totalTokens += snippet.ArticleContent.Length;
                if(totalTokens > 8000) { break; }
            }
            chatMessages.Add(new Message(Role.User, TheQuestion));

            var chatRequest = new ChatRequest(chatMessages,temperature: 0.0, topP: 1, frequencyPenalty: 0, presencePenalty: 0, model: Model.GPT3_5_Turbo_16K);
            var result = await api.ChatEndpoint.GetCompletionAsync(chatRequest);
            //var result = await api.CompletionsEndpoint.CreateCompletionAsync(prompts: chatMessages, temperature: 0.1, model: "text-davinci-003");



            target.Answer = result;
            newObjectSpace.CommitChanges();
            Application.ShowViewStrategy.ShowMessage(string.Format("Answered!"));
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
