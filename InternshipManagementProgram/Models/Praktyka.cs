using System;
using System.Collections.Generic;

namespace InternshipManagementProgram.Models;

public partial class Praktyka
{
    public int Idpraktyki { get; set; }

    public DateOnly DataZgloszenia { get; set; }

    public DateOnly? DataRozpoczecia { get; set; }

    public DateOnly? DataZakonczenia { get; set; }

    public string Status { get; set; } = null!;

    public decimal? Ocena { get; set; }

    public string RodzajPraktyki { get; set; } = null!;

    public string NrAlbumu { get; set; } = null!;

    public int Idfirmy { get; set; }

    public int Idopiekuna { get; set; }

    public int? Idosoby { get; set; }

    public virtual Firma IdfirmyNavigation { get; set; } = null!;

    public virtual Opiekun IdopiekunaNavigation { get; set; } = null!;

    public virtual OsobaKontaktowa? IdosobyNavigation { get; set; }

    public virtual Student NrAlbumuNavigation { get; set; } = null!;
}
