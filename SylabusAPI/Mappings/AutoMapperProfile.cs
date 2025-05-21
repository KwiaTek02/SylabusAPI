using AutoMapper;
using SylabusAPI.Models;    // przestrzeń nazw wygenerowanych modeli EF
using SylabusAPI.DTOs;      // przestrzeń nazw Twoich DTO
using SylabusAPI.Data;

namespace SylabusAPI.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Tutaj definiujesz mapowanie
            CreateMap<przedmioty, PrzedmiotDto>()
                .ForMember(
                    dest => dest.SumaGodzinCalosciowe,
                    opt => opt.MapFrom(src => src.suma_godzin_calosciowe)
                );
            // (opcjonalnie odwrotne, jeśli będziesz mapować DTO na encję)
            CreateMap<PrzedmiotDto, przedmioty>();
        }
    }
}