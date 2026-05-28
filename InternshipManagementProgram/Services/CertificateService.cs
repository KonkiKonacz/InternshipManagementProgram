using InternshipManagementProgram.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace InternshipManagementProgram.Services;

/// <summary>
/// Generuje zaswiadczenie PDF o odbyciu praktyki studenckiej (QuestPDF).
/// Dane pobiera z widoku V_PraktykiPelne, ktory laczy informacje o studencie,
/// firmie, opiekunie i statusie praktyki.
/// </summary>
public class CertificateService
{
    /// <summary>
    /// Buduje dokument PDF na podstawie jednego rekordu praktyki i zwraca go
    /// jako tablice bajtow gotowa do pobrania w przegladarce.
    /// </summary>
    public byte[] GenerujZaswiadczenie(VPraktykiPelne p)
    {
        var dokument = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.DefaultTextStyle(t => t.FontSize(11).FontFamily("Arial"));

                page.Header().Column(col =>
                {
                    col.Item().AlignCenter().Text("ZAŚWIADCZENIE")
                        .FontSize(20).Bold();
                    col.Item().AlignCenter().Text("o odbyciu praktyki studenckiej")
                        .FontSize(13);
                });

                page.Content().PaddingVertical(20).Column(col =>
                {
                    col.Spacing(10);

                    col.Item().Text(text =>
                    {
                        text.Span("Niniejszym zaświadcza się, że student ");
                        text.Span($"{p.ImieStudenta} {p.NazwiskoStudenta}").Bold();
                        text.Span(", nr albumu ");
                        text.Span(p.NrAlbumu).Bold();
                        text.Span($", studiujący na kierunku „{p.NazwaKierunku}” na wydziale „{p.NazwaWydzialu}” " +
                                  $"(semestr {p.Semestr}), odbył praktykę studencką.");
                    });

                    col.Item().PaddingTop(10).Text("Szczegóły praktyki:").Bold();

                    col.Item().Table(t =>
                    {
                        t.ColumnsDefinition(c =>
                        {
                            c.RelativeColumn(2);
                            c.RelativeColumn(3);
                        });

                        void Row(string label, string value)
                        {
                            t.Cell().PaddingVertical(3).Text(label).SemiBold();
                            t.Cell().PaddingVertical(3).Text(value);
                        }

                        Row("Rodzaj praktyki:", p.RodzajPraktyki);
                        Row("Firma:", $"{p.NazwaFirmy}, {p.AdresFirmy}, {p.MiastoFirmy}");
                        Row("Opiekun:", $"{p.TytulOpiekuna} {p.ImieOpiekuna} {p.NazwiskoOpiekuna}".Trim());
                        Row("Data rozpoczęcia:", p.DataRozpoczecia?.ToString("yyyy-MM-dd") ?? "—");
                        Row("Data zakończenia:", p.DataZakonczenia?.ToString("yyyy-MM-dd") ?? "—");
                        Row("Status:", p.Status);
                        Row("Ocena końcowa:", p.Ocena?.ToString("0.0") ?? "—");
                    });

                    col.Item().PaddingTop(30).Text(
                        "Zaświadczenie wystawiono na podstawie danych z systemu PraktykiApp.");
                });

                page.Footer().AlignRight().Text(t =>
                {
                    t.Span("Wygenerowano: ");
                    t.Span(DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
                });
            });
        });

        return dokument.GeneratePdf();
    }
}
