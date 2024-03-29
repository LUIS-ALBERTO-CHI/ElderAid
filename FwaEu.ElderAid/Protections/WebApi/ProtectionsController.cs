using FwaEu.Fwamework.Data;
using FwaEu.ElderAid.Protections.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.ElderAid.Protections.WebApi
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ProtectionsController : Controller
    {
        // POST /Protections/Create
        [HttpPost("Create")]
        public async Task<IActionResult> CreateAsync(CreateProtectionModelApi model, [FromServices] IProtectionService protectionService)
        {
            try
            {
                if (model.StopDate < model.StartDate)
                {
                    throw new ArgumentException("Stop date cannot be before start date.");
                }
                await protectionService.CreateProtectionAsync(new CreateProtectionModel
                {
                    PatientId = model.PatientId,
                    ArticleId = model.ArticleId,
                    StartDate = model.StartDate,
                    StopDate = model.StopDate,
                    ProtectionDosages = model.ProtectionDosages,
                    ArticleUnit = model.ArticleUnit
                });
                return Ok();
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // POST /Protections/Update
        [HttpPost("Update")]
        public async Task<IActionResult> UpdateAsync(UpdateProtectionModelApi model, [FromServices] IProtectionService protectionService)
        {
            try
            {
                if (model.StopDate < model.StartDate)
                {
                    throw new ArgumentException("Stop date cannot be before start date.");
                }
                await protectionService.UpdateProtectionAsync(new UpdateProtectionModel
                {
                    ProtectionId= model.ProtectionId,
                    StartDate= model.StartDate,
                    StopDate= model.StopDate,
                    ProtectionDosages = model.ProtectionDosages,
                    ArticleUnit = model.ArticleUnit,
                    PatientId = model.PatientId
                });
                return Ok();
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST /Protections/Stop
        [HttpPost("Stop")]
        public async Task<IActionResult> StopAsync(StopProtectionModelApi model, [FromServices] IProtectionService protectionService)
        {
            try
            {
                await protectionService.StopProtectionAsync(new StopProtectionModel
                {
                    ProtectionId = model.ProtectionId,
                    StopDate = model.StopDate,
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