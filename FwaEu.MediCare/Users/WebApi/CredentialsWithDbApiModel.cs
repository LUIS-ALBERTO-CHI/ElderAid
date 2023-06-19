using System.ComponentModel.DataAnnotations;

namespace FwaEu.MediCare.Users.WebApi
{
	public class CredentialsWithDbApiModel
	{
		[Required]
		public string Identity { get; set; }

		[Required]
		public string Password { get; set; }

        [Required]
        public string Database { get; set; }

    }
}
