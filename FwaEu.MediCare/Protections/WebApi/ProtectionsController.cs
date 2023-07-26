using FwaEu.Fwamework.Data;
using FwaEu.MediCare.Protections.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.MediCare.Protections.WebApi
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ProtectionsController : Controller
    {
        //// GET /Orders/GetAll
        //[HttpPost("GetAll")]
        //public async Task<IActionResult> GetAllAsync(GetAllOrdersPostApi modelApi, IOrderService orderService)
        //{
        //    try
        //    {
        //        var models = await orderService.GetAllAsync(new GetAllOrdersPost
        //        {
        //            PatientId = modelApi.PatientId,
        //            Page = modelApi.Page,
        //            PageSize = modelApi.PageSize,
        //        });
        //        return Ok(models.Select(x => new GetAllOrdersResponseApi()
        //        {
        //            Id = x.Id,
        //            ArticleId = x.ArticleId,
        //            PatientId = x.PatientId,
        //            Quantity = x.Quantity,
        //            State = x.State,
        //            UpdatedBy = x.UpdatedBy,
        //            UpdatedOn = x.UpdatedOn
        //        }));
        //    }
        //    catch (NotFoundException)
        //    {
        //        return NotFound();
        //    }
        //}


        //// POST /Orders/Create
        //[HttpPost("Create")]
        //public async Task<IActionResult> CreateAsync(CreateOrdersPostApi[] orders, IOrderService orderService)
        //{
        //    try
        //    {
        //        await orderService.CreateOrdersAsync(orders.Select(x => new CreateOrdersPost
        //        {
        //            Quantity = x.Quantity,
        //            ArticleId = x.ArticleId,
        //            PatientId = x.PatientId
        //        }).ToArray());
        //        return Ok();
        //    }
        //    catch (NotFoundException)
        //    {
        //        return NotFound();
        //    }
        //}

        //// POST /Orders/ValidatePeriodicOrder
        //[HttpPost("ValidatePeriodicOrder")]
        //public async Task<IActionResult> ValidatePeriodicOrderAsync([FromBody] ValidatePeriodicOrderPostApi validatePeriodicOrder, IOrderService orderService)
        //{
        //    try
        //    {
        //        var model = new ValidatePeriodicOrderPost(){
        //             PatientId= validatePeriodicOrder.PatientId,
        //             Articles = validatePeriodicOrder.Articles.Select(x => new ArticleValidatePeriodicOrderPost()
        //             {
        //                 ArticleId= x.ArticleId,
        //                 DefaultQuantity= x.DefaultQuantity,
        //                 Quantity= x.Quantity
        //             }).ToArray()
        //        };
        //        await orderService.ValidatePeriodicOrderAsync(model);
        //        return Ok();
        //    }
        //    catch (NotFoundException)
        //    {
        //        return NotFound();
        //    }
        //}

        // POST /Protections/Update
        [HttpPost("Update")]
        public async Task<IActionResult> UpdateAsync(UpdateProtectionModelApi model, IProtectionService protectionService)
        {
            try
            {
                await protectionService.UpdateProtectionAsync(new UpdateProtectionModel
                {
                    ProtectionId= model.ProtectionId,
                    StartDate= model.StartDate,
                    StopDate= model.StopDate,
                    ProtectionDosages = model.ProtectionDosages,
                });
                return Ok();
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

        // POST /Protections/Stop
        [HttpPost("Stop")]
        public async Task<IActionResult> StopAsync(StopProtectionModelApi model, IProtectionService protectionService)
        {
            try
            {
                await protectionService.StopProtectionAsync(new StopProtectionModel
                {
                    ProtectionId = model.ProtectionId,
                    StopDate = model.StopDate
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