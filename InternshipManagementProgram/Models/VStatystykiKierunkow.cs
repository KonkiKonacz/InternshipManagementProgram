using System;
using System.Collections.Generic;

namespace InternshipManagementProgram.Models;

public partial class VStatystykiKierunkow
{
    public string NazwaKierunku { get; set; } = null!;

    public int Semestr { get; set; }

    public int? LiczbaStudentow { get; set; }

    public int? LiczbaZaliczonych { get; set; }

    public decimal? ProcentZdawalnosci { get; set; }
}
