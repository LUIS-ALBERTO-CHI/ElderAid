using FwaEu.Fwamework.Data;
using FwaEu.MediCare.Orders.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.MediCare.Orders.WebApi
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : Controller
    {
        // GET /Orders
        [HttpPost("GetAll")]
        public async Task<IActionResult> GetAll(GetAllOrdersPostApi modelApi, IOrderService orderService)
        {
            try
            {
                var models = await orderService.GetAllAsync(new GetAllOrdersPost
                {
                    PatientId = modelApi.PatientId,
                    Page = modelApi.Page,
                    PageSize = modelApi.PageSize,
                });
                return Ok(models.Select(x => new GetAllOrdersResponseApi()
                {
                    Id = x.Id,
                    ArticleId = x.ArticleId,
                    PatientId = x.PatientId,
                    Quantity = x.Quantity,
                    State = x.State,
                    UpdatedBy = x.UpdatedBy,
                    UpdatedOn = x.UpdatedOn
                }));
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }


        // POST /Orders
        [HttpPost("Create")]
        public async Task<IActionResult> Create(CreateOrdersPostApi[] orders, IOrderService orderService)
        {
            try
            {
                await orderService.CreateOrdersAsync(orders.Select(x => new CreateOrdersPost
                {
                    Quantity = x.Quantity,
                    ArticleId = x.ArticleId,
                    PatientId = x.PatientId
                }).ToArray());
                return Ok();
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }
    }
}
