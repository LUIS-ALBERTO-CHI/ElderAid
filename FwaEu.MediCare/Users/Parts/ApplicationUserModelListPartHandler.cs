using FwaEu.Fwamework.Users;
using FwaEu.Fwamework.Users.Parts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.MediCare.Users
{
	public class ApplicationUserModelListModel
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
        public string Login { get; set; }
    }

	public class ApplicationUserModelListPartHandler : ListPartHandler<ApplicationUserModelListModel>
	{
		public override string Name => "Application";

		public override Task<ApplicationUserModelListModel> LoadAsync(UserListModel model)
		{
			var accessor = (IApplicationPartLoadingListModelPropertiesAccessor)model;
			
			var part = new ApplicationUserModelListModel()
			{
				FirstName = accessor.FirstName,
				LastName = accessor.LastName,
				Email = accessor.Email,
                Login =  accessor.Login
            };

			return Task.FromResult(part);
		}
	}
}
