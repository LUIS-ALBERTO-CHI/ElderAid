using System.Collections.Generic;
using System.Threading.Tasks;

namespace FwaEu.MediCare.Articles.Services
{
    public interface IArticleService
    {
        Task<List<GetArticlesBySearchResponse>> GetAllBySearchAsync(GetArticlesBySearchPost model);

        Task<List<GetArticlesByIdsReponse>> GetAllByIdsAsync(int[] ids);

        Task<List<GetArticleImagesByPharmaCodeResponse>> GetArticleImagesByPharmaCodeAsync(int pharmaCode);

    }
}
