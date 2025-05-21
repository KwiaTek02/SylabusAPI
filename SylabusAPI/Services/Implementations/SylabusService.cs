using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Nodes;
using SylabusAPI.Data;
using SylabusAPI.DTOs;
using SylabusAPI.Services.Interfaces;
using SylabusAPI.Models;

namespace SylabusAPI.Services.Implementations
{
    public class SylabusService : ISylabusService
    {
        private readonly SyllabusContext _db;
        private readonly IMapper _mapper;

        public SylabusService(SyllabusContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SylabusDto>> GetByPrzedmiotAsync(int przedmiotId)
        {
            var list = await _db.sylabusies
                .Where(s => s.przedmiot_id == przedmiotId)
                .OrderBy(s => s.data_powstania)
                .ToListAsync();
            return list.Select(s => MapToDto(s));
        }

        public async Task<SylabusDto?> GetByIdAsync(int id)
        {
            var entity = await _db.sylabusies.FindAsync(id);
            return entity == null ? null : MapToDto(entity);
        }

        public async Task<SylabusDto> CreateAsync(SylabusDto dto)
        {
            var entity = new sylabusy
            {
                przedmiot_id = dto.PrzedmiotId,
                wersja = dto.Wersja,
                data_powstania = dto.DataPowstania ?? DateTime.UtcNow,
                kto_stworzyl = dto.KtoStworzyl,
                tresci_ksztalcenia_json = dto.TresciKsztalcenia?.ToJsonString(),
                efekty_ksztalcenia_json = dto.EfektyKsztalcenia?.ToJsonString(),
                metody_weryfikacji_json = dto.MetodyWeryfikacji?.ToJsonString(),
                naklad_pracy_json = dto.NakladPracy?.ToJsonString(),
                literatura_json = dto.Literatura?.ToJsonString(),
                metody_realizacji_json = dto.MetodyRealizacji?.ToJsonString()
            };

            _db.sylabusies.Add(entity);
            await _db.SaveChangesAsync();
            return MapToDto(entity);
        }

        public async Task UpdateAsync(int id, SylabusDto dto)
        {
            var entity = await _db.sylabusies.FindAsync(id);
            if (entity == null)
                throw new KeyNotFoundException("Sylabus nie został znaleziony.");

            entity.wersja = dto.Wersja;
            entity.data_powstania = dto.DataPowstania;
            entity.tresci_ksztalcenia_json = dto.TresciKsztalcenia?.ToJsonString();
            entity.efekty_ksztalcenia_json = dto.EfektyKsztalcenia?.ToJsonString();
            entity.metody_weryfikacji_json = dto.MetodyWeryfikacji?.ToJsonString();
            entity.naklad_pracy_json = dto.NakladPracy?.ToJsonString();
            entity.literatura_json = dto.Literatura?.ToJsonString();
            entity.metody_realizacji_json = dto.MetodyRealizacji?.ToJsonString();

            _db.sylabusies.Update(entity);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _db.sylabusies.FindAsync(id);
            if (entity == null)
                throw new KeyNotFoundException("Sylabus nie został znaleziony.");

            _db.sylabusies.Remove(entity);
            await _db.SaveChangesAsync();
        }

        private SylabusDto MapToDto(sylabusy s)
        {
            return new SylabusDto
            {
                Id = s.id,
                PrzedmiotId = s.przedmiot_id,
                Wersja = s.wersja,
                DataPowstania = s.data_powstania,
                KtoStworzyl = s.kto_stworzyl,
                TresciKsztalcenia = JsonNode.Parse(s.tresci_ksztalcenia_json ?? "{}"),
                EfektyKsztalcenia = JsonNode.Parse(s.efekty_ksztalcenia_json ?? "{}"),
                MetodyWeryfikacji = JsonNode.Parse(s.metody_weryfikacji_json ?? "{}"),
                NakladPracy = JsonNode.Parse(s.naklad_pracy_json ?? "{}"),
                Literatura = JsonNode.Parse(s.literatura_json ?? "{}"),
                MetodyRealizacji = JsonNode.Parse(s.metody_realizacji_json ?? "{}")
            };
        }
    }
}