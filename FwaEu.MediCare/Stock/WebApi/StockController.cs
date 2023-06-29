﻿using FwaEu.Fwamework.Data;
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
        // GET /Orders
        [HttpPost("StockConsumptionPatient")]
        public async Task<IActionResult> GetAllStockConsumptionPatient(GetAllStockConsumptionPatientPostApi modelApi, IStockService stockService)
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


    }
}
