using FwaEu.Fwamework.Data;
using FwaEu.MediCare.Referencials.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.MediCare.Referencials.WebApi
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ArticlesController : Controller
    {
        // GET /Articles
        [HttpPost("GetAllBySearchAsync")]
        public async Task<IActionResult> GetAllBySearchAsync(GetArticlesBySearchPostApi modelApi, IArticleService articleService)
        {
            try
            {
                var models = await articleService.GetAllBySearchAsync(new GetArticlesBySearchPost
                {
                    SearchExpression = modelApi.SearchExpression,
                    ArticleFamily = modelApi.ArticleFamily,
                    Page = modelApi.Page,
                    PageSize = modelApi.PageSize,
                });
                return Ok(models.Select(x => new GetArticlesBySearchResponseApi()
                {
                    Id = x.Id,
                    AlternativePackagingCount= x.AlternativePackagingCount,
                    GroupName= x.GroupName,
                    ImageURLs= x.ImageURLs,
                    InvoicingUnit= x.InvoicingUnit,
                    IsFavorite= x.IsFavorite,
                    LikesCount= x.LikesCount,
                    Packaging = x.Packaging,
                    Price= (double)x.Price,
                    AmountRemains =(double)x.AmountRemains,
                    CountInBox = x.CountInBox,
                    SubstitutionsCount = x.SubstitutionsCount,
                    ThumbnailURL= x.ThumbnailURL,
                    Title= x.Title,
                    ArticleType = x.ArticleType,
                    Unit = x.Unit
                }));
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }


    }
}
