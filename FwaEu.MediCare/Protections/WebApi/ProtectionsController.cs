﻿using FwaEu.Fwamework.Data;
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
        // POST /Protections/Create
        [HttpPost("Create")]
        public async Task<IActionResult> CreateAsync(CreateProtectionModelApi model, IProtectionService protectionService)
        {
            try
            {
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
        }


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
                    ArticleUnit = model.ArticleUnit
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