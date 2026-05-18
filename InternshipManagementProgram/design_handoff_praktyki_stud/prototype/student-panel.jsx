// Student panel — list, details, new application form

const { STUDENT_PRACTICES: STUDENT_DATA } = window.AppData;

function StudentPanel() {
  const [section, setSection] = useState("practices");
  const [practices, setPractices] = useState(STUDENT_DATA);
  const [selectedId, setSelectedId] = useState(STUDENT_DATA[0].id);
  const [showModal, setShowModal] = useState(false);
  const [toast, setToast] = useState(null);

  const selected = practices.find(p => p.id === selectedId);

  const triggerToast = (msg) => {
    setToast(msg);
    setTimeout(() => setToast(null), 2600);
  };

  const handleNewSubmit = (vals) => {
    const company = window.AppData.COMPANIES.find(c => c.name === vals.company) || {
      name: vals.company, short: vals.company.slice(0, 2).toUpperCase(),
      domain: "—", color: "linear-gradient(135deg,#776be7,#4a3fbf)",
    };
    const next = {
      id: "PR-2026-0" + Math.floor(220 + Math.random() * 80),
      company, type: vals.type, status: "Oczekujaca",
      dateFrom: vals.from, dateTo: vals.to, hours: parseInt(vals.hours || "240", 10),
      hoursDone: 0, supervisor: vals.supervisor, semester: "2025/26 letni", grade: null,
    };
    setPractices([next, ...practices]);
    setSelectedId(next.id);
    setShowModal(false);
    triggerToast(`Zgłoszenie ${next.id} zostało wysłane do akceptacji.`);
  };

  return (
    <div className="app">
      <SidebarStudent section={section} onNav={setSection} />
      <main className="main">
        <Topbar
          crumbs={["Panel studenta", "Moje praktyki"]}
          panel="student"
          onPanelSwitch={(p) => window.dispatchEvent(new CustomEvent("panelSwitch", { detail: p }))}
        />
        <div className="content">
          <div className="page-head">
            <div>
              <h1 className="page-title">Moje praktyki</h1>
              <p className="page-sub">Zgłoszenia, status realizacji i zaświadczenia. Aktualny semestr: <strong>2025/26 letni</strong>.</p>
            </div>
            <div className="row">
              <button className="btn btn-outlined"><Icon name="file" size={14}/>Regulamin</button>
              <button className="btn btn-primary" onClick={() => setShowModal(true)}>
                <Icon name="plus" size={14}/>Nowe zgłoszenie
              </button>
            </div>
          </div>

          <StudentStats practices={practices} />

          <div className="split">
            <PracticesTable
              practices={practices}
              selectedId={selectedId}
              onSelect={setSelectedId}
            />
            <DetailsPanel
              practice={selected}
              onDownload={() => triggerToast(`Zaświadczenie ${selected.id} zostało pobrane.`)}
            />
          </div>
        </div>
      </main>

      {showModal && (
        <NewPracticeModal
          onClose={() => setShowModal(false)}
          onSubmit={handleNewSubmit}
        />
      )}
      {toast && <Toast>{toast}</Toast>}
    </div>
  );
}

const StudentStats = ({ practices }) => {
  const counts = {
    zaliczone: practices.filter(p => p.status === "Zaliczona").length,
    wTrakcie:  practices.filter(p => p.status === "WTrakcie").length,
    oczekujace:practices.filter(p => p.status === "Oczekujaca").length,
    godziny:   practices.reduce((s, p) => s + (p.hoursDone || 0), 0),
  };
  return (
    <div className="stats">
      <div className="stat">
        <div className="label"><span className="dot" style={{background:"var(--success)"}}></span>Zaliczone</div>
        <div className="value">{counts.zaliczone}</div>
        <div className="trend">Wymóg programu: <strong style={{ color: "var(--text-primary)", marginLeft: 4 }}>1 obowiązkowa</strong></div>
      </div>
      <div className="stat">
        <div className="label"><span className="dot" style={{background:"var(--info)"}}></span>W trakcie</div>
        <div className="value">{counts.wTrakcie}</div>
        <div className="trend">Najbliższy raport: <strong style={{ color: "var(--text-primary)", marginLeft: 4 }}>05.06.2026</strong></div>
      </div>
      <div className="stat">
        <div className="label"><span className="dot" style={{background:"var(--warning)"}}></span>Oczekuje na akceptację</div>
        <div className="value">{counts.oczekujace}</div>
        <div className="trend">Średni czas odpowiedzi: 3 dni rob.</div>
      </div>
      <div className="stat">
        <div className="label"><span className="dot" style={{background:"var(--primary)"}}></span>Zrealizowane godziny</div>
        <div className="value">{counts.godziny}<span style={{fontSize:14, color:"var(--text-secondary)", fontWeight:500, marginLeft:6}}>h</span></div>
        <div className="trend"><span className="up">+184h</span><span>w bieżącym semestrze</span></div>
      </div>
    </div>
  );
};

const PracticesTable = ({ practices, selectedId, onSelect }) => (
  <div className="card">
    <div className="card-head">
      <div>
        <h3 className="card-title">Lista praktyk</h3>
        <div className="muted" style={{fontSize:12, marginTop:2}}>{practices.length} wpisów · sortowane wg daty rozpoczęcia</div>
      </div>
      <div className="row">
        <button className="btn btn-text btn-sm"><Icon name="filter" size={13}/>Filtruj</button>
        <button className="btn btn-text btn-sm"><Icon name="download" size={13}/>Eksport CSV</button>
      </div>
    </div>
    <div className="table-wrap">
      <table className="t">
        <thead>
          <tr>
            <th>Firma</th>
            <th>Rodzaj praktyki</th>
            <th>Status</th>
            <th>Daty</th>
            <th style={{textAlign:"right"}}>Postęp</th>
            <th></th>
          </tr>
        </thead>
        <tbody>
          {practices.map(p => (
            <tr key={p.id} className={p.id === selectedId ? "selected" : ""} onClick={() => onSelect(p.id)}>
              <td><CompanyCell company={p.company} sub={p.id} /></td>
              <td><span className="t-type">{p.type}</span><div className="t-meta">{p.hours} h</div></td>
              <td><StatusChip value={p.status} /></td>
              <td className="t-dates">{fmtRange(p.dateFrom, p.dateTo)}</td>
              <td style={{textAlign:"right"}}>
                <ProgressBar value={p.hoursDone || 0} max={p.hours} status={p.status} />
              </td>
              <td style={{textAlign:"right"}}>
                <Icon name="chevronRight" size={16} />
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  </div>
);

const ProgressBar = ({ value, max, status }) => {
  const pct = Math.min(100, Math.round((value / max) * 100));
  let color = "var(--info)";
  if (status === "Zaliczona") color = "var(--success)";
  if (status === "Niezaliczona") color = "var(--error)";
  if (status === "Oczekujaca" || status === "Robocza") color = "var(--neutral)";
  return (
    <div style={{display:"inline-flex", alignItems:"center", gap:10, minWidth:160, justifyContent:"flex-end"}}>
      <div style={{flex:1, maxWidth:120, height:6, borderRadius:4, background:"var(--bg-input)", overflow:"hidden"}}>
        <div style={{width:`${pct}%`, height:"100%", background:color, transition:"width .3s ease"}}></div>
      </div>
      <span style={{fontVariantNumeric:"tabular-nums", fontSize:12, color:"var(--text-secondary)", minWidth:36, textAlign:"right"}}>{pct}%</span>
    </div>
  );
};

const DetailsPanel = ({ practice, onDownload }) => {
  if (!practice) return null;
  const isPassed = practice.status === "Zaliczona";
  return (
    <div className="drawer">
      <div className="drawer-head">
        <div className="row" style={{gap:12, minWidth:0}}>
          <div className="t-logo" style={{ background: practice.company.color, width:38, height:38, borderRadius:10, fontSize:13 }}>{practice.company.short}</div>
          <div style={{minWidth:0}}>
            <div style={{fontWeight:700, fontSize:14, whiteSpace:"nowrap", overflow:"hidden", textOverflow:"ellipsis"}}>{practice.company.name}</div>
            <div className="t-meta" style={{marginTop:2}}>{practice.id} · {practice.company.domain}</div>
          </div>
        </div>
        <button className="icon-btn" title="Więcej"><Icon name="moreHorizontal"/></button>
      </div>
      <div className="drawer-body">
        <div style={{display:"flex", alignItems:"center", gap:10, marginBottom:14}}>
          <StatusChip value={practice.status} />
          <span className="t-meta">· {practice.type}</span>
          {practice.grade != null && (
            <span style={{marginLeft:"auto"}}>
              <span className="t-meta" style={{marginRight:6}}>Ocena:</span>
              <span className="grade-display" style={{color: practice.grade >= 3 ? "var(--success)" : "var(--error)"}}>{fmtGrade(practice.grade)}</span>
            </span>
          )}
        </div>

        <div className="detail-section">
          <div className="detail-title">Informacje</div>
          <dl className="kv">
            <dt>Opiekun praktyki</dt><dd>{practice.supervisor}</dd>
            <dt>Rodzaj</dt><dd>{practice.type}</dd>
            <dt>Okres</dt><dd>{fmtRange(practice.dateFrom, practice.dateTo)}</dd>
            <dt>Wymiar</dt><dd>{practice.hours} godzin · {practice.hoursDone || 0} h zrealizowane</dd>
            <dt>Semestr</dt><dd>{practice.semester}</dd>
          </dl>
        </div>

        {practice.timeline && (
          <div className="detail-section">
            <div className="detail-title">Etapy realizacji</div>
            <div className="timeline">
              {practice.timeline.map((s, i) => (
                <div key={i} className={`tl-item ${s.state}`}>
                  <div className="tl-label">{s.label}</div>
                  <div className="tl-meta">{s.date}</div>
                </div>
              ))}
            </div>
          </div>
        )}

        <div className="detail-section">
          <div className="detail-title">Dokumenty</div>
          <div style={{display:"flex", flexDirection:"column", gap:6}}>
            <DocRow name="Skierowanie na praktykę.pdf"   size="124 KB" />
            <DocRow name="Program praktyki.pdf"           size="48 KB" />
            {isPassed && <DocRow name="Raport końcowy.pdf" size="312 KB" />}
          </div>
        </div>

        <div style={{display:"flex", gap:8, marginTop:18, paddingTop:14, borderTop:"1px solid var(--border)"}}>
          {isPassed ? (
            <button className="btn btn-success" style={{flex:1}} onClick={onDownload}>
              <Icon name="download" size={14}/>Pobierz zaświadczenie
            </button>
          ) : (
            <button className="btn btn-outlined" style={{flex:1}} disabled title="Dostępne po statusie „Zaliczona”">
              <Icon name="download" size={14}/>Pobierz zaświadczenie
            </button>
          )}
          <button className="btn btn-outlined"><Icon name="edit" size={14}/></button>
        </div>

        {!isPassed && (
          <div style={{
            marginTop: 12, padding: "10px 12px",
            background: "var(--info-soft)",
            border: "1px solid rgba(33,150,243,0.25)",
            borderRadius: 8, fontSize: 12, color: "#bcdcfc",
            display: "flex", alignItems: "flex-start", gap: 8,
          }}>
            <Icon name="info" size={14} />
            <span>Przycisk „Pobierz zaświadczenie” jest aktywny dopiero gdy status praktyki to <strong>Zaliczona</strong>.</span>
          </div>
        )}
      </div>
    </div>
  );
};

const DocRow = ({ name, size }) => (
  <div style={{display:"flex", alignItems:"center", gap:10, padding:"8px 10px", background:"var(--bg-input)", border:"1px solid var(--border)", borderRadius:8, fontSize:13}}>
    <Icon name="file" size={14} />
    <span style={{flex:1, whiteSpace:"nowrap", overflow:"hidden", textOverflow:"ellipsis"}}>{name}</span>
    <span className="t-meta">{size}</span>
    <button className="icon-btn" style={{width:26, height:26}}><Icon name="download" size={13}/></button>
  </div>
);

// New practice form
function NewPracticeModal({ onClose, onSubmit }) {
  const [form, setForm] = useState({
    company: "", supervisor: "", type: "Obowiązkowa",
    from: "", to: "", hours: "240", supervisorEmail: "", description: "",
  });
  const upd = (k, v) => setForm(f => ({ ...f, [k]: v }));
  const valid = form.company && form.supervisor && form.from && form.to;

  return (
    <div className="modal-backdrop" onClick={onClose}>
      <div className="modal" onClick={(e) => e.stopPropagation()} style={{maxWidth: 720}}>
        <div className="modal-head">
          <div>
            <h2 className="modal-title">Zgłoszenie nowej praktyki</h2>
            <div className="modal-sub">Wypełnij dane praktyki — po wysłaniu zgłoszenie trafia do pełnomocnika ds. praktyk.</div>
          </div>
          <button className="icon-btn" onClick={onClose}><Icon name="close" /></button>
        </div>
        <div className="modal-body">
          <div className="form-grid">
            <div className="form-field required full">
              <label>Firma / Instytucja</label>
              <input className="field" placeholder="np. Nordcom Software" value={form.company} onChange={(e) => upd("company", e.target.value)} list="company-list" />
              <datalist id="company-list">
                {COMPANIES.map(c => <option key={c.name} value={c.name}>{c.domain}</option>)}
              </datalist>
              <span className="hint">Zacznij wpisywać — podpowiedzi z listy zatwierdzonych firm.</span>
            </div>

            <div className="form-field required">
              <label>Opiekun praktyki (z ramienia firmy)</label>
              <input className="field" placeholder="Imię i nazwisko" value={form.supervisor} onChange={(e) => upd("supervisor", e.target.value)} />
            </div>
            <div className="form-field">
              <label>E-mail opiekuna</label>
              <input className="field" type="email" placeholder="opiekun@firma.pl" value={form.supervisorEmail} onChange={(e) => upd("supervisorEmail", e.target.value)} />
            </div>

            <div className="form-field required">
              <label>Rodzaj praktyki</label>
              <select className="field" value={form.type} onChange={(e) => upd("type", e.target.value)}>
                {TYPES.map(t => <option key={t}>{t}</option>)}
              </select>
            </div>
            <div className="form-field">
              <label>Wymiar (godziny)</label>
              <input className="field" type="number" min="60" step="20" value={form.hours} onChange={(e) => upd("hours", e.target.value)} />
              <span className="hint">Wg programu studiów minimum 160 h.</span>
            </div>

            <div className="form-field required">
              <label>Data rozpoczęcia</label>
              <input className="field" type="date" value={form.from} onChange={(e) => upd("from", e.target.value)} />
            </div>
            <div className="form-field required">
              <label>Data zakończenia</label>
              <input className="field" type="date" value={form.to} onChange={(e) => upd("to", e.target.value)} />
            </div>

            <div className="form-field full">
              <label>Krótki opis zadań</label>
              <textarea className="field" placeholder="Zakres obowiązków, technologie, dział…" value={form.description} onChange={(e) => upd("description", e.target.value)} />
            </div>
          </div>
        </div>
        <div className="modal-foot">
          <button className="btn btn-text" onClick={onClose}>Anuluj</button>
          <button className="btn btn-outlined">Zapisz jako roboczą</button>
          <button className="btn btn-primary" disabled={!valid} onClick={() => valid && onSubmit(form)} style={!valid ? {opacity:0.5, cursor:"not-allowed", boxShadow:"none"} : {}}>
            <Icon name="check" size={14}/>Wyślij do akceptacji
          </button>
        </div>
      </div>
    </div>
  );
}

window.StudentPanel = StudentPanel;
