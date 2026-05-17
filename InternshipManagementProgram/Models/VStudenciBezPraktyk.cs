using System;
using System.Collections.Generic;

namespace InternshipManagementProgram.Models;

public partial class VStudenciBezPraktyk
{
    public string NrAlbumu { get; set; } = null!;

    public string Imie { get; set; } = null!;

    public string Nazwisko { get; set; } = null!;

    public int Semestr { get; set; }

    public string NazwaKierunku { get; set; } = null!;

    public string NazwaWydzialu { get; set; } = null!;
}
