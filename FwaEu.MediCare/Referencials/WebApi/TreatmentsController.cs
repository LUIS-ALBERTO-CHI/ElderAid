﻿using FwaEu.Fwamework.Data;
using FwaEu.MediCare.Referencials.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.MediCare.Referencials.WebApi
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class TreatmentsController : Controller
    {
        // GET /GetAllTreatmentsByPatientAsync
        [HttpPost("GetAllTreatmentsByPatientAsync")]
        public async Task<IActionResult> GetAllTreatmentsByPatientAsync(GetTreatmentsByPatientPostApi modelApi, ITreatmentService treatmentService)
        {
            try
            {
                var models = await treatmentService.GetAllTreatmentsByPatientAsync(new GetTreatmentsByPatientPost
                {
                    PatientId = modelApi.PatientId,
                    State = modelApi.State,
                    ArticleFamily = modelApi.ArticleFamily,
                    Page = modelApi.Page,
                    PageSize = modelApi.PageSize,
                });
                return Ok(models.Select(x => new GetTreatmentsByPatientResponseApi()
                {
                    Id = x.Id,
                    TreatmentType= x.TreatmentType,
                    AppliedArticleId= x.AppliedArticleId,
                    ArticleType= x.ArticleType,
                    DateEnd= x.DateEnd,
                    DateStart= x.DateStart,
                    DosageDescription= x.DosageDescription,
                    PatientId= x.PatientId,
                    PrescribedArticleId = x.PrescribedArticleId 
                }));
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }


    }
}
