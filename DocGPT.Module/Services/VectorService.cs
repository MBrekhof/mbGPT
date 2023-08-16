using DocGPT.Module.BusinessObjects;
using Microsoft.EntityFrameworkCore;
using System.Text;


namespace DocGPT.Module.Services
{
    public class VectorService
    {

        private readonly CustomDbContext _dbContext;

        public VectorService(CustomDbContext dbContext)
        {

            _dbContext = dbContext;

        }
        // GetSimilarContentArticles
        public List<SimilarContentArticlesResult> GetSimilarContentArticles(string vector)
        {
            var cdb = _dbContext.CreateDbContext();
            var question = $"SELECT articledetailid as id," +
                $"(select articlename from article a where r.articledetailid = a.articleid) as articlename," +
                $"articlecontent,articlesequence,vectordatastring <=> " +
                $"'{vector}' as cosine_distance  FROM articledetail r ORDER BY vectordatastring <=> '{vector}' LIMIT 5";
            question = question.ToLower();
            List<SimilarContentArticlesResult> r = cdb.SimilarContentArticlesResult.FromSqlRaw(question).ToList();
            return r;
        }

        // GetSimilarCode
        public List<SimilarContentArticlesResult> GetSimilarCodeContent(string vector)
        {
            var cdb = _dbContext;
            var question = $"SELECT CodeObjectId as id,Subject as articlename,CodeObjectContent as articlecontent,1 as articlesequence,vectordatastring <=> " +
                $"'{vector}' as cosine_distance  FROM codeobject ORDER BY vectordatastring <=> '{vector}' LIMIT 5";
            question = question.ToLower();
            List<SimilarContentArticlesResult> r = cdb.SimilarContentArticlesResult.FromSqlRaw(question).ToList();          
            return r;
        }
        //public async Task<EmbeddingsResponse> CreateEmbeddingAsync(string articleContent, string model)
        //{
        //    //// Create an instance of the OpenAI client
        //    var api = new OpenAIClient(new OpenAIAuthentication("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"));

        //    //// Get the model details
        //    var mymodel = await api.ModelsEndpoint.GetModelDetailsAsync("text-embedding-ada-002");
        //    var embedding = await api.EmbeddingsEndpoint.CreateEmbeddingAsync(articleContent, model);
        //    return embedding;
        //}

        public List<string> SplitArticleIntoChunks(string articleName, string articleText, int tokenLimit)
        {
            // remove line breaks
            articleText = articleText.Replace("\r\n", "");
            // Split the article text into sentences.
            string[] sentences = articleText.Split(new[] { ". " }, StringSplitOptions.TrimEntries & StringSplitOptions.RemoveEmptyEntries);

            // Initialize a list to hold the chunks.
            List<string> chunks = new List<string>();

            // Initialize a StringBuilder to build each chunk.
            StringBuilder chunk = new StringBuilder();

            // Initialize a counter to keep track of the number of tokens in the current chunk.
            int tokenCount = 0;
            List<int> lengtes = new List<int>();

            // Iterate over the sentences.
            foreach (string sentence in sentences)
            {
                // Split the sentence into tokens.
                string[] tokens = sentence.Split(' ');
                if(tokens.Length < 2) { continue; } // remove single '.' 
                lengtes.Add(tokens.Length);
                
                // If adding the next sentence would exceed the token limit, add the current chunk to the list.
                if (tokenCount + tokens.Length > tokenLimit)
                {
                    // Add the article name as a header to the chunk.
                    //chunks.Add(articleName + "\n" + chunk.ToString());
                    if (chunk.Length < (2 * 1024))
                    {
                        chunks.Add( chunk.ToString());
                    }
                    else
                    {
                        var splitter = chunk.ToString();
                        var splitList = splitter.Chunk((2 * 1024)).Select(s => new string(s)).ToList();
                        foreach (var split in splitList)
                        {
                            chunks.Add( split);
                        }

                    }
                    // Start a new chunk with the current sentence.
                    chunk.Clear();
                    chunk.Append(sentence + ". ");

                    // Reset the token count.
                    tokenCount = tokens.Length;
                    lengtes.Clear();
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
                if (chunk.Length < (2 * 1024))
                {
                    chunks.Add( chunk.ToString());
                }
                else
                {
                    var splitter = chunk.ToString();
                    var splitList = splitter.Chunk((2*1024)).Select(s => new string(s)).ToList();
                    foreach (var split in splitList)
                    {
                        chunks.Add(split);
                    }
                                      
                }
            }

            // Return the list of chunks.
            return chunks;
        }
    }

}

