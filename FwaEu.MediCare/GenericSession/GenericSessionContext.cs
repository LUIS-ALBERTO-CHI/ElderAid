using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.MediCare.ViewContext;
using Microsoft.Extensions.DependencyInjection;
using NHibernate;
using System;
using System.Data.Common;

namespace FwaEu.MediCare.GenericRepositorySession
{
    public class GenericSessionContext : BaseSessionContext<IStatefulSessionAdapter>
    {
        public ISession NhibernateSession => (ISession)RepositorySession.Session.InnerSession;
        public GenericSessionContext(
            IRepositorySessionFactory<IStatefulSessionAdapter> repositorySessionFactory,
            IServiceProvider serviceProvider)
            : base(repositorySessionFactory, serviceProvider)
        {
            var contextService = serviceProvider.GetRequiredService<IViewContextService>();
            var databaseName = contextService.Current?.DatabaseName;
            if(databaseName != null)
                NhibernateSession.Connection.ChangeDatabase(databaseName);
        }

        protected override CreateSessionOptions GetCreateSessionOptions()
        {
            return new CreateSessionOptions() { ConnectionStringName = "Generic" };
        }
    }
}
