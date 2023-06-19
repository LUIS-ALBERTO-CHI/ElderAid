﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace FwaEu.MediCare.Referencials.Services
{
    public interface IBuildingService
    {
        Task<List<BuildingEntity>> GetAllAsync();
    }
}
