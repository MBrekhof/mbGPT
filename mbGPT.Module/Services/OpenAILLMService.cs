using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using mbGPT.Module.BusinessObjects;
using Markdig;
using Microsoft.Extensions.DependencyInjection;
using OpenAI;
using OpenAI.Chat;
//using static System.Net.Mime.MediaTypeNames;

namespace mbGPT.Module.Services
{
    public class OpenAILLMService
    {
        private readonly IServiceProvider serviceProvider;
        private Settings settings;

        public  OpenAILLMService( IServiceProvider serviceProvider,SettingsService settingsService) 
        {            
            this.serviceProvider = serviceProvider;
            this.settings = settingsService.GetSettingsAsync().GetAwaiter().GetResult(); 
        }
        public async Task<bool> GetAnswer(Chat chat, IObjectSpace objectSpace)
        {
            var usesLocalKnowledge = false;
            var target = chat;
            if (target.ChatModel == null)
                return false;
            if ((target.Question == null) || (target.Prompt.SystemPrompt==null))
                return false;

            target.Answer = string.Empty;
            //// Create an instance of the OpenAI client
            var api = new OpenAIClient(new OpenAIAuthentication(settings.OpenAIKey));

            //// Get the model details
            var model = await api.ModelsEndpoint.GetModelDetailsAsync(settings.EmbeddingModel.Name);
          
            // two step text insertion/replacement
            var text = target.Prompt.SystemPrompt;
            var TheQuestion = text + " " + target.Question;
            if (target.Prompt.AssistantPrompt != null)
                TheQuestion += " " + target.Prompt.AssistantPrompt;

            var embeddings = await api.EmbeddingsEndpoint.CreateEmbeddingAsync(TheQuestion, model);
            target.QuestionDataString = "[" + String.Join(",", embeddings.Data[0].Embedding) + "]";

            var vectorService = serviceProvider.GetRequiredService<VectorService>();
            //similar content
 
            var SimilarContentArticles = vectorService.GetSimilarCodeContent(target.QuestionDataString);
            var codeHits = vectorService.GetSimilarContentArticles(target.QuestionDataString);

            SimilarContentArticles.AddRange(codeHits);
            // TODO: change the hard coded value
            SimilarContentArticles.RemoveAll(a => a.cosine_distance >= 0.20);
            var aantal = SimilarContentArticles.Count;
            if (aantal > 0)
            {
                // low cosine distance is what we look for
                SimilarContentArticles.Sort((a, b) => a.cosine_distance.CompareTo(b.cosine_distance));
                //SimilarContentArticles = SimilarContentArticles.Take(10).ToList();
                usesLocalKnowledge = true;
            }

            // Create a new list of chatMessages objects
            var chatMessages = new List<Message>();

            var totalTokens = 0;
            string gptmodel = target.ChatModel.Name;
            var encoding = Tiktoken.Encoding.ForModel(gptmodel);
            var limitSwitch = 0;

            var maxTokens = (int)(target.ChatModel.Size * 0.7);
            chatMessages.Add(new Message(Role.System, target.Prompt.SystemPrompt));
            chatMessages.Add(new Message(Role.User, target.Question));
            if (target.Prompt.AssistantPrompt != null)
                chatMessages.Add(new Message(Role.Assistant, target.Prompt.AssistantPrompt));
            totalTokens += encoding.CountTokens(TheQuestion);
            chatMessages.Add(new Message(Role.Assistant, "Sources: "));
            totalTokens += 2; // assumption


            foreach (var snippet in SimilarContentArticles)
            {
                limitSwitch = totalTokens + encoding.CountTokens(snippet.articlecontent);
                if (limitSwitch > maxTokens) { break; }
                // Add the existing knowledge to the chatMessages list
                chatMessages.Add(new Message(Role.Assistant,"Source: "+snippet.articlename+"("+snippet.articlesequence+") " +snippet.articlecontent  + "###"));
                var uk = objectSpace.CreateObject<UsedKnowledge>();
                uk.Chat = target;
                uk.cosinedistance = snippet.cosine_distance;
                if ((snippet.articletype == 'C')|| (snippet.articletype == 'c'))
                {
                    uk.Code = objectSpace.FindObject<CodeObject>(CriteriaOperator.Parse("CodeObjectId = ?", snippet.id));  
                    target.UsedKnowledge.Add(uk);
                }
                else
                {
                    uk.Article = objectSpace.FindObject<ArticleDetail>(CriteriaOperator.Parse("ArticleDetailId = ?", snippet.id));
                    target.UsedKnowledge.Add(uk);
                }
                totalTokens += encoding.CountTokens(snippet.articlecontent);

            }

            //chatMessages.Add(new Message(Role.System, "Sources for the information provided are mentioned after 'Source:', please show them as a list in your answer when asked at the bottom only. "));

            var chatRequest = new ChatRequest(chatMessages, temperature: 0.0, topP: 1, frequencyPenalty: 0, presencePenalty: 0, model: gptmodel);

            var result = await api.ChatEndpoint.GetCompletionAsync(chatRequest);

            Cost cost = objectSpace.CreateObject<Cost>();
            cost.Chat = chat;
            cost.SourceType = SourceType.Chat;
            cost.PromptTokens = result.Usage.PromptTokens;
            cost.CompletionTokens = result.Usage.CompletionTokens;
            cost.TotalTokens = result.Usage.TotalTokens;
            cost.LlmAction = LlmAction.completion;

            target.Answer = Markdown.Parse(result).ToHtml();

            target.Tokens = result.Usage.TotalTokens;
            // TODO: either dbcontext or NODA package
            target.Created = DateTime.UtcNow;

            return usesLocalKnowledge;
        }
    }
}
