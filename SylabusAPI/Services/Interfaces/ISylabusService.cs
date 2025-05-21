﻿using SylabusAPI.DTOs;

namespace SylabusAPI.Services.Interfaces
{
    public interface ISylabusService
    {
        Task<IEnumerable<SylabusDto>> GetByPrzedmiotAsync(int przedmiotId);
        Task<SylabusDto?> GetByIdAsync(int id);
        Task<SylabusDto> CreateAsync(SylabusDto dto);
        Task UpdateAsync(int id, SylabusDto dto);
        Task DeleteAsync(int id);
    }
}