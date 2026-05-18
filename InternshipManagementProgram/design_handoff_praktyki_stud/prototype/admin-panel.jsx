// Admin panel — practices table with filters + inline edits + Students tab

const { ADMIN_PRACTICES, STUDENTS_NO_PRACTICE, STUDENTS_FAILED } = window.AppData;

function AdminPanel() {
  const [section, setSection] = useState("practices");

  return (
    <div className="app">
      <SidebarAdmin section={section} onNav={setSection} />
      <main className="main">
        <Topbar
          crumbs={["Panel administratora", section === "students" ? "Studenci" : "Praktyki"]}
          panel="admin"
          onPanelSwitch={(p) => window.dispatchEvent(new CustomEvent("panelSwitch", { detail: p }))}
        />
        <div className="content">
          {section === "practices" && <AdminPractices />}
          {section === "students" && <AdminStudents />}
          {section !== "practices" && section !== "students" && (
            <PlaceholderSection label={
              section === "dashboard" ? "Pulpit administratora" :
              section === "companies" ? "Katalog firm" :
              section === "semesters" ? "Konfiguracja semestrów" :
              section === "stats" ? "Statystyki" :
              section === "grades" ? "Oceny" :
              section === "settings" ? "Ustawienia" : "Sekcja"
            } onJump={() => setSection("practices")} />
          )}
        </div>
      </main>
    </div>
  );
}

const PlaceholderSection = ({ label, onJump }) => (
  <div className="card" style={{padding: 60}}>
    <div className="empty">
      <Icon name="sparkle" size={28}/>
      <div className="em-title">{label}</div>
      <div className="em-sub">Sekcja w przygotowaniu. Przełącz się na zakładkę „Praktyki” lub „Studenci”, aby zobaczyć działający widok.</div>
      <button className="btn btn-outlined" style={{marginTop:14}} onClick={onJump}>
        <Icon name="briefcase" size={14}/>Przejdź do praktyk
      </button>
    </div>
  </div>
);

function AdminPractices() {
  const [data, setData] = useState(ADMIN_PRACTICES);
  const [filters, setFilters] = useState({
    status: "all", student: "", company: "all", semester: "all", type: "all",
  });
  const [toast, setToast] = useState(null);

  const triggerToast = (msg) => { setToast(msg); setTimeout(() => setToast(null), 2200); };

  const filtered = useMemo(() => data.filter(p => {
    if (filters.status !== "all" && p.status !== filters.status) return false;
    if (filters.company !== "all" && p.company.name !== filters.company) return false;
    if (filters.semester !== "all" && p.semester !== filters.semester) return false;
    if (filters.type !== "all" && p.type !== filters.type) return false;
    if (filters.student && !p.student.toLowerCase().includes(filters.student.toLowerCase()) && !p.studentId.includes(filters.student)) return false;
    return true;
  }), [data, filters]);

  const counts = useMemo(() => ({
    total:        data.length,
    wTrakcie:     data.filter(p => p.status === "WTrakcie").length,
    oczekujace:   data.filter(p => p.status === "Oczekujaca").length,
    zaliczone:    data.filter(p => p.status === "Zaliczona").length,
    niezaliczone: data.filter(p => p.status === "Niezaliczona").length,
  }), [data]);

  const setStatus = (id, status) => {
    setData(d => d.map(p => p.id === id ? { ...p, status, grade: status === "Niezaliczona" && p.grade == null ? 2.0 : status === "Zaliczona" && p.grade == null ? 4.0 : p.grade } : p));
    triggerToast(`Zaktualizowano status praktyki ${id} → ${window.AppData.STATUSES[status].label}`);
  };
  const setGrade = (id, grade) => {
    setData(d => d.map(p => p.id === id ? { ...p, grade } : p));
  };

  const resetFilters = () => setFilters({ status: "all", student: "", company: "all", semester: "all", type: "all" });
  const anyFilter = filters.status !== "all" || filters.student || filters.company !== "all" || filters.semester !== "all" || filters.type !== "all";

  return (
    <>
      <div className="page-head">
        <div>
          <h1 className="page-title">Wszystkie praktyki</h1>
          <p className="page-sub">{counts.total} praktyk w bazie. {counts.oczekujace} wymaga akceptacji.</p>
        </div>
        <div className="row">
          <button className="btn btn-outlined"><Icon name="download" size={14}/>Eksport raportu</button>
          <button className="btn btn-primary"><Icon name="plus" size={14}/>Dodaj praktykę</button>
        </div>
      </div>

      <div className="stats">
        <StatTile color="var(--info)"    label="W trakcie"     value={counts.wTrakcie}   trend="aktualnie realizowane" />
        <StatTile color="var(--warning)" label="Do akceptacji" value={counts.oczekujace} trend="średnio 3 dni rob." pulse />
        <StatTile color="var(--success)" label="Zaliczone"     value={counts.zaliczone}  trend="bieżący rok akadem." />
        <StatTile color="var(--error)"   label="Niezaliczone"  value={counts.niezaliczone} trend="wymagają interwencji" />
      </div>

      <div className="card">
        <div className="filters">
          <div className="filter">
            <span className="filter-label">Status</span>
            <select className="field" value={filters.status} onChange={(e) => setFilters({...filters, status: e.target.value})}>
              <option value="all">Wszystkie statusy</option>
              {Object.entries(window.AppData.STATUSES).map(([k,v]) => <option key={k} value={k}>{v.label}</option>)}
            </select>
          </div>
          <div className="filter">
            <span className="filter-label">Student</span>
            <div className="search-field" style={{minWidth: 220}}>
              <Icon name="search" size={13}/>
              <input placeholder="Imię, nazwisko lub nr albumu" value={filters.student} onChange={(e) => setFilters({...filters, student: e.target.value})}/>
            </div>
          </div>
          <div className="filter">
            <span className="filter-label">Firma</span>
            <select className="field" value={filters.company} onChange={(e) => setFilters({...filters, company: e.target.value})}>
              <option value="all">Wszystkie firmy</option>
              {COMPANIES.map(c => <option key={c.name}>{c.name}</option>)}
            </select>
          </div>
          <div className="filter">
            <span className="filter-label">Semestr</span>
            <select className="field" value={filters.semester} onChange={(e) => setFilters({...filters, semester: e.target.value})}>
              <option value="all">Wszystkie semestry</option>
              {SEMESTERS.map(s => <option key={s}>{s}</option>)}
            </select>
          </div>
          <div className="filter">
            <span className="filter-label">Rodzaj</span>
            <select className="field" value={filters.type} onChange={(e) => setFilters({...filters, type: e.target.value})}>
              <option value="all">Wszystkie</option>
              {TYPES.map(t => <option key={t}>{t}</option>)}
            </select>
          </div>
          <div className="filter-spacer"></div>
          {anyFilter && (
            <button className="btn btn-text btn-sm" onClick={resetFilters}>
              <Icon name="close" size={13}/>Wyczyść filtry
            </button>
          )}
        </div>

        <div className="table-wrap">
          <table className="t">
            <thead>
              <tr>
                <th>ID / Student</th>
                <th>Firma</th>
                <th>Rodzaj</th>
                <th>Daty</th>
                <th>Semestr</th>
                <th>Status</th>
                <th style={{textAlign:"center"}}>Ocena</th>
                <th></th>
              </tr>
            </thead>
            <tbody>
              {filtered.map(p => (
                <AdminRow
                  key={p.id}
                  p={p}
                  onStatus={(s) => setStatus(p.id, s)}
                  onGrade={(g) => setGrade(p.id, g)}
                />
              ))}
              {filtered.length === 0 && (
                <tr><td colSpan="8">
                  <div className="empty">
                    <Icon name="search" size={28}/>
                    <div className="em-title">Brak wyników</div>
                    <div className="em-sub">Spróbuj wyczyścić filtry lub zmień zapytanie.</div>
                  </div>
                </td></tr>
              )}
            </tbody>
          </table>
        </div>

        <div style={{padding:"12px 16px", borderTop:"1px solid var(--border)", display:"flex", alignItems:"center", justifyContent:"space-between", fontSize:12.5, color:"var(--text-secondary)"}}>
          <span>Wyświetlono <strong style={{color:"var(--text-primary)"}}>{filtered.length}</strong> z <strong style={{color:"var(--text-primary)"}}>{data.length}</strong> praktyk</span>
          <div className="row">
            <button className="btn btn-text btn-sm" disabled style={{opacity:0.4}}><Icon name="chevronLeft" size={13}/>Poprzednie</button>
            <span>1 / 1</span>
            <button className="btn btn-text btn-sm" disabled style={{opacity:0.4}}>Następne<Icon name="chevronRight" size={13}/></button>
          </div>
        </div>
      </div>

      {toast && <Toast>{toast}</Toast>}
    </>
  );
}

const StatTile = ({ color, label, value, trend, pulse }) => (
  <div className="stat">
    <div className="label"><span className="dot" style={{background: color, boxShadow: pulse ? `0 0 0 3px ${color}22` : "none"}}></span>{label}</div>
    <div className="value">{value}</div>
    <div className="trend">{trend}</div>
  </div>
);

function AdminRow({ p, onStatus, onGrade }) {
  const initials = p.student.split(" ").map(n => n[0]).slice(0,2).join("");
  return (
    <tr>
      <td>
        <div style={{display:"flex", alignItems:"center", gap:10}}>
          <div className="avatar" style={{width:30, height:30, fontSize:11}}>{initials}</div>
          <div>
            <div style={{fontWeight:600, fontSize:13}}>{p.student}</div>
            <div className="t-meta">{p.id} · nr {p.studentId}</div>
          </div>
        </div>
      </td>
      <td><CompanyCell company={p.company} /></td>
      <td><span className="t-type">{p.type}</span></td>
      <td className="t-dates">{fmtRange(p.dateFrom, p.dateTo)}</td>
      <td><span className="t-meta">{p.semester}</span></td>
      <td>
        <StatusSelect value={p.status} onChange={onStatus} />
      </td>
      <td style={{textAlign:"center"}}>
        <GradeInput value={p.grade} onChange={onGrade} status={p.status} />
      </td>
      <td style={{textAlign:"right"}}>
        <button className="icon-btn"><Icon name="moreHorizontal"/></button>
      </td>
    </tr>
  );
}

function StatusSelect({ value, onChange }) {
  const s = window.AppData.STATUSES[value];
  return (
    <div className={`chip ${s.chip}`} style={{padding: "0 6px 0 9px"}}>
      <span className="dot"></span>
      <select
        value={value}
        onChange={(e) => onChange(e.target.value)}
        className="cell-select"
        style={{color:"inherit", padding:"3px 18px 3px 4px", fontWeight:600, fontSize:11.5, letterSpacing:"0.01em",
                appearance:"none", WebkitAppearance:"none",
                backgroundImage:`url("data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' width='10' height='10' viewBox='0 0 24 24' fill='none' stroke='currentColor' stroke-width='2.5' stroke-linecap='round' stroke-linejoin='round'%3E%3Cpolyline points='6 9 12 15 18 9'/%3E%3C/svg%3E")`,
                backgroundRepeat:"no-repeat", backgroundPosition:"right 4px center"}}
        onClick={(e) => e.stopPropagation()}
      >
        {Object.entries(window.AppData.STATUSES).map(([k,v]) => <option key={k} value={k} style={{background:"var(--bg-elev)", color:"var(--text-primary)"}}>{v.label}</option>)}
      </select>
    </div>
  );
}

function GradeInput({ value, onChange, status }) {
  const [val, setVal] = useState(value ?? "");
  useEffect(() => setVal(value ?? ""), [value]);
  const disabled = status === "Oczekujaca" || status === "Robocza";
  const color = val === "" ? "var(--text-disabled)" :
                parseFloat(val) >= 3 ? "var(--success)" : "var(--error)";
  return (
    <input
      className="grade-input"
      type="number"
      min="2" max="5" step="0.5"
      placeholder="—"
      value={val}
      disabled={disabled}
      onChange={(e) => {
        const v = e.target.value;
        setVal(v);
        if (v === "") onChange(null);
        else onChange(parseFloat(v));
      }}
      style={{color, opacity: disabled ? 0.4 : 1}}
      onClick={(e) => e.stopPropagation()}
      title={disabled ? "Wprowadź ocenę po zmianie statusu" : "Edytuj ocenę inline"}
    />
  );
}

// ----- Students tab -----
function AdminStudents() {
  const [tab, setTab] = useState("noPractice");

  return (
    <>
      <div className="page-head">
        <div>
          <h1 className="page-title">Studenci</h1>
          <p className="page-sub">Lista studentów wymagających uwagi pełnomocnika ds. praktyk.</p>
        </div>
        <div className="row">
          <button className="btn btn-outlined"><Icon name="mail" size={14}/>Wyślij przypomnienie</button>
          <button className="btn btn-outlined"><Icon name="download" size={14}/>Eksport listy</button>
        </div>
      </div>

      <div className="tabs">
        <button className={`tab ${tab === "noPractice" ? "active" : ""}`} onClick={() => setTab("noPractice")}>
          <Icon name="users" size={14}/>Studenci bez praktyk
          <span className="badge-count">{STUDENTS_NO_PRACTICE.length}</span>
        </button>
        <button className={`tab ${tab === "failed" ? "active" : ""}`} onClick={() => setTab("failed")}>
          <Icon name="info" size={14}/>Z niezaliczonymi praktykami
          <span className="badge-count">{STUDENTS_FAILED.length}</span>
        </button>
      </div>

      {tab === "noPractice" && <NoPracticeView />}
      {tab === "failed"     && <FailedView />}
    </>
  );
}

const NoPracticeView = () => (
  <div className="card">
    <div className="card-head">
      <div>
        <h3 className="card-title">Studenci bez zarejestrowanej praktyki</h3>
        <div className="muted" style={{fontSize:12, marginTop:2}}>Semestr 2025/26 letni · obowiązkowe wg programu studiów</div>
      </div>
      <div className="row">
        <div className="search-field">
          <Icon name="search" size={13}/>
          <input placeholder="Szukaj studenta"/>
        </div>
      </div>
    </div>
    <div className="table-wrap">
      <table className="t">
        <thead>
          <tr>
            <th>Student</th>
            <th>Kierunek</th>
            <th>Wymagany rodzaj</th>
            <th>Termin zgłoszenia</th>
            <th>Kontakt</th>
            <th style={{textAlign:"right"}}>Akcja</th>
          </tr>
        </thead>
        <tbody>
          {STUDENTS_NO_PRACTICE.map(s => {
            const initials = s.name.split(" ").map(n => n[0]).slice(0,2).join("");
            return (
              <tr key={s.id}>
                <td>
                  <div style={{display:"flex", alignItems:"center", gap:10}}>
                    <div className="avatar" style={{width:32, height:32, fontSize:12, background:"linear-gradient(135deg,#f0c419,#c89c00)", color:"#2a1d00"}}>{initials}</div>
                    <div>
                      <div style={{fontWeight:600, fontSize:13}}>{s.name}</div>
                      <div className="t-meta">nr albumu {s.id}</div>
                    </div>
                  </div>
                </td>
                <td><span className="t-type">{s.program}</span></td>
                <td><span className="chip chip-warning"><span className="dot"></span>{s.reqType}</span></td>
                <td className="t-dates">do 31.03.2026</td>
                <td className="t-meta">{s.email}</td>
                <td style={{textAlign:"right"}}>
                  <button className="btn btn-text btn-sm"><Icon name="mail" size={13}/>Przypomnij</button>
                  <button className="btn btn-outlined btn-sm" style={{marginLeft:6}}>Szczegóły</button>
                </td>
              </tr>
            );
          })}
        </tbody>
      </table>
    </div>
  </div>
);

const FailedView = () => (
  <div className="card">
    <div className="card-head">
      <div>
        <h3 className="card-title">Studenci z niezaliczonymi praktykami</h3>
        <div className="muted" style={{fontSize:12, marginTop:2}}>Wymagana decyzja: powtórzenie / przedłużenie / skreślenie</div>
      </div>
    </div>
    <div className="table-wrap">
      <table className="t">
        <thead>
          <tr>
            <th>Student</th>
            <th>Kierunek</th>
            <th>Liczba prób</th>
            <th>Ostatnia firma</th>
            <th>Niezaliczona</th>
            <th>Ocena</th>
            <th style={{textAlign:"right"}}>Akcja</th>
          </tr>
        </thead>
        <tbody>
          {STUDENTS_FAILED.map(s => {
            const initials = s.name.split(" ").map(n => n[0]).slice(0,2).join("");
            return (
              <tr key={s.id}>
                <td>
                  <div style={{display:"flex", alignItems:"center", gap:10}}>
                    <div className="avatar" style={{width:32, height:32, fontSize:12, background:"linear-gradient(135deg,#f15463,#7d2532)", color:"#fff"}}>{initials}</div>
                    <div>
                      <div style={{fontWeight:600, fontSize:13}}>{s.name}</div>
                      <div className="t-meta">nr albumu {s.id}{s.note ? ` · ${s.note}` : ""}</div>
                    </div>
                  </div>
                </td>
                <td><span className="t-type">{s.program}</span></td>
                <td>
                  <span style={{
                    display:"inline-flex", alignItems:"center", justifyContent:"center",
                    minWidth:24, height:22, padding:"0 8px", borderRadius:6,
                    background: s.attempts >= 2 ? "var(--error-soft)" : "var(--warning-soft)",
                    color: s.attempts >= 2 ? "#ff9aa8" : "#ffd470",
                    fontSize:12, fontWeight:700,
                  }}>
                    {s.attempts}
                  </span>
                </td>
                <td>{s.lastCompany}</td>
                <td className="t-dates">{fmtDate(s.failedAt)}</td>
                <td>
                  {s.grade != null ? (
                    <span className="grade-display" style={{color:"var(--error)"}}>{fmtGrade(s.grade)}</span>
                  ) : (
                    <span className="chip chip-neutral"><span className="dot"></span>Odrzucona</span>
                  )}
                </td>
                <td style={{textAlign:"right"}}>
                  <button className="btn btn-text btn-sm"><Icon name="edit" size={13}/>Otwórz</button>
                  <button className="btn btn-outlined btn-sm" style={{marginLeft:6}}>Wyznacz termin</button>
                </td>
              </tr>
            );
          })}
        </tbody>
      </table>
    </div>
  </div>
);

window.AdminPanel = AdminPanel;
