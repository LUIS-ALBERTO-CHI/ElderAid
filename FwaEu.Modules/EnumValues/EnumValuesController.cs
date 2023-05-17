using FwaEu.Fwamework.Temporal;
using FwaEu.Modules.MasterData.WebApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.EnumValues
{
	[Authorize]
	[ApiController]
	[Route("[controller]")]
	public class EnumValuesController : ControllerBase
	{
		private static DateTime? _dateTimeNow = null;

		public EnumValuesController(ICurrentDateTime currentDateTime)
		{
			if (!_dateTimeNow.HasValue)
			{
				_dateTimeNow = currentDateTime.Now;
			}
		}

		[HttpGet(nameof(Get))]
		public ActionResult<EnumValuesGetModelsResult> Get([FromServices] IEnumValuesService enumValuesProvider,
			[FromQuery] string enumTypeName)
		{
			try
			{
				return enumValuesProvider.GetEnumValues(enumTypeName);
			}
			catch (EnumNotFoundException ex)
			{
				return NotFound(ex.Message);
			}
		}

		[HttpGet(nameof(GetChangeInfos))]
		public ActionResult<MasterDataGetChangeInfosResponseModel> GetChangeInfos([FromServices] IEnumValuesService enumValuesProvider,
			[FromQuery] string enumTypeName)
		{
			try
			{
				return Ok(new[] { new MasterDataGetChangeInfosResponseModel
				{
					MasterDataKey = enumTypeName,
					Count = enumValuesProvider.GetEnumValues(enumTypeName).Values.Count(),
					MaximumUpdatedOn = _dateTimeNow,
				}});
			}
			catch (EnumNotFoundException ex)
			{
				return NotFound(ex.Message);
			}
		}
	}
}
