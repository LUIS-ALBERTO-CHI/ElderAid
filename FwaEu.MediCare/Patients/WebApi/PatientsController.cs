﻿using FwaEu.Fwamework.Data;
using FwaEu.Fwamework.Permissions.WebApi;
using FwaEu.MediCare.Patients.Services;
using FwaEu.TemplateCore.FarmManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.MediCare.Patients.WebApi
{
    [Authorize]
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
                var modelApi = model == null ? new GetIncontinenceLevelApi()
                                            : new GetIncontinenceLevelApi()
                                            {
                                                Id = model.Id,
                                                Consumed = model.Consumed,
                                                AnnualFixedPrice = model.AnnualFixedPrice,
                                                DailyFixedPrice = model.DailyFixedPrice,
                                                DailyProtocolEntered = model.DailyProtocolEntered,
                                                DateEnd = model.DateEnd,
                                                DateStart = model.DateStart,
                                                FixedPrice = model.FixedPrice,
                                                IncontinenceLevel = (IncontinenceLevel)model.IncontinenceLevel,
                                                OverPassed = model.OverPassed,
                                                VirtualDateWithoutOverPassed = model.VirtualDateWithoutOverPassed
                                            };
                return Ok(modelApi);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost("SaveIncontinenceLevel")]
        [RequirePermissions(nameof(PatientPermissionProvider.CanChangeIncontinenceLevel))]
        public async Task<IActionResult> SaveIncontinenceLevelAsync(SaveIncontinenceLevelApi model, IPatientService patientService)
        {
            try
            {
                await patientService.SaveIncontinenceLevelAsync(new SaveIncontinenceLevel()
                {
                    Id = model.Id,
                    Level = model.Level,
                    DateStart = model.DateStart,
                    DateEnd = model.DateEnd
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