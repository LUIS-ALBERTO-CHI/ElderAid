using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Tracking;
using FwaEu.Fwamework.Users;
using FwaEu.MediCare.ViewContext;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.MediCare.Users
{
	public class ApplicationUserEntity : UserEntity
		, IPerson
		, ICreationAndUpdateTracked
		, IApplicationPartEntityPropertiesAccessor
	{
		//NOTE: Sample of project-specific property
		//public string DogName { get; set; }

		public string FirstName { get; set; }
		public string LastName { get; set; }

		//private string _email;
		public string Email { get; set; }
		//public string Email
		//{
		//	get { return _email; }
		//	set
		//	{
		//		_email = value;
		//		Identity = value;
		//	}
		//}
		public string Login { get; set; }
		public UserEntity CreatedBy { get; set; }
		public DateTime CreatedOn { get; set; }
		public UserEntity UpdatedBy { get; set; }
		public DateTime UpdatedOn { get; set; }


		public override string ToString()
		{
			return this.ToFullNameString();
		}
	}

	public class ApplicationUserEntityRepository : DefaultUserEntityRepository<ApplicationUserEntity>, IQueryByIds<ApplicationUserEntity, int>
	{
		public IQueryable<ApplicationUserEntity> QueryByIds(int[] ids)
		{
			return this.Query().Where(entity => ids.Contains(entity.Id));
		}

		//public override IQueryable<ApplicationUserEntity> QueryForUsersAdmin()
		//{
		//	var contextService = this.ServiceProvider.GetRequiredService<IViewContextService>();
		//	var currentUserService = this.ServiceProvider.GetRequiredService<ICurrentUserService>();

		//	if (currentUserService.User == null || currentUserService.User.Entity.IsAdmin)
		//	{
		//		return this.Query();
		//	}

		//	return this.Query().Where(user => user == currentUserService.User.Entity
		//		|| !user.IsAdmin);
		//}
	}

	/// <summary>
	/// Base class map that you can override to change constraints on base properties (Identity...)
	/// </summary>
	public abstract class ApplicationUserEntityClassMap<TUserEntity> : UserEntityClassMap<TUserEntity>
		where TUserEntity : UserEntity
	{

	}

	/// <summary>
	/// Register the class map for UserEntity, using the common overrides from ApplicationUserEntityClassMap<>,
	/// used for UserEntity and also ApplicationUserEntity.
	/// </summary>
	public class UserEntityClassMap : ApplicationUserEntityClassMap<UserEntity>
	{
	}

	/// <summary>
	/// Register the class map for ApplicationUserEntity, which include application-specific properties.
	/// </summary>
	public class ApplicationUserEntityClassMap : ApplicationUserEntityClassMap<ApplicationUserEntity>
	{
		/// <summary>
		/// This name will be shared with UserEntity.Identity, this identity property is used for login/password authentication
		/// </summary>
		public const string IdentityColumnName = "login";

		public ApplicationUserEntityClassMap()
		{
			Map(entity => entity.Login)
				.Column(IdentityColumnName)
				.Not.Nullable()
				.Unique();

			Map(entity => entity.FirstName)
				.Not.Nullable();

			Map(entity => entity.LastName)
			   .Not.Nullable();
			Map(entity => entity.Email);

			this.AddCreationAndUpdateTrackedPropertiesIntoMapping();
		}
	}
}
