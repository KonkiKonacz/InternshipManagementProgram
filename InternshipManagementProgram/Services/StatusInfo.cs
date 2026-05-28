namespace InternshipManagementProgram.Services;

/// <summary>
/// Pomocnicze mapowanie statusow praktyki z bazy na klasy CSS chipow, kolory i
/// etykiety w UI. Zawiera tez generatory inicjalow i gradientu "logo" firmy.
/// Statusy w DB: "Zgloszona", "Zatwierdzona", "Odrzucona", "Zakonczona", "Zaliczona", "Niezaliczona".
/// </summary>
public static class StatusInfo
{
    public static string ChipClass(string? status) => status switch
    {
        "Zaliczona" => "chip chip-success",
        "Niezaliczona" => "chip chip-error",
        "Zatwierdzona" => "chip chip-info",
        "Zakończona" => "chip chip-info",
        "Zgłoszona" => "chip chip-warning",
        "Odrzucona" => "chip chip-neutral",
        _ => "chip chip-neutral"
    };

    public static string DotColor(string? status) => status switch
    {
        "Zaliczona" => "var(--success)",
        "Niezaliczona" => "var(--error)",
        "Zatwierdzona" => "var(--info)",
        "Zakończona" => "var(--info)",
        "Zgłoszona" => "var(--warning)",
        "Odrzucona" => "var(--neutral)",
        _ => "var(--neutral)"
    };

    // 2-literowy skrot z nazwy firmy (do logo w komorce tabeli).
    public static string Initials(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) return "?";
        var parts = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length == 1)
            return parts[0].Substring(0, Math.Min(2, parts[0].Length)).ToUpperInvariant();
        return ((parts[0][0].ToString() + parts[1][0].ToString())).ToUpperInvariant();
    }

    // Deterministyczny gradient na podstawie nazwy (do "logo" firmy w tabeli).
    public static string GradientFor(string name)
    {
        if (string.IsNullOrEmpty(name)) name = "??";
        var hash = 0;
        foreach (var c in name) hash = unchecked(hash * 31 + c);
        var h1 = (hash & 0xFF) % 360;
        var h2 = (h1 + 40) % 360;
        return $"linear-gradient(135deg, hsl({h1},55%,45%), hsl({h2},60%,30%))";
    }
}
