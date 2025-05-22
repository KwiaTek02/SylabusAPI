﻿using System;

namespace SylabusAPI.DTOs
{
    public class SylabusHistoriaDto
    {
        public int Id { get; set; }
        public int SylabusId { get; set; }
        public DateOnly DataZmiany { get; set; }
        public DateTime CzasZmiany { get; set; }
        public int ZmienionyPrzez { get; set; }
        public string? OpisZmiany { get; set; }
        public string? WersjaWtedy { get; set; }
    }
}