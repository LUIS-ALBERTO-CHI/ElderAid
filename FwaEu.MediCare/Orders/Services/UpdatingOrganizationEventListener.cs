using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using FwaEu.Fwamework.DependencyInjection;
using NHibernate.Event;
using FwaEu.Fwamework;
using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.MediCare.Organizations;
using Microsoft.Extensions.Options;
using FwaEu.MediCare.Users;

namespace FwaEu.MediCare.Orders.Services
{
	public class UpdatingOrganizationEventListener : IPreInsertEventListener, IPreUpdateEventListener
	{
		public UpdatingOrganizationEventListener(IScopedServiceProvider scopedServiceProvider)
		{
			this._scopedServiceProvider = scopedServiceProvider
				?? throw new ArgumentNullException(nameof(scopedServiceProvider));
		}

		private readonly IScopedServiceProvider _scopedServiceProvider;

		private IServiceProvider GetServiceProviderAndCurrentUser()
		{
			var serviceProvider = this._scopedServiceProvider.GetScopeServiceProvider();

			if (serviceProvider == null)
			{
				return ApplicationServices.ServiceProvider;
			}

			return serviceProvider;
		}

		private void SetProperties(bool inserting, AbstractPreDatabaseOperationEvent @event, object[] state)
		{
			var entity = @event.Entity;
			if (entity is OrganizationEntity organization || entity is PeriodicOrderValidationEntity periodicOrderValidation)
			{
				var serviceProvider = this.GetServiceProviderAndCurrentUser();

				var mainSession = serviceProvider.GetService<MainSessionContext>();
                var robotEmail = serviceProvider.GetService<IOptions<PeriodicOrderOptions>>().Value.RobotEmail;  

                var userRobotEntity = mainSession.RepositorySession.Create<ApplicationUserEntityRepository>().Query().FirstOrDefault(x => x.Email.Equals(robotEmail, StringComparison.InvariantCultureIgnoreCase));

				if (entity is OrganizationEntity org && org.UpdatedBy is null)
				{
					org.UpdatedBy = userRobotEntity;
				}
				else if (entity is PeriodicOrderValidationEntity pov && pov.UpdatedBy is null)
				{
					pov.UpdatedBy = userRobotEntity;
				}
			}
		}

		public bool OnPreUpdate(PreUpdateEvent @event)
		{
			this.SetProperties(false, @event, @event.State);
			return false;
		}

		public Task<bool> OnPreUpdateAsync(PreUpdateEvent @event, CancellationToken cancellationToken)
		{
			this.SetProperties(false, @event, @event.State);
			return Task.FromResult(false);
		}

        public bool OnPreInsert(PreInsertEvent @event)
        {
            this.SetProperties(true, @event, @event.State);
            return false;
        }

        public Task<bool> OnPreInsertAsync(PreInsertEvent @event, CancellationToken cancellationToken)
        {
            this.SetProperties(true, @event, @event.State);
            return Task.FromResult(false);
        }
    }
}