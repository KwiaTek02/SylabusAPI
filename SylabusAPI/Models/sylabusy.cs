﻿using System;
using System.Collections.Generic;

namespace SylabusAPI.Models;

public partial class sylabusy
{
    public int id { get; set; }

    public int przedmiot_id { get; set; }

    public string wersja { get; set; } = null!;

    public string? nazwa_jednostki_organizacyjnej { get; set; }

    public string? profil_ksztalcenia { get; set; }

    public string? nazwa_specjalnosci { get; set; }

    public string? rodzaj_modulu_ksztalcenia { get; set; }

    public string? wymagania_wstepne { get; set; }

    public string? rok_data { get; set; }

    public DateTime? data_powstania { get; set; }

    public int kto_stworzyl { get; set; }

    public string? tresci_ksztalcenia_json { get; set; }

    public string? efekty_ksztalcenia_json { get; set; }

    public string? metody_weryfikacji_json { get; set; }

    public string? naklad_pracy_json { get; set; }

    public string? literatura_json { get; set; }

    public string? metody_realizacji_json { get; set; }

    public virtual ICollection<koordynatorzy_sylabusu> koordynatorzy_sylabusus { get; set; } = new List<koordynatorzy_sylabusu>();

    public virtual uzytkownicy kto_stworzylNavigation { get; set; } = null!;

    public virtual przedmioty przedmiot { get; set; } = null!;

    public virtual ICollection<sylabus_historium> sylabus_historia { get; set; } = new List<sylabus_historium>();
}
