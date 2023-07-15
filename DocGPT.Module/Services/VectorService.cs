
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
    }
}
