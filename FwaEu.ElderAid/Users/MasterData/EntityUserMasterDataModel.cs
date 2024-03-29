using FwaEu.Modules.Users.Avatars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.ElderAid.Users.MasterData
{
	public class EntityUserMasterDataModel : IAvatarUser
	{
		public EntityUserMasterDataModel(int id, string firstName, string lastName, string email, string login = null)
		{
			this.Id = id;
			this.FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
			this.LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
			this.Email = email ?? throw new ArgumentNullException(nameof(email));
			this.Login = email ?? throw new ArgumentNullException(nameof(login));
		}

		public int Id { get; }
		public string FirstName { get; }
		public string LastName { get; }
		public string Email { get; }
		public string Login { get; }
		public string AvatarUrl { get; set; }
	}
}
