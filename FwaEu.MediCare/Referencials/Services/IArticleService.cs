using System.Collections.Generic;
using System.Threading.Tasks;

namespace FwaEu.MediCare.Referencials.Services
{
    public interface IArticleService
    {
        Task<List<ArticleEntity>> GetAllAsync();
        Task<List<GetArticlesBySearchResponse>> GetAllBySearchAsync(GetArticlesBySearchPost model);
    }
}
