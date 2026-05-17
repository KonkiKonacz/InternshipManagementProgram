using System;
using System.Collections.Generic;

namespace InternshipManagementProgram.Models;

public partial class Kierunek
{
    public int Idkierunku { get; set; }

    public string NazwaKierunku { get; set; } = null!;

    public int Idwydzialu { get; set; }

    public virtual Wydzial IdwydzialuNavigation { get; set; } = null!;

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}
