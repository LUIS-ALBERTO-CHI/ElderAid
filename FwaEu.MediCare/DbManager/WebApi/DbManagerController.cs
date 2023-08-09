using FwaEu.Fwamework.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FwaEu.MediCare.DbManager.WebApi
{
    //[Authorize]
    [ApiController]
    [Route("[controller]")]
    public class DbManagerController : Controller
    {

        // POST /Orders
        [HttpPost]
        public async Task<IActionResult> Create(string database)
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
    }
}
