using FwaEu.Fwamework.Data;
using FwaEu.MediCare.Articles.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.MediCare.Articles.WebApi
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
                    ArticleFamily = (int?)modelApi.ArticleFamily,
                    Page = modelApi.Page,
                    PageSize = modelApi.PageSize,
                });
                return Ok(models.Select(x => new GetArticlesBySearchResponseApi()
                {
                    Id = x.Id,
                    AlternativePackagingCount= x.AlternativePackagingCount,
                    GroupName= x.GroupName,
                    InvoicingUnit= x.InvoicingUnit,
                    IsFavorite= x.IsFavorite,
                    LikesCount= x.LikesCount,
                    LeftAtChargeExplanation = x.LeftAtChargeExplanation,
                    Price= (double)x.Price.GetValueOrDefault(),
                    InsuranceCode = x.InsuranceCode,
                    LeftAtCharge = (double)x.LeftAtCharge.GetValueOrDefault(),
                    CountInBox = x.CountInBox,
                    SubstitutionsCount = x.SubstitutionsCount,
                    Title= x.Title,
                    ArticleType = (ArticleType?)x.ArticleType,
                    Unit = x.Unit,
                    IsGalenicDosageForm = x.IsGalenicDosageForm,
                    IsDeleted = x.IsDeleted == 1,
                    PharmaCode= x.PharmaCode
                }));
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }


        [HttpPost("GetAllByIdsAsync")]
        public async Task<IActionResult> GetAllByIdsAsync([FromBody]int[] ids, [FromServices]IArticleService articleService)
        {
            try
            {
                var models = await articleService.GetAllByIdsAsync(ids);
                return Ok(models.Select(x => new GetArticlesByIdsResponseApi()
                {
                    Id = x.Id,
                    AlternativePackagingCount = x.AlternativePackagingCount,
                    GroupName = x.GroupName,
                    InvoicingUnit = x.InvoicingUnit,
                    IsFavorite = x.IsFavorite,
                    LikesCount = x.LikesCount,
                    LeftAtChargeExplanation = x.LeftAtChargeExplanation,
                    Price = (double)x.Price.GetValueOrDefault(),
                    InsuranceCode = x.InsuranceCode,
                    LeftAtCharge = (double)x.LeftAtCharge.GetValueOrDefault(),
                    CountInBox = x.CountInBox,
                    SubstitutionsCount = x.SubstitutionsCount,
                    Title = x.Title,
                    ArticleType = x.ArticleType,
                    Unit = x.Unit,
                    IsDeleted = x.IsDeleted == 1,
                    IsGalenicDosageForm = x.IsGalenicDosageForm,
                    PharmaCode = x.PharmaCode
                }));
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("{pharmaCode}/Images")]
        public async Task<IActionResult> GetImagesByPharmaCodeAsync([FromRoute] int pharmaCode, [FromServices]IArticleService articleService)
        {
            try
            {
                var models = await articleService.GetArticleImagesByPharmaCodeAsync(pharmaCode);
                return Ok(models.Select(x => new GetArticleImagesByPharmaCodeResponseApi()
                {
                    ImageType = x.ImageType
                }));
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }
    }
}
