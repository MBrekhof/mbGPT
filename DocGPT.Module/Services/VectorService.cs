using DocGPT.Module.BusinessObjects;
using Microsoft.EntityFrameworkCore;
using System.Text;


namespace DocGPT.Module.Services
{
    public class VectorService
    {

        private readonly DocGPTEFCoreDbContext _dbContext;
        public VectorService(DocGPTEFCoreDbContext dbContext)
        {

            _dbContext = dbContext;

        }
        public List<SimilarContentArticlesResult> GetSimilarContentArticles(string vector)
        {
            // limit to 5 results, order by best matching (closest to 0) first
            var question = $"SELECT articledetailid as id," +
                $"(select articlename from article a where r.articleid = a.articleid) as articlename," +
                $"articlecontent,articlesequence,'A' as articletype,vectordatastring <=> " +
                $"'{vector}' as cosine_distance  FROM articledetail r ORDER BY vectordatastring <=> '{vector}' LIMIT 5";
            question = question.ToLower();
            List<SimilarContentArticlesResult> r = _dbContext.SimilarContentArticlesResult.FromSqlRaw(question).ToList();
            return r;
        }

        // GetSimilarCode
        public List<SimilarContentArticlesResult> GetSimilarCodeContent(string vector)
        {
            var cdb = _dbContext;
            var question = $"SELECT CodeObjectId as id,Subject as articlename,CodeObjectContent as articlecontent,1 as articlesequence,'C' as articletype,vectordatastring <=> " +
                $"'{vector}' as cosine_distance  FROM codeobject ORDER BY vectordatastring <=> '{vector}' LIMIT 5";
            question = question.ToLower();
            List<SimilarContentArticlesResult> r = cdb.SimilarContentArticlesResult.FromSqlRaw(question).ToList();
            return r;
        }


        public List<string> SplitArticleIntoChunks(string articleName, string articleText, int tokenLimit)
        {
            // Remove line breaks
            articleText = articleText.Replace("\r\n", "");

            // Split the article text by markers
            string markerStart = "<#>";
            string[] sections = articleText.Split(new[] { markerStart }, StringSplitOptions.None);

            // Initialize a list to hold the chunks
            List<string> chunks = new List<string>();

            // Initialize a StringBuilder to build each chunk
            StringBuilder chunk = new StringBuilder();

            // Initialize a counter to keep track of the number of tokens in the current chunk
            int tokenCount = 0;

            // Iterate over the sections
            foreach (string section in sections)
            {
                if (section.Contains(markerStart))  // If the section contains a marker, add it as a whole
                {
                    string markedSection = section.Replace(markerStart, "");
                    string[] tokens = markedSection.Split(' ');
                    if (tokenCount + tokens.Length > tokenLimit)
                    {
                        chunks.Add(chunk.ToString());
                        chunk.Clear();
                        tokenCount = 0;
                    }
                    chunk.Append(markedSection);
                    tokenCount += tokens.Length;
                }
                else  // Otherwise, process it sentence by sentence
                {
                    string[] sentences = section.Split(new[] { ". " }, StringSplitOptions.TrimEntries & StringSplitOptions.RemoveEmptyEntries);
                    foreach (string sentence in sentences)
                    {
                        string[] tokens = sentence.Split(' ');
                        if (tokens.Length < 2) { continue; } // remove single '.'
                        if (tokenCount + tokens.Length > tokenLimit)
                        {
                            chunks.Add(chunk.ToString());
                            chunk.Clear();
                            chunk.Append(sentence + ". ");
                            tokenCount = tokens.Length;
                        }
                        else
                        {
                            chunk.Append(sentence + ". ");
                            tokenCount += tokens.Length;
                        }
                    }
                }
            }

            // Add the last chunk to the list, if it's not empty
            if (chunk.Length > 0)
            {
                chunks.Add(chunk.ToString());
            }

            // Return the list of chunks
            return chunks;
        }
    }



}
