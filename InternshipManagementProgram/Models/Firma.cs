using System;
using System.Collections.Generic;

namespace InternshipManagementProgram.Models;

public partial class Firma
{
    public int Idfirmy { get; set; }

    public string NazwaFirmy { get; set; } = null!;

    public string Adres { get; set; } = null!;

    public string KodPocztowy { get; set; } = null!;

    public string Miasto { get; set; } = null!;

    public string? Telefon { get; set; }

    public string? Email { get; set; }

    public virtual ICollection<OsobaKontaktowa> OsobaKontaktowas { get; set; } = new List<OsobaKontaktowa>();

    public virtual ICollection<Praktyka> Praktykas { get; set; } = new List<Praktyka>();
}
