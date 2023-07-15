
using DocGPT.Module.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocGPT.Module.Services
{
    public class VectorService
    {
        private readonly DocGPTEFCoreDbContext _dbContext;

        public VectorService(DocGPTEFCoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        // GetSimilarContentArticles
        public List<SimilarContentArticlesResult> GetSimilarContentArticles(string vector)
        {
            return _dbContext.SimilarContentArticles(vector).ToList();
        }


        public List<string> SplitArticleIntoChunks(string articleName, string articleText, int tokenLimit)
        {
            // Split the article text into sentences.
            string[] sentences = articleText.Split(new[] { ". " }, StringSplitOptions.None);

            // Initialize a list to hold the chunks.
            List<string> chunks = new List<string>();

            // Initialize a StringBuilder to build each chunk.
            StringBuilder chunk = new StringBuilder();

            // Initialize a counter to keep track of the number of tokens in the current chunk.
            int tokenCount = 0;

            // Iterate over the sentences.
            foreach (string sentence in sentences)
            {
                // Split the sentence into tokens.
                string[] tokens = sentence.Split(' ');

                // If adding the next sentence would exceed the token limit, add the current chunk to the list.
                if (tokenCount + tokens.Length > tokenLimit)
                {
                    // Add the article name as a header to the chunk.
                    chunks.Add(articleName + "\n" + chunk.ToString());

                    // Start a new chunk with the current sentence.
                    chunk.Clear();
                    chunk.Append(sentence + ". ");

                    // Reset the token count.
                    tokenCount = tokens.Length;
                }
                else
                {
                    // Add the sentence to the current chunk.
                    chunk.Append(sentence + ". ");

                    // Update the token count.
                    tokenCount += tokens.Length;
                }
            }

            // Add the last chunk to the list, if it's not empty.
            if (chunk.Length > 0)
            {
                chunks.Add(articleName + "\n" + chunk.ToString());
            }

            // Return the list of chunks.
            return chunks;
        }
    }

}

