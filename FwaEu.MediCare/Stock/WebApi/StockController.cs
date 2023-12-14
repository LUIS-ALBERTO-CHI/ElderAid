using FwaEu.Fwamework.Data;
using FwaEu.MediCare.Stock.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.MediCare.Stock.WebApi
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class StockController : Controller
    {
        // POST /Stock/StockConsumptionPatient
        [HttpPost("StockConsumptionPatient")]
        public async Task<IActionResult> GetAllStockConsumptionPatient(GetAllStockConsumptionPatientPostApi modelApi,[FromServices] IStockService stockService)
        {
            try
            {
                var models = await stockService.GetAllStockConsumptionPatient(new GetAllStockConsumptionPatientPost
                {
                    PatientId = modelApi.PatientId,
                    Page = modelApi.Page,
                    PageSize = modelApi.PageSize,
                });
                return Ok(models.Select(x => new GetAllStockConsumptionPatientResponseApi()
                {
                    Id = x.Id,
                    ArticleId = x.ArticleId,
                    PatientId = x.PatientId,
                    Quantity = x.Quantity,
                    UpdatedBy = x.UpdatedBy,
                    UpdatedOn = x.UpdatedOn
                }));
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

        // POST /Stock/SearchPharmacyArticles
        [HttpPost("SearchPharmacyArticles")]

        public async Task<IActionResult> GetAllArticlesCabinets(GetAllArticlesInStockPostApi modelApi, [FromServices] IStockService stockService)
        {
            try
            {
                var models = await stockService.GetAllArticlesCabinets(new GetAllArticlesCabinetPost
                {
                    CabinetId = modelApi.CabinetId,
                    SearchTerm = modelApi.SearchTerm,
                    Page = modelApi.Page,
                    PageSize = modelApi.PageSize,
                });
                return Ok(models.Select(x => new GetAllArticlesInStockResponseApi()
                {
                    Id = x.Id,
                    ArticleId = x.ArticleId,
                    Quantity = x.Quantity,
                    MinQuantity = x.MinQuantity,
                    UpdatedOn = x.UpdatedOn,
                    CabinetId = x.CabinetId
                }));
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

        // POST /Stock/Update
        [HttpPost("Update")]
        public async Task<IActionResult> CreateAsync(UpdateStockPostApi model, [FromServices] IStockService stockService)
        {
            try
            {
                await stockService.UpdateAsync(new UpdateStockPost
                {
                    Quantity = model.Quantity,
                    StockId = model.StockId,
					PatientId = model.PatientId
				});
                return Ok();
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }
    }
}