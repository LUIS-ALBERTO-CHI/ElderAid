using FwaEu.Fwamework.Setup;
using FwaEu.Modules.Authentication.Impersonate.Setup;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.MediCare.Users.AnonymousLogin.WebApi
{
	[Route("MediCare/[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		[HttpPost()]
		[ProducesResponseType(typeof(ImpersonateResultModel), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
		public async Task<ActionResult<ImpersonateResultModel>> Impersonate(ImpersonateModel model,
			[FromServices] IEnumerable<ISetupTask> tasks)
		{
			var task = tasks.OfType<ImpersonateTask>().First();
			var result = await task.ExecuteAsync(model);
			return Ok(result);
		}
	}
}
