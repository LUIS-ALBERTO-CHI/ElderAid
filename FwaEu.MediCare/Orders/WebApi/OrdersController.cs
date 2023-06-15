using FwaEu.Fwamework.Data;
using FwaEu.MediCare.Referencials.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FwaEu.MediCare.Orders.WebApi
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : Controller
    {
        // GET /Orders
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                return Ok();
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }


        // POST /Orders
        [HttpPost]
        public async Task<IActionResult> Create(CreateOrdersPostApi[] orders, IBuildingService buildingService)
        {
            try
            {
                var result = await buildingService.GetAllAsync();
                return Ok();
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }
    }
}
