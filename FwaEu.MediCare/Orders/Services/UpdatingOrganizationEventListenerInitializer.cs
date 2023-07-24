using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper.Internal;
using FwaEu.Fwamework.Data.Database.Nhibernate;
using FwaEu.Fwamework.DependencyInjection;
using NHibernate.Event;

namespace FwaEu.MediCare.Orders.Services
{
	public class UpdatingOrganizationEventListenerInitializer : IEventListenersInitializer
	{
		public void Initialize(EventListeners eventListeners, IScopedServiceProvider scopedServiceProvider)
		{
			var listener = Enumerable.Repeat(new UpdatingOrganizationEventListener(scopedServiceProvider), 1);

            eventListeners.PreInsertEventListeners = eventListeners.PreInsertEventListeners.Concat(listener).ToArray();
            eventListeners.PreUpdateEventListeners = eventListeners.PreUpdateEventListeners.Concat(listener).ToArray();

		}
	}
}