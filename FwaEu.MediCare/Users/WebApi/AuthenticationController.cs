using System.Threading.Tasks;
using FwaEu.Fwamework.Authentication;
using FwaEu.Modules.Authentication.JsonWebToken.Credentials;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FwaEu.MediCare.Users.WebApi
{

	[ApiController]
	[Route("[controller]")]
	public class AuthenticationController : ControllerBase
	{

		[AllowAnonymous]
		[HttpPost(nameof(AuthenticateWithDb))]
		[ProducesResponseType(typeof(AuthenticationResponseApiModel), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(void), StatusCodes.Status501NotImplemented)]
		[ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
		public async Task<IActionResult> AuthenticateWithDb(
			[FromBody] CredentialsWithDbApiModel model,
			[FromServices] IAuthenticationFeatures features,
			[FromServices] ICredentialsAuthenticateService credentialsAuthenticateService)
		{
			if (!features.EnableCredentialsAuthenticateAction)
			{
				return StatusCode(StatusCodes.Status501NotImplemented);
			}

			var user = await credentialsAuthenticateService.AuthenticateAsync(model.Identity, model.Password);
			if (user == null)
			{
				return Unauthorized("Invalid credentials.");
			}

			return Ok(new AuthenticationResponseApiModel
			{
				Token = user.Token,
			});
		}

	}
}