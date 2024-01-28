using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.ElderAid.GenericSession;
using FwaEu.ElderAid.ViewContext;
using Microsoft.Extensions.DependencyInjection;
using NHibernate;
using System;
using System.Data.Common;

namespace FwaEu.ElderAid.GenericRepositorySession
{
    public class GenericSessionContext : BaseSessionContext<IStatefulSessionAdapter>
    {
        public ISession NhibernateSession => (ISession)RepositorySession.Session.InnerSession;
        public GenericSessionContext(
            IRepositorySessionFactory<IStatefulSessionAdapter> repositorySessionFactory,
            IServiceProvider serviceProvider)
            : base(repositorySessionFactory, serviceProvider)
        {
            var manageGenericDbService = serviceProvider.GetRequiredService<IManageGenericDbService>();
            var databaseName = manageGenericDbService.GetGenericDb();
            if (databaseName != null)
                NhibernateSession.Connection.ChangeDatabase(databaseName);
        }

        protected override CreateSessionOptions GetCreateSessionOptions()
        {
            return new CreateSessionOptions() { ConnectionStringName = "Generic" };
        }
    }
}
