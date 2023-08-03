﻿using JobPortal.Entities;

namespace JobPortal.Repositories
{
    public interface ILocationRepository
    {
        Task<Location> InsertLocation(string LocName);
        Task<Location> GetLocation(string LocName);
    }
}
