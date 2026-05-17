using System;
using System.Collections.Generic;

namespace InternshipManagementProgram.Models;

public partial class VStudenciNiezaliczeni
{
    public string NrAlbumu { get; set; } = null!;

    public string Imie { get; set; } = null!;

    public string Nazwisko { get; set; } = null!;

    public int Semestr { get; set; }

    public string NazwaKierunku { get; set; } = null!;

    public int Idpraktyki { get; set; }

    public decimal? Ocena { get; set; }

    public string NazwaFirmy { get; set; } = null!;

    public DateOnly? DataRozpoczecia { get; set; }

    public DateOnly? DataZakonczenia { get; set; }
}
