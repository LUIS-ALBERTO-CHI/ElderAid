using FwaEu.Fwamework.Data.Database.Sessions;
using NHibernate;
using System;
using System.Data.Common;

namespace FwaEu.MediCare.GenericRepositorySession
{
    public class GenericRepositorySessionService : IGenericRepositorySessionService
    {
        private readonly MainSessionContext _sessionContext;
        private readonly IRepositorySessionFactory<IStatefulSessionAdapter> _repositorySessionFactory;
        public GenericRepositorySessionService(MainSessionContext sessionContext, 
                                                    IRepositorySessionFactory<IStatefulSessionAdapter> repositorySessionFactory) {
            _sessionContext = sessionContext;
            _repositorySessionFactory = repositorySessionFactory
                ?? throw new ArgumentNullException(nameof(repositorySessionFactory));
        }

        public RepositorySession<IStatefulSessionAdapter> GetRepositorySession()
        {
            var databaseName = "MEDICARE_EMS2";
            DbConnection connection = ((ISession)_sessionContext.RepositorySession.Session.InnerSession).Connection;
            connection.ChangeDatabase(databaseName);

            return this._repositorySessionFactory.CreateSession(new CreateSessionOptions
            {
                Connection = connection,
                ConnectionStringName = "Default",
            });
        }
    }
}
