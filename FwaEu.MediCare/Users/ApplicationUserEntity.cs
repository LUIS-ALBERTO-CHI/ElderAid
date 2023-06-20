using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Tracking;
using FwaEu.Fwamework.Users;
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
		public string Email { get; set; }


		public UserEntity CreatedBy { get; set; }
		public DateTime CreatedOn { get; set; }
		public UserEntity UpdatedBy { get; set; }
		public DateTime UpdatedOn { get; set; }

        public string Login { get; set; }

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
		public const string EmailColumnName = "email";

		public ApplicationUserEntityClassMap()
		{
			Map(entity => entity.Email)
				.Column(EmailColumnName)
				.Not.Nullable()
				.Unique();

			Map(entity => entity.FirstName)
				.Not.Nullable();

			Map(entity => entity.LastName)
			   .Not.Nullable();
			Map(entity => entity.Login);

            this.AddCreationAndUpdateTrackedPropertiesIntoMapping();
		}
	}
}
