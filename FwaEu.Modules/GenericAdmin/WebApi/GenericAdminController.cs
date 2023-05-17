using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using FwaEu.Fwamework.WebApi;
using FwaEu.Modules.Data.Database;

namespace FwaEu.Modules.GenericAdmin.WebApi
{
	[Authorize]
	[ApiController]
	[Route("[controller]")]
	public class GenericAdminController : ControllerBase
	{
		private static string CamelToPascal(string value)
		{
			return value.Substring(0, 1).ToUpper() + value.Substring(1);
		}

		private static object RecursiveCamelToPascalValue(object value)
		{
			if (value != null)
			{
				if (value is Dictionary<string, object>) //NOTE: Must be before ICollection test
				{
					return RecursiveCamelToPascal((Dictionary<string, object>)value);
				}

				if (value is ICollection) //NOTE: Not IEnumerable to avoid types like string
				{
					return ((ICollection)value)
						.Cast<object>()
						.Select(v => RecursiveCamelToPascalValue(v))
						.ToArray();
				}
			}

			return value;
		}

		private static Dictionary<string, object> RecursiveCamelToPascal(Dictionary<string, object> dictionary)
		{
			return dictionary.ToDictionary(
				kv => CamelToPascal(kv.Key),
				kv => RecursiveCamelToPascalValue(kv.Value));
		}

		private static Dictionary<string, object>[] RecursiveCamelToPascal(Dictionary<string, object>[] dictionaries)
		{
			return dictionaries.Select(RecursiveCamelToPascal).ToArray();
		}

		[HttpPost("GetConfiguration/{key}")]
		[ProducesResponseType(typeof(ConfigurationModel), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
		[ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
		public async Task<ActionResult<ConfigurationModel>> GetConfiguration(string key,
			[FromServices] IEnumerable<IGenericAdminModelConfiguration> configurations,
			[FromServices] IServiceProvider serviceProvider)
		{
			var configuration = configurations.FirstOrDefault(em => em.Key == key);

			if (configuration == null)
			{
				return KeyNotfound(key);
			}

			if (!await configuration.IsAccessibleAsync())
			{
				return Forbid("Configuration not accessible for current user.");
			}

			return Ok(await ConfigurationModel.FromConfigurationAsync(configuration, serviceProvider));
		}

		[HttpPost("Save/{configurationKey}")]
		[ProducesResponseType(typeof(SaveResultsModel), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
		[ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
		[ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
		public async Task<ActionResult<SaveResultsModel>> Save(string configurationKey, 
			[FromBody] SaveContextModel context,
			[FromServices] IEnumerable<IGenericAdminModelConfiguration> configurations,
			[FromServices] IModelValidationService validationService)
		{
			var configuration = configurations.FirstOrDefault(em => em.Key == configurationKey);

			if (configuration == null)
			{
				return KeyNotfound(configurationKey);
			}

			if (!await configuration.IsAccessibleAsync())
			{
				return Forbid("Configuration not accessible for current user.");
			}

			var models = (IEnumerable<object>)Newtonsoft.Json.JsonConvert.DeserializeObject(context.Models.ToString(),
				(configuration.ModelType == null ? typeof(Dictionary<string, object>) : configuration.ModelType).MakeArrayType());

			foreach (var model in models)
			{
				var validationErrors = validationService.Validate(model);
				if (validationErrors.Length != 0)
				{
					return BadRequest(validationErrors);
				}
			}

			if (configuration.ModelType == null)
			{
				models = RecursiveCamelToPascal(models.Cast<Dictionary<string, object>>().ToArray());
			}

			var saveResult = default(SaveResult);

			try
			{
				saveResult = await configuration.SaveAsync(models);
			}
			catch (DatabaseException ex)
			{
				return Conflict(ex.Type);
			}

			return Ok(SaveResultsModel.FromSaveResults(saveResult));
		}

		[HttpPost("Delete/{configurationKey}")]
		[ProducesResponseType(typeof(DeleteResultsModel), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
		[ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
		[ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
		public async Task<ActionResult<DeleteResultsModel>> Delete(string configurationKey, 
			[FromBody] DeleteContextModel context,
			[FromServices] IEnumerable<IGenericAdminModelConfiguration> configurations)
		{
			var configuration = configurations.FirstOrDefault(em => em.Key == configurationKey);

			if (configuration == null)
			{
				return KeyNotfound(configurationKey);
			}

			if (!await configuration.IsAccessibleAsync())
			{
				return Forbid("Configuration not accessible for current user.");
			}

			var camelDictionaries = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>[]>(context.Keys.ToString());

			var deleteResult = default(DeleteResult);
			try
			{
				deleteResult = await configuration.DeleteAsync(RecursiveCamelToPascal(camelDictionaries));
			}
			catch (DatabaseException ex)
			{
				return Conflict(ex.Type);
			}

			return Ok(DeleteResultsModel.FromDeleteResults(deleteResult));
		}


		private NotFoundObjectResult KeyNotfound(String key)
		{
			return NotFound("No generic admin registered with this key: " + key);
		}
	}
}