using System;
using System.Collections.Generic;

namespace InternshipManagementProgram.Models;

public partial class Wydzial
{
    public int Idwydzialu { get; set; }

    public string NazwaWydzialu { get; set; } = null!;

    public virtual ICollection<Kierunek> Kieruneks { get; set; } = new List<Kierunek>();
}
