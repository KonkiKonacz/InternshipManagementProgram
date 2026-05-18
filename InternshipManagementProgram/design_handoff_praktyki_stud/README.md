# Handoff: System zarządzania praktykami studenckimi (PraktykiStud)

## Overview
Aplikacja webowa dla uczelni do prowadzenia praktyk studenckich. Dwa panele oddzielone rolą:

- **Panel studenta** — przegląd własnych praktyk, szczegóły wybranej praktyki, formularz zgłoszenia nowej praktyki, pobieranie zaświadczenia po zaliczeniu.
- **Panel administratora** — pełna tabela praktyk wszystkich studentów z filtrami i inline edycją statusu/oceny + zakładka „Studenci” z dwoma podlistami: bez praktyk i z niezaliczonymi.

## About the Design Files
Pliki w katalogu `prototype/` to **referencje projektowe wykonane w HTML/CSS/React (Babel inline)** — pokazują docelowy wygląd, układ i zachowania, ale **nie są kodem produkcyjnym do skopiowania 1:1**. Zadaniem developera jest **odtworzenie tych projektów w istniejącym środowisku aplikacji** (np. Blazor / MudBlazor, ASP.NET, React, Vue, itp.) z wykorzystaniem ustalonych w projekcie wzorców, bibliotek komponentów i stylów. Jeśli stos nie istnieje, można wybrać framework najlepiej pasujący do reszty projektu.

Wzorzec wizualny inspirowany jest **MudBlazor** (karty, tabele, chipy, przyciski) — jeśli docelowy stos to Blazor/MudBlazor, większość elementów ma bezpośrednie odpowiedniki (`MudTable`, `MudCard`, `MudChip`, `MudButton`, `MudTextField`, `MudSelect`, `MudDialog`, `MudDrawer`).

## Fidelity
**High-fidelity.** Mockupy są w pełni doszlifowane — finalna paleta, typografia, odstępy, stany i mikrointerakcje. Developer powinien odtworzyć je pixel-perfect przy użyciu komponentów istniejącego systemu projektowego.

---

## Brand
- Logo: granatowy biret z żółtym frędzlem nad zieloną teczką (`prototype/assets/logo.png`, 1024×1024, transparentne tło).
- Wartości brandu: **granat** = zaufanie/instytucja, **zieleń** = sukces/CTA, **złoto** = wyróżnienie/zdobyty certyfikat.

## Design Tokens

### Kolory
| Token | Hex | Użycie |
|---|---|---|
| `--bg-app` | `#131c34` | Główne tło aplikacji (granat) |
| `--bg-surface` | `#1a2547` | Karty, tabele, drawer |
| `--bg-surface-2` | `#1f2c52` | Nagłówki tabel, paski filtrów |
| `--bg-elev` | `#263560` | Wyższa warstwa (dropdowny, toast, badge) |
| `--bg-input` | `#101831` | Tła inputów, kontrolek formularzy |
| `--bg-hover` | `rgba(255,255,255,0.04)` | Hover na elementach nawigacji |
| `--bg-row-hover` | `rgba(54,178,76,0.08)` | Hover na wierszach tabeli |
| `--border` | `rgba(255,255,255,0.07)` | Linie separujące |
| `--border-strong` | `rgba(255,255,255,0.14)` | Outline'y przycisków, focus |
| `--text-primary` | `rgba(255,255,255,0.94)` | Tekst główny |
| `--text-secondary` | `rgba(220,228,248,0.62)` | Etykiety, metadane |
| `--text-disabled` | `rgba(220,228,248,0.38)` | Disabled, hinty |
| **Primary (zielony brand)** | | |
| `--primary` | `#4ac255` | Hover / jaśniejszy wariant |
| `--primary-strong` | `#36b24c` | Główne CTA |
| `--primary-deep` | `#2a9a3e` | Wersja "pressed", gradienty |
| `--primary-soft` | `rgba(54,178,76,0.16)` | Tła "soft", aktywne nav-itemy |
| **Granat (brand navy)** | | |
| `--navy` | `#1f2f5e` | Akcent identyfikacyjny |
| `--navy-light` | `#2a3f7a` | Wariant jaśniejszy |
| **Złoto (brand gold)** | | |
| `--gold` | `#f0c419` | Akcent ceremonialny — „Pobierz zaświadczenie", warningi |
| `--gold-soft` | `rgba(240,196,25,0.18)` | Tła chip-warning |
| **Statusy semaforowe** | | |
| `--success` | `#4ac255` | Status „Zaliczona", pozytywne stany |
| `--success-soft` | `rgba(74,194,85,0.16)` | Chip „Zaliczona" |
| `--error` | `#f15463` | Status „Niezaliczona" |
| `--error-soft` | `rgba(241,84,99,0.16)` | Chip „Niezaliczona" |
| `--info` | `#4d8df6` | Status „W trakcie" |
| `--info-soft` | `rgba(77,141,246,0.18)` | Chip „W trakcie" |
| `--warning` | `#f0c419` | Status „Oczekująca" |
| `--warning-soft` | `rgba(240,196,25,0.18)` | Chip „Oczekująca" |
| `--neutral` | `#8a99c0` | Status „Odrzucona / Robocza" |
| `--neutral-soft` | `rgba(138,153,192,0.18)` | Chip neutralny |

### Typografia
- **Sans:** `Inter` (400, 500, 600, 700), fallback: `system-ui, -apple-system, "Segoe UI", Roboto, sans-serif`
- **Mono:** `JetBrains Mono` (400, 500) — do ID praktyk, klawiszy `⌘K`
- Font features: `"cv11", "ss01"`, letter-spacing `-0.005em`
- Skala:
  - Tytuł strony (`.page-title`): 24px / 700 / `-0.02em`
  - Tytuł karty (`.card-title`): 14.5px / 600
  - Etykieta sekcji (`.nav-section-label`, `.detail-title`): 10–11px / 600 / uppercase / `0.1em` letter-spacing
  - Body table: 13px / 400 (nagłówki 11.5px / 600 / uppercase)
  - Chipy statusów: 11.5px / 600
  - Stat value: 28px / 700 / `-0.02em` / `tabular-nums`

### Odstępy i kształty
- Spacing podstawowy: 4 / 8 / 10 / 12 / 14 / 16 / 18 / 20 / 22 / 28 px
- Padding contentu (`.content`): 28px na boki, 28px góra, 64px dół
- Sidebar: 248px szer. (zwija się do 72px przy `<= 900px`)
- Topbar: 60px wys., sticky, `backdrop-filter: blur(10px)`
- Border-radius: `--radius-sm: 6px` (przyciski/inputy), `--radius-md: 10px` (karty), `--radius-lg: 14px` (modale)
- Cieniowanie:
  - `--shadow-card: 0 1px 0 rgba(255,255,255,.03) inset, 0 8px 24px rgba(0,0,0,.28)`
  - `--shadow-elev: 0 12px 40px rgba(0,0,0,.45)` (modale, toasty)
  - CTA primary: `0 4px 14px rgba(54,178,76,.35)` (hover: 6px 18px)

---

## Screens / Views

### 1. Sidebar (wspólne)
**Cel:** stała nawigacja po lewej w obu panelach.

**Layout:**
- Sticky, pełna wysokość viewportu, 248px szer., `background: #0e1530` (granat głębszy niż canvas)
- Sekcje:
  1. **Brand** (60px wys., border-bottom): logo 34×34 + dwa wiersze tekstu ("PraktykiStud" 14px/600 + "v3.4 · Politechnika" 11px/400/uppercase/`0.04em`)
  2. **Nav** (flex:1, padding 14/10): pozycje z ikoną + etykietą, opcjonalnie liczbą po prawej. Nagłówki sekcji w stylu uppercase 10px.
  3. **Sidebar-foot** (border-top, padding 12): avatar 34×34, imię + meta, ikona „logout"
- Stany nav-item:
  - hover: tło `var(--bg-hover)`, tekst `--text-primary`
  - active: tło `var(--primary-soft)`, tekst `var(--primary)`, badge tło `var(--primary-soft-2)`
- Responsywność: poniżej 900px sidebar zwija się do 72px, etykiety i nagłówki sekcji znikają (zostają same ikony)

**Pozycje (student):**
- Pulpit · Moje praktyki (badge 5) · Nowe zgłoszenie · Dokumenty · Mój profil · Powiadomienia (badge 2) · Pomoc

**Pozycje (admin):**
- Pulpit · Praktyki (badge 48) · Studenci (badge 9) · Firmy · Semestry · Statystyki · Oceny · Ustawienia

### 2. Topbar (wspólne)
- 60px, sticky, granica dolna
- Breadcrumbs po lewej (separator `›`, ostatnia pozycja `<strong>`)
- Po prawej w kolejności: **segmented switch Student/Administrator** (chip-style, tło `var(--bg-input)`, aktywna pigułka `var(--bg-elev)`), vertical divider, search field (280px, ikona lupy, placeholder „Szukaj…", chip `⌘K`), ikona dzwonka z czerwoną kropką notyfikacji
- Switch ról jest komponentem demo — w produkcji ten widok powinien być wybierany przez routing/auth.

### 3. Student — Moje praktyki (`StudentPanel`)
**Cel:** student widzi swoje praktyki, klika wiersz → szczegóły po prawej; klika „Nowe zgłoszenie" → modal formularza.

**Layout:**
- Page head: tytuł 24/700 „Moje praktyki" + podtytuł („Aktualny semestr: **2025/26 letni**"), po prawej dwa przyciski („Regulamin" outlined, „Nowe zgłoszenie" primary z ikoną `+`)
- 4 kafelki KPI w grid `repeat(4, 1fr)`, gap 14px (Zaliczone / W trakcie / Oczekuje / Zrealizowane godziny). Każdy ma: kolorową kropkę 8px + label 12/500, value 28/700, trend 11.5px (z kolorowanym wzrostem)
- Split layout 2 kolumny: `1fr / 420px`, gap 18px (≤1200px zwija do jednej kolumny)

**Lewa kolumna — tabela praktyk:**
- Kolumny: **Firma** · **Rodzaj praktyki** · **Status** · **Daty** · **Postęp** (pasek + %) · chevron
- Komórka firmy: kwadratowe „logo" 32×32 z gradientem branżowym + 2-literowy skrót, obok nazwa (600) i ID praktyki w `t-meta`
- Wiersz hover: tło `var(--bg-row-hover)` (zielonkawe), cursor pointer
- Wiersz wybrany: tło `var(--primary-soft)`
- Pasek postępu: 6px wys., kolor zależny od statusu (success/info/error/neutral), `transition: width .3s ease`

**Prawa kolumna — `DetailsPanel`:**
- Header drawera: logo firmy 38×38, nazwa, ID + branża; po prawej ikona „więcej"
- Body: chip statusu + rodzaj, opcjonalnie ocena po prawej (kolor success/error)
- Sekcja „Informacje" (`<dl class="kv">` 130px / 1fr): Opiekun, Rodzaj, Okres, Wymiar, Semestr
- Sekcja „Etapy realizacji" — timeline z pionową linią po lewej, kropki: done (zielona pełna), active (primary z aurą `box-shadow: 0 0 0 4px primary-soft`), pending (szara)
- Sekcja „Dokumenty" — wiersze z ikoną pliku, nazwą, rozmiarem, mini-przyciskiem download
- Stopka: główny przycisk **„Pobierz zaświadczenie"** zajmuje flex:1
  - **Tylko gdy status === „Zaliczona"** → przycisk `btn-success` (złoty `#f0c419`, ciemny tekst, cień złoty)
  - W przeciwnym razie → `btn-outlined disabled`, opacity 0.5, tooltip „Dostępne po statusie „Zaliczona""
  - Pod nim info-banner z `--info-soft` z wyjaśnieniem reguły

### 4. Student — Nowa praktyka (modal)
**Cel:** zgłoszenie nowej praktyki do akceptacji.

**Layout:**
- Backdrop: pełny ekran, `rgba(8,8,16,0.65)` + `backdrop-filter: blur(6px)`, fade-in
- Modal 720px max-szer., border-radius 14px, `shadow-elev`, animacja `modalIn` (translateY+scale)
- Head: tytuł + krótki opis + ikona zamknięcia
- Body: grid 2 kolumny (`form-grid`), gap 18px. Pola oznaczone `*` mają czerwoną gwiazdkę.
- **Pola wymagane** (`required`): Firma (`full`, datalist z firmami partnerskimi), Opiekun, Rodzaj (select), Data rozpoczęcia, Data zakończenia
- Pola opcjonalne: E-mail opiekuna, Wymiar (h, default 240), Opis (`textarea`, `full`)
- Footer: „Anuluj" (text) · „Zapisz jako roboczą" (outlined) · „Wyślij do akceptacji" (primary, disabled dopóki wymagane puste)
- Po wysłaniu → toast „Zgłoszenie PR-XXXX zostało wysłane do akceptacji."

### 5. Admin — Praktyki (`AdminPractices`)
**Cel:** zarządzanie wszystkimi praktykami, inline edycja statusu i oceny.

**Layout:**
- Page head: tytuł + podtytuł z liczbą praktyk i ile oczekuje akceptacji. Po prawej „Eksport raportu" (outlined) + „Dodaj praktykę" (primary).
- 4 kafelki KPI (z opcją `pulse` dla "Do akceptacji" — kropka z ringiem)
- Karta z dwoma częściami:
  1. **Pasek filtrów** (`.filters`, tło `--bg-surface-2`, border-bottom): pięć filtrów (Status select, Student search-field, Firma select, Semestr select, Rodzaj select), filter-spacer (flex:1), opcjonalny „Wyczyść filtry" gdy któryś aktywny. Etykiety filtrów: 10.5/600/uppercase/`0.08em`. Inputy: focus → border `var(--primary)` + ring `0 0 0 3px var(--primary-soft)`.
  2. **Tabela** (`AdminRow`): ID/Student (avatar 30×30 + imię + nr albumu) · Firma · Rodzaj · Daty · Semestr · Status (`StatusSelect`) · Ocena (`GradeInput`) · `…`
  3. **Pasek paginacji** (border-top): „Wyświetlono X z Y praktyk" + przyciski stronicowania

**`StatusSelect` (inline):**
- Wygląda jak chip statusu — kolor zależy od bieżącej wartości (`chip-success` / `chip-info` / itd.)
- W środku natywny `<select>` z transparentnym tłem, dziedziczonym kolorem, własną strzałką w prawym rogu (SVG `data-uri`)
- Opcje pokrywają wszystkie 6 statusów. Zmiana wartości → toast „Zaktualizowano status… → …" + jeśli nowo Niezaliczona/Zaliczona i ocena pusta, autouzupełnia 2.0/4.0 (do nadpisania).

**`GradeInput` (inline):**
- `<input type="number" min="2" max="5" step="0.5">`, 70px szer., text-align center, `tabular-nums`, font-weight 700
- Kolor wartości: ≥3 → success, <3 → error, puste → text-disabled
- Disabled, gdy status to „Oczekująca" lub „Robocza" (opacity 0.4)
- Focus: border primary + ring soft

**Pusty stan filtrów:** ikona lupy 28px + „Brak wyników" + sugestia wyczyszczenia filtrów.

### 6. Admin — Studenci (`AdminStudents`)
**Cel:** lista studentów wymagających interwencji.

**Layout:**
- Page head + dwie akcje (Wyślij przypomnienie, Eksport)
- **Tabs** (`.tabs`): underline 2px, aktywna w kolorze `var(--primary)`. Każda zakładka ma badge-count.
  - **Studenci bez praktyk** (`NoPracticeView`): kolumny Student · Kierunek · Wymagany rodzaj (chip warning) · Termin zgłoszenia · Kontakt · Akcje („Przypomnij" text + „Szczegóły" outlined)
  - **Z niezaliczonymi praktykami** (`FailedView`): Student · Kierunek · Liczba prób (pigułka, ≥2 = error, 1 = warning) · Ostatnia firma · Niezaliczona (data) · Ocena (red lub chip „Odrzucona") · Akcje („Otwórz" / „Wyznacz termin")
- Avatary w obu widokach mają charakterystyczne gradienty (warning żółty / error czerwony) — komunikacja "to wymaga uwagi".

---

## Interactions & Behavior

### Globalne
- Wszystkie elementy interaktywne mają `transition: all .12s ease` na hover/focus
- Inputy/selecty: focus border `var(--primary)` + ring `0 0 0 3px var(--primary-soft)`
- Wiersze tabel są klikalne (selekcja w student), w admin pojedyncze komórki (StatusSelect/GradeInput) zatrzymują propagację, by nie trigerować onSelectRow
- Toast: dolny-prawy róg, 2.2–2.6s, fade-out automatyczny, ikona ✓ w zielonej pigułce

### Panel switch (`window.dispatchEvent('panelSwitch', detail: 'student'|'admin')`)
- W produkcji zastąp natywnym routingiem opartym na roli użytkownika (np. `/student` vs `/admin`). Switch demo zachowuje stan w `localStorage('ps.panel')`.

### Tabela admin — inline editing
1. Klik w `StatusSelect` → dropdown natywny, wybór nowej wartości
2. `onStatus(id, newStatus)`:
   - Aktualizuje rekord
   - Jeśli nowy status to „Niezaliczona" a ocena pusta → automatycznie ustaw 2.0
   - Jeśli „Zaliczona" a ocena pusta → 4.0
   - Pokazuje toast
3. Klik w `GradeInput`, wpisanie wartości:
   - Puste → `null` (ocena = "—")
   - Wartość poza zakresem 2–5 i krokiem 0.5 jest ograniczona przez `min/max/step`
   - Brak osobnego "zapisz" — autoupdate na blur (w tym prototypie na każdy `onChange`)

### Filtry admin
- Filtry łączą się logicznym AND
- Search po studencie szuka w `student.name` (case-insensitive) **lub** `studentId`
- „Wyczyść filtry" pokazuje się tylko gdy jakikolwiek aktywny

### Modal nowego zgłoszenia
- Klik w backdrop zamyka; klik wewnątrz modala → `stopPropagation`
- Walidacja: `valid = company && supervisor && from && to` — przycisk „Wyślij" disabled + opacity 0.5 gdy nieprawidłowe
- Po `onSubmit`: tworzony nowy rekord ze statusem `Oczekujaca`, zostaje wybrany w tabeli, modal się zamyka, toast

---

## State Management

### `StudentPanel`
```
section: 'practices' | 'new' | 'dashboard' | …
practices: Practice[]   // local, w prod fetchowane z API
selectedId: string
showModal: boolean
toast: string | null
```

### `AdminPanel` → `AdminPractices`
```
data: AdminPractice[]
filters: { status, student, company, semester, type }   // all = brak filtra
toast: string | null
```

### `AdminStudents`
```
tab: 'noPractice' | 'failed'
```

### Modele
```ts
type Status = 'Zaliczona' | 'Niezaliczona' | 'WTrakcie' | 'Oczekujaca' | 'Odrzucona' | 'Robocza';
type Type = 'Obowiązkowa' | 'Nadobowiązkowa' | 'Wakacyjna' | 'Dyplomowa';

interface Company { name: string; short: string; domain: string; color: string; }

interface Practice {
  id: string;            // "PR-2026-0142"
  company: Company;
  type: Type;
  status: Status;
  dateFrom: string;      // ISO
  dateTo: string;        // ISO
  hours: number;
  hoursDone: number;
  supervisor: string;
  semester: string;      // "2025/26 letni"
  grade: number | null;  // 2.0 | 2.5 | 3.0 | 3.5 | 4.0 | 4.5 | 5.0
  timeline?: { label: string; date: string; state: 'done' | 'active' | 'pending' }[];
}

interface AdminPractice extends Omit<Practice, 'hoursDone' | 'timeline'> {
  student: string;
  studentId: string;     // nr albumu
}
```

### API endpoints (sugerowane)
- `GET  /api/student/practices` — lista praktyk zalogowanego studenta
- `POST /api/student/practices` — nowe zgłoszenie
- `GET  /api/student/practices/:id/certificate` — pobiera PDF zaświadczenia (tylko status Zaliczona)
- `GET  /api/admin/practices?status=…&student=…&company=…&semester=…&type=…`
- `PATCH /api/admin/practices/:id` — `{ status?, grade? }`
- `GET  /api/admin/students/no-practice?semester=…`
- `GET  /api/admin/students/failed?semester=…`

---

## Mapping na MudBlazor (jeśli docelowy stos to Blazor)

| Element prototypu | MudBlazor |
|---|---|
| `.card` / `.card-head` / `.card-body` | `MudCard` + `MudCardHeader` + `MudCardContent` |
| `.btn-primary` / `.btn-outlined` / `.btn-text` | `MudButton Variant="Filled\|Outlined\|Text" Color="Primary"` |
| `.chip` (status) | `MudChip Variant="Filled" Color="Success\|Error\|Info\|Warning\|Default"` |
| `.t` (tabela) | `MudTable` z `RowEditPreview` (lub własny inline cell editor) |
| `StatusSelect` w wierszu | `MudSelect` w `RowEditingTemplate`, ale w prototypie editor jest zawsze widoczny — można użyć `MudSelect` z `Dense` + custom style |
| `GradeInput` | `MudNumericField T="double?" Min="2" Max="5" Step="0.5"` |
| `.tabs` | `MudTabs` |
| `.modal` (Nowe zgłoszenie) | `MudDialog` |
| Toast | `MudSnackbar` (`ISnackbar.Add(...)`) |
| Sidebar | `MudDrawer Open Variant="Persistent"` + `MudNavMenu` / `MudNavLink` |
| Topbar | `MudAppBar` |
| Datalist (firma) | `MudAutocomplete<Company>` |
| Timeline | `MudTimeline TimelinePosition="Start"` |

Paletę zdefiniuj w `MudThemeProvider` → `PaletteDark`:
```csharp
PaletteDark = new PaletteDark
{
    Primary = "#36b24c",
    Secondary = "#f0c419",
    Tertiary = "#1f2f5e",
    Success = "#4ac255",
    Error = "#f15463",
    Info = "#4d8df6",
    Warning = "#f0c419",
    Background = "#131c34",
    Surface = "#1a2547",
    DrawerBackground = "#0e1530",
    AppbarBackground = "#131c34",
    TextPrimary = "rgba(255,255,255,0.94)",
    TextSecondary = "rgba(220,228,248,0.62)",
    ActionDefault = "rgba(255,255,255,0.62)",
    LinesDefault = "rgba(255,255,255,0.07)",
    TableLines = "rgba(255,255,255,0.07)",
}
```

---

## Assets
- `prototype/assets/logo.png` — logo aplikacji (granat + zieleń + złoto, 1024×1024, transparentne tło). Wstawiane w sidebarze (34×34 z `object-fit: contain`).
- Ikony: zestaw inline SVG w `prototype/icons.jsx` (`<Icon name="…" />`) — odpowiedniki w MudBlazor: `MudIcon Icon="@Icons.Material.Filled.<…>"`.

## Files
Wszystkie pliki referencyjne są w katalogu `prototype/`:
- `PraktykiStud.html` — wejściowy plik HTML, ładuje React + Babel + pozostałe moduły
- `styles.css` — wszystkie tokeny CSS + style komponentów (~500 linii)
- `data.jsx` — przykładowe dane (statusy, typy, firmy, praktyki, studenci)
- `icons.jsx` — komponent `<Icon>` z lokalnymi SVG
- `shell.jsx` — wspólne komponenty (Sidebar*, Topbar, Toast, StatusChip, CompanyCell, helpery dat)
- `student-panel.jsx` — cały panel studenta (lista + szczegóły + modal nowego zgłoszenia)
- `admin-panel.jsx` — cały panel administratora (tabela z filtrami + Studenci/taby)
- `assets/logo.png` — logo aplikacji

Aby uruchomić referencję lokalnie, otwórz `PraktykiStud.html` w przeglądarce.
