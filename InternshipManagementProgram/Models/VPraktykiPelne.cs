using System;
using System.Collections.Generic;

namespace InternshipManagementProgram.Models;

public partial class VPraktykiPelne
{
    public int Idpraktyki { get; set; }

    public DateOnly DataZgloszenia { get; set; }

    public DateOnly? DataRozpoczecia { get; set; }

    public DateOnly? DataZakonczenia { get; set; }

    public string Status { get; set; } = null!;

    public decimal? Ocena { get; set; }

    public string RodzajPraktyki { get; set; } = null!;

    public string NrAlbumu { get; set; } = null!;

    public string ImieStudenta { get; set; } = null!;

    public string NazwiskoStudenta { get; set; } = null!;

    public string? EmailStudenta { get; set; }

    public int Semestr { get; set; }

    public string NazwaKierunku { get; set; } = null!;

    public string NazwaWydzialu { get; set; } = null!;

    public int Idfirmy { get; set; }

    public string NazwaFirmy { get; set; } = null!;

    public string AdresFirmy { get; set; } = null!;

    public string MiastoFirmy { get; set; } = null!;

    public int Idopiekuna { get; set; }

    public string ImieOpiekuna { get; set; } = null!;

    public string NazwiskoOpiekuna { get; set; } = null!;

    public string? TytulOpiekuna { get; set; }

    public int? Idosoby { get; set; }

    public string? ImieOsobyKontaktowej { get; set; }

    public string? NazwiskoOsobyKontaktowej { get; set; }

    public string? StanowiskoOsobyKontaktowej { get; set; }
}
