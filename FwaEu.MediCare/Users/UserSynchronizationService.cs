using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Fwamework.Users;
using Microsoft.Extensions.Logging;
using NHibernate.Impl;

namespace FwaEu.MediCare.Users
{
    public class UserSynchronizationService : IUserSynchronizationService
    {
        private readonly MainSessionContext _sessionContext;
        private readonly ILogger _logger;

        public UserSynchronizationService(MainSessionContext sessionContext, ILoggerFactory loggerFactory)
        {
            this._sessionContext = sessionContext
            ?? throw new ArgumentNullException(nameof(sessionContext));

            this._logger = loggerFactory.CreateLogger<UserSynchronizationService>();

        }

        public async Task SyncUserAsync(int userId)
        {
            try
            {
                var innerSession = (SessionImpl)(_sessionContext.RepositorySession.Session.InnerSession);
                await innerSession
                .CreateSQLQuery("EXEC SP_MDC_SyncUser :UserID")
                .SetInt32("UserID", userId)
                .ExecuteUpdateAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error trying this operation");
            }
        }
    }
}