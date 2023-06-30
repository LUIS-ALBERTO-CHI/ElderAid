using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Fwamework.Users;
using NHibernate.Impl;

namespace FwaEu.MediCare.Users
{
    public class UserSynchronizationService : IUserSynchronizationService
    {
        private readonly MainSessionContext _sessionContext;

        public UserSynchronizationService(MainSessionContext sessionContext)
        {
            this._sessionContext = sessionContext
            ?? throw new ArgumentNullException(nameof(sessionContext));
        }

        public async Task SyncUserAsync(int userId)
        {
        var innerSession = (SessionImpl)(_sessionContext.RepositorySession.Session.InnerSession);
        await innerSession
        .CreateSQLQuery("EXEC SP_MDC_SyncUser :UserID")
        .SetInt32("UserID", userId)
        .ExecuteUpdateAsync();
        }
    }
}