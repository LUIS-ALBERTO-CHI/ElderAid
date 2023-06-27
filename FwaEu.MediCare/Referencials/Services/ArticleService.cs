using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.MediCare.GenericRepositorySession;
using NHibernate.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FwaEu.MediCare.Referencials.Services
{
    public class ArticleService : IArticleService
    {
        private readonly GenericSessionContext _sessionContext;

        public ArticleService(GenericSessionContext sessionContext)
        {
            _sessionContext = sessionContext;
        }
        public async Task<List<ArticleEntity>> GetAllAsync()
        {
            var repository = _sessionContext.RepositorySession;
            return await repository.Create<ArticleEntityRepository>().Query().ToListAsync();
        }
    }
}
