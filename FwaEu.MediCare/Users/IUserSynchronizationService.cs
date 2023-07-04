using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.MediCare.Users
{
    public interface IUserSynchronizationService
    {
        Task  SyncUserAsync(int userId);
    }
}