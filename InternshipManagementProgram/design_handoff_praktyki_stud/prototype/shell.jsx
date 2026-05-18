// Shared building blocks (chips, company cell, sidebar, etc.)

const { useState, useMemo, useEffect, useRef } = React;
const { STATUSES, TYPES, COMPANIES, SEMESTERS } = window.AppData;

const fmtDate = (iso) => {
  if (!iso || iso.startsWith("od ") || iso.startsWith("do ")) return iso || "—";
  const d = new Date(iso);
  return d.toLocaleDateString("pl-PL", { day: "2-digit", month: "short", year: "numeric" });
};
const fmtRange = (a, b) => `${fmtDate(a)} – ${fmtDate(b)}`;
const fmtGrade = (g) => (g == null ? "—" : g.toFixed(1));

const StatusChip = ({ value }) => {
  const s = STATUSES[value];
  if (!s) return null;
  return (
    <span className={`chip ${s.chip}`}>
      <span className="dot"></span>{s.label}
    </span>
  );
};

const CompanyCell = ({ company, sub }) => (
  <div className="t-company">
    <div className="t-logo" style={{ background: company.color }}>{company.short}</div>
    <div>
      <div>{company.name}</div>
      {sub && <div className="t-meta">{sub}</div>}
    </div>
  </div>
);

const Brand = () => (
  <div className="brand">
    <div className="brand-mark"><img src="assets/logo.png" alt="PraktykiStud" /></div>
    <div className="brand-text">
      PraktykiStud
      <small>v3.4 · Politechnika</small>
    </div>
  </div>
);

const NavItem = ({ icon, label, active, badge, onClick }) => (
  <button className={`nav-item ${active ? "active" : ""}`} onClick={onClick}>
    <Icon name={icon} size={17} />
    <span className="nav-label">{label}</span>
    {badge != null && <span className="badge-count">{badge}</span>}
  </button>
);

const SidebarStudent = ({ section, onNav }) => (
  <aside className="sidebar">
    <Brand />
    <nav className="nav">
      <div className="nav-section-label">Praktyki</div>
      <NavItem icon="dashboard" label="Pulpit"     active={section === "dashboard"} onClick={() => onNav("dashboard")} />
      <NavItem icon="briefcase" label="Moje praktyki" active={section === "practices"} badge={5} onClick={() => onNav("practices")} />
      <NavItem icon="plus"       label="Nowe zgłoszenie" active={section === "new"} onClick={() => onNav("new")} />
      <NavItem icon="file"       label="Dokumenty"  active={section === "docs"} onClick={() => onNav("docs")} />

      <div className="nav-section-label">Konto</div>
      <NavItem icon="user"     label="Mój profil" active={section === "profile"} onClick={() => onNav("profile")} />
      <NavItem icon="bell"     label="Powiadomienia" badge={2} active={section === "notif"} onClick={() => onNav("notif")} />
      <NavItem icon="helpCircle" label="Pomoc"    active={section === "help"} onClick={() => onNav("help")} />
    </nav>
    <div className="sidebar-foot">
      <div className="avatar" style={{ background: "linear-gradient(135deg,#4ac255,#2a9a3e)", color:"#07210d" }}>AW</div>
      <div className="user-info">
        <div className="name">Anna Wiśniewska</div>
        <div className="meta">nr 166044 · Informatyka</div>
      </div>
      <button className="icon-btn" title="Wyloguj"><Icon name="logout" /></button>
    </div>
  </aside>
);

const SidebarAdmin = ({ section, onNav }) => (
  <aside className="sidebar">
    <Brand />
    <nav className="nav">
      <div className="nav-section-label">Zarządzanie</div>
      <NavItem icon="dashboard"  label="Pulpit"         active={section === "dashboard"} onClick={() => onNav("dashboard")} />
      <NavItem icon="briefcase"  label="Praktyki"        active={section === "practices"} badge={48} onClick={() => onNav("practices")} />
      <NavItem icon="users"      label="Studenci"        active={section === "students"} badge={9}  onClick={() => onNav("students")} />
      <NavItem icon="building"   label="Firmy"           active={section === "companies"} onClick={() => onNav("companies")} />
      <NavItem icon="calendar"   label="Semestry"        active={section === "semesters"} onClick={() => onNav("semesters")} />

      <div className="nav-section-label">Raporty</div>
      <NavItem icon="file"       label="Statystyki"     active={section === "stats"} onClick={() => onNav("stats")} />
      <NavItem icon="award"      label="Oceny"          active={section === "grades"} onClick={() => onNav("grades")} />

      <div className="nav-section-label">System</div>
      <NavItem icon="settings"   label="Ustawienia"     active={section === "settings"} onClick={() => onNav("settings")} />
    </nav>
    <div className="sidebar-foot">
      <div className="avatar" style={{ background: "linear-gradient(135deg,#f0c419,#c89c00)", color:"#2a1d00" }}>JK</div>
      <div className="user-info">
        <div className="name">dr Jan Kowalski</div>
        <div className="meta">Pełnomocnik ds. praktyk</div>
      </div>
      <button className="icon-btn" title="Wyloguj"><Icon name="logout" /></button>
    </div>
  </aside>
);

const Topbar = ({ crumbs, onPanelSwitch, panel, actions }) => (
  <div className="topbar">
    <div className="crumbs">
      {crumbs.map((c, i) => (
        <React.Fragment key={i}>
          {i > 0 && <Icon name="chevronRight" size={14} />}
          {i === crumbs.length - 1 ? <strong>{c}</strong> : <span>{c}</span>}
        </React.Fragment>
      ))}
    </div>
    <div className="topbar-spacer"></div>

    {/* Panel switcher (demo only — normally driven by user role) */}
    <div className="panel-switch" title="Demo: przełącznik widoków paneli">
      <button className={panel === "student" ? "active" : ""} onClick={() => onPanelSwitch("student")}>
        <Icon name="user" size={14} /> Student
      </button>
      <button className={panel === "admin" ? "active" : ""} onClick={() => onPanelSwitch("admin")}>
        <Icon name="settings" size={14} /> Administrator
      </button>
    </div>

    <div className="divider-v"></div>

    <div className="topbar-search">
      <Icon name="search" size={14} />
      <input placeholder="Szukaj praktyk, firm, studentów…" />
      <span className="kbd">⌘K</span>
    </div>
    <button className="icon-btn has-dot" title="Powiadomienia"><Icon name="bell" /></button>
    {actions}
  </div>
);

const Toast = ({ children }) => (
  <div className="toast">
    <div className="toast-icon"><Icon name="check" size={14} /></div>
    <div>{children}</div>
  </div>
);

Object.assign(window, {
  fmtDate, fmtRange, fmtGrade,
  StatusChip, CompanyCell,
  SidebarStudent, SidebarAdmin, Topbar, Toast, NavItem,
});
