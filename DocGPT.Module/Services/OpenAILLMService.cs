using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DocGPT.Module.BusinessObjects;
using Markdig;
using Microsoft.Extensions.DependencyInjection;
using OpenAI;
using OpenAI.Chat;
using OpenAI.Models;

namespace DocGPT.Module.Services
{
    public class OpenAILLMService
    {
        private readonly IServiceProvider serviceProvider;
        private readonly SettingsService settingsService;
        private Settings settings;

        public OpenAILLMService(IServiceProvider serviceProvider, SettingsService settingsService)
        {
            this.serviceProvider = serviceProvider;
            this.settingsService = settingsService;
        }

        public async Task InitializeAsync()
        {
            this.settings =  this.settingsService.GetSettings();
        }

        public async Task<bool> GetAnswer(SimpleActionExecuteEventArgs e)
        {
            // Ensure settings are initialized
            await InitializeAsync();

            var usesLocalKnowledge = false;
            var target = (Chat)e.CurrentObject;
            if (target.ChatModel == null)
                return false;

            target.Answer = string.Empty;
            //// Create an instance of the OpenAI client
            var api = new OpenAIClient(new OpenAIAuthentication(settings.OpenAIKey));

            //// Get the model details
            var model = await api.ModelsEndpoint.GetModelDetailsAsync(settings.EmbeddingModel.Name);
          
            // two step text insertion/replacement
            var text = target.Prompt.PromptBody;
            var TheQuestion = text.Replace("{{question}}", target.Question);

            var embeddings = await api.EmbeddingsEndpoint.CreateEmbeddingAsync(TheQuestion, model);
            target.QuestionDataString = "[" + String.Join(",", embeddings.Data[0].Embedding) + "]";

            var vectorService = serviceProvider.GetRequiredService<VectorService>();
            //similar content
 
            var SimilarContentArticles = vectorService.GetSimilarCodeContent(target.QuestionDataString);
            var codeHits = vectorService.GetSimilarContentArticles(target.QuestionDataString);

            SimilarContentArticles.AddRange(codeHits);

            var aantal = SimilarContentArticles.Count;
            if (aantal > 0)
            {
                SimilarContentArticles.Sort((a, b) => b.cosine_distance.CompareTo(a.cosine_distance));
                //SimilarContentArticles = SimilarContentArticles.Take(10).ToList();
                usesLocalKnowledge = true;
            }

            // Create a new list of chatMessages objects
            var chatMessages = new List<Message>();

            var totalTokens = 0;
            string gptmodel = target.ChatModel.Name;

            var maxTokens = target.ChatModel.Size;

            foreach (var snippet in SimilarContentArticles)
            {
                // Add the existing knowledge to the chatMessages list
                chatMessages.Add(new Message(Role.System, snippet.articlecontent + "###"));
                totalTokens += snippet.articlecontent.Length;
                if (totalTokens > maxTokens) { break; }
            }
            chatMessages.Add(new Message(Role.User, TheQuestion));

            var chatRequest = new ChatRequest(chatMessages, temperature: 0.0, topP: 1, frequencyPenalty: 0, presencePenalty: 0, model: gptmodel);

            var result = await api.ChatEndpoint.GetCompletionAsync(chatRequest);

            target.Answer = Markdown.Parse(result).ToHtml();
            //target.Answer = result;
            target.Tokens = result.Usage.TotalTokens;
            // TODO: either dbcontext or NODA package
            target.Created = DateTime.SpecifyKind(DateTime.Now,DateTimeKind.Utc);
            // newObjectSpace.CommitChanges();
            return usesLocalKnowledge;
        }
    }
}
