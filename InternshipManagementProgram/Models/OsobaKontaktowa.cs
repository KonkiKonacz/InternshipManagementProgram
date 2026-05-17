using System;
using System.Collections.Generic;

namespace InternshipManagementProgram.Models;

public partial class OsobaKontaktowa
{
    public int Idosoby { get; set; }

    public string Imie { get; set; } = null!;

    public string Nazwisko { get; set; } = null!;

    public string? Stanowisko { get; set; }

    public string? Telefon { get; set; }

    public string? Email { get; set; }

    public int Idfirmy { get; set; }

    public virtual Firma IdfirmyNavigation { get; set; } = null!;

    public virtual ICollection<Praktyka> Praktykas { get; set; } = new List<Praktyka>();
}
