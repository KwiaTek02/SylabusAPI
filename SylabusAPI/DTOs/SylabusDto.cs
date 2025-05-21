using System.Text.Json.Nodes;

namespace SylabusAPI.DTOs
{
    public class SylabusDto
    {
        public int Id { get; set; }
        public int PrzedmiotId { get; set; }
        public string Wersja { get; set; } = default!;
        public DateTime? DataPowstania { get; set; }
        public int KtoStworzyl { get; set; }
        public JsonNode? TresciKsztalcenia { get; set; }
        public JsonNode? EfektyKsztalcenia { get; set; }
        public JsonNode? MetodyWeryfikacji { get; set; }
        public JsonNode? NakladPracy { get; set; }
        public JsonNode? Literatura { get; set; }
        public JsonNode? MetodyRealizacji { get; set; }
    }
}