﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace FwaEu.ElderAid.Protections.Services
{
    public interface IProtectionService
    {
        Task CreateProtectionAsync(CreateProtectionModel model);
        Task UpdateProtectionAsync(UpdateProtectionModel model);
        Task StopProtectionAsync(StopProtectionModel model);
    }
}