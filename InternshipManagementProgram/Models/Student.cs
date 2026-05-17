using System;
using System.Collections.Generic;

namespace InternshipManagementProgram.Models;

public partial class Student
{
    public string NrAlbumu { get; set; } = null!;

    public string Imie { get; set; } = null!;

    public string Nazwisko { get; set; } = null!;

    public string? Email { get; set; }

    public string? Telefon { get; set; }

    public int Semestr { get; set; }

    public int Idkierunku { get; set; }

    public virtual Kierunek IdkierunkuNavigation { get; set; } = null!;

    public virtual ICollection<Praktyka> Praktykas { get; set; } = new List<Praktyka>();
}
