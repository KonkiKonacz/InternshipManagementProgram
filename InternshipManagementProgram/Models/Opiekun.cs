using System;
using System.Collections.Generic;

namespace InternshipManagementProgram.Models;

public partial class Opiekun
{
    public int Idopiekuna { get; set; }

    public string Imie { get; set; } = null!;

    public string Nazwisko { get; set; } = null!;

    public string? Tytul { get; set; }

    public string? Email { get; set; }

    public string? Telefon { get; set; }

    public virtual ICollection<Praktyka> Praktykas { get; set; } = new List<Praktyka>();
}
