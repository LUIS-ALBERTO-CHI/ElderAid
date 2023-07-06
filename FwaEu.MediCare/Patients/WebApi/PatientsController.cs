using FwaEu.Fwamework.Data;
using FwaEu.MediCare.Patients.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.MediCare.Patients.WebApi
{
    //[Authorize]
    [ApiController]
    [Route("[controller]")]
    public class PatientsController : Controller
    {
        // GET /Patients
        [HttpGet("{id}/GetIncontinenceLevel")]

        public async Task<IActionResult> GetIncontinenceLevelAsync([FromRoute] int id, [FromServices] IPatientService patientService)
        {
            try
            {
                var model = await patientService.GetIncontinenceLevelAsync(id);
                return Ok(new GetIncontinenceLevelApi()
                {
                    Id = model.Id,
                    Consumed = model.Consumed,
                    AnnualFixedPrice= model.AnnualFixedPrice,
                    DailyFixedPrice= model.DailyFixedPrice,
                    DailyProtocolEntered= model.DailyProtocolEntered,
                    DateEnd= model.DateEnd,
                    DateStart= model.DateStart,
                    FixedPrice= model.FixedPrice,
                    IncontinenceLevel= model.IncontinenceLevel,
                    OverPassed= model.OverPassed,
                    VirtualDateWithoutOverPassed = model.VirtualDateWithoutOverPassed
                });
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost("SaveIncontinenceLevel")]
        public async Task<IActionResult> SaveIncontinenceLevelAsync(int id, IncontinenceLevel incontinenceLevel, IPatientService patientService)
        {
            try
            {
                await patientService.SaveIncontinenceLevelAsync(id, incontinenceLevel);
                return Ok();
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }
    }
}