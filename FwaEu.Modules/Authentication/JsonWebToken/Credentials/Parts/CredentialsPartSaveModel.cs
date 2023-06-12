using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.Authentication.JsonWebToken.Credentials.Parts
{
	public class CredentialsPartSaveModel
	{
		[Required]
		public string CurrentPassword { get; set; }

		[Required]
		public string NewPassword { get; set; }
	}
}
