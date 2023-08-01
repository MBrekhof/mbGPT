﻿using DevExpress.ExpressApp;
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
        //private readonly DocGPTEFCoreDbContext _dbContext;
        private readonly IServiceProvider serviceProvider;
        private string ApiKey = "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";
        private string EmbeddingModel = "text-embedding-ada-002";
        private int Chat4Limit = 6000;
        private int Chat35Limit = 13000;
        public  OpenAILLMService( IServiceProvider serviceProvider) //, DocGPTEFCoreDbContext dbContext)
        {
            //_dbContext = dbContext;
            this.serviceProvider = serviceProvider;
        }
        public async Task<bool> GetAnswer(SimpleActionExecuteEventArgs e)
        {
            //IObjectSpace newObjectSpace = e.CurrentObject Application.CreateObjectSpace(typeof(Chat));

            var usesLocalKnowledge = false;
            var target = (Chat)e.CurrentObject;
            target.Answer = string.Empty;
            //// Create an instance of the OpenAI client
            var api = new OpenAIClient(new OpenAIAuthentication(ApiKey));

            //// Get the model details
            var model = await api.ModelsEndpoint.GetModelDetailsAsync(EmbeddingModel);

            // two step text insertion/replacement
            var text = target.Prompt.PromptBody;
            var TheQuestion = text.Replace("{{question}}", target.Question);

            var embeddings = await api.EmbeddingsEndpoint.CreateEmbeddingAsync(TheQuestion, model);
            target.QuestionDataString = "[" + String.Join(",", embeddings.Data[0].Embedding) + "]";

            var serviceOne = serviceProvider.GetRequiredService<VectorService>();
            //similar content
 
            var SimilarContentArticles = serviceOne.GetSimilarCodeContent(target.QuestionDataString);
            var codeHits = serviceOne.GetSimilarContentArticles(target.QuestionDataString);

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
            Model gptmodel = target.ChatModel == ChatModel.GPT4 ? Model.GPT4 : Model.GPT3_5_Turbo_16K;
            var maxTokens = gptmodel == Model.GPT4 ? Chat4Limit : Chat35Limit;

            foreach (var snippet in SimilarContentArticles)
            {
                // Add the existing knowledge to the chatMessages list
                chatMessages.Add(new Message(Role.System, snippet.ArticleContent + "###"));
                totalTokens += snippet.ArticleContent.Length;
                if (totalTokens > maxTokens) { break; }
            }
            chatMessages.Add(new Message(Role.User, TheQuestion));

            var chatRequest = new ChatRequest(chatMessages, temperature: 0.0, topP: 1, frequencyPenalty: 0, presencePenalty: 0, model: gptmodel);

            var result = await api.ChatEndpoint.GetCompletionAsync(chatRequest);

            target.Answer = Markdown.Parse(result).ToHtml();
            //target.Answer = result;
            target.Tokens = result.Usage.TotalTokens;
            target.Created = DateTime.Now;
            // newObjectSpace.CommitChanges();
            return usesLocalKnowledge;
        }
    }
}
