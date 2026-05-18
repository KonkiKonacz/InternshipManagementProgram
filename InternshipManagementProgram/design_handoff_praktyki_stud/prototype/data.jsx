// Sample data for the internship management app

const STATUSES = {
  Zaliczona:   { label: "Zaliczona",   chip: "chip-success", dot: "var(--success)" },
  Niezaliczona:{ label: "Niezaliczona",chip: "chip-error",   dot: "var(--error)" },
  WTrakcie:    { label: "W trakcie",   chip: "chip-info",    dot: "var(--info)" },
  Oczekujaca:  { label: "Oczekująca",  chip: "chip-warning", dot: "var(--warning)" },
  Odrzucona:   { label: "Odrzucona",   chip: "chip-neutral", dot: "var(--neutral)" },
  Robocza:     { label: "Robocza",     chip: "chip-neutral", dot: "var(--neutral)" },
};

const TYPES = ["Obowiązkowa", "Nadobowiązkowa", "Wakacyjna", "Dyplomowa"];

const COMPANIES = [
  { name: "Nordcom Software", short: "NS", domain: "Software / B2B", color: "linear-gradient(135deg,#1f2f5e,#0f1a3c)" },
  { name: "Vistalink Logistics", short: "VL", domain: "Logistyka",   color: "linear-gradient(135deg,#4d8df6,#1f5fc4)" },
  { name: "BlueOak Bank",       short: "BB", domain: "Bankowość",   color: "linear-gradient(135deg,#36b24c,#1d7a30)" },
  { name: "Mediform Sp. z o.o.",short: "MF", domain: "MedTech",     color: "linear-gradient(135deg,#f0c419,#c89c00)" },
  { name: "Helion Energy",      short: "HE", domain: "Energetyka", color: "linear-gradient(135deg,#ff9a3c,#c66a14)" },
  { name: "PixelForge Studio",  short: "PF", domain: "Gamedev",     color: "linear-gradient(135deg,#f15463,#a02230)" },
  { name: "Greenfield AgriTech",short: "GA", domain: "AgriTech",    color: "linear-gradient(135deg,#4ac255,#2a9a3e)" },
  { name: "Skylab Robotics",    short: "SR", domain: "Robotyka",    color: "linear-gradient(135deg,#2a3f7a,#1a2547)" },
];

const SEMESTERS = ["2025/26 zimowy", "2025/26 letni", "2024/25 zimowy", "2024/25 letni"];

// Student panel — practices belonging to the logged-in student
const STUDENT_PRACTICES = [
  {
    id: "PR-2026-0142",
    company: COMPANIES[0],
    type: "Obowiązkowa",
    status: "WTrakcie",
    dateFrom: "2026-03-02",
    dateTo:   "2026-05-29",
    hours: 320,
    hoursDone: 184,
    supervisor: "mgr inż. Anna Wiśniewska",
    semester: "2025/26 letni",
    grade: null,
    timeline: [
      { label: "Zgłoszenie złożone",     date: "2026-02-10", state: "done" },
      { label: "Akceptacja opiekuna",    date: "2026-02-14", state: "done" },
      { label: "Akceptacja administratora", date: "2026-02-18", state: "done" },
      { label: "Realizacja praktyki",    date: "od 02.03.2026", state: "active" },
      { label: "Raport końcowy",         date: "do 05.06.2026", state: "pending" },
      { label: "Zaliczenie",             date: "do 12.06.2026", state: "pending" },
    ],
  },
  {
    id: "PR-2025-0088",
    company: COMPANIES[2],
    type: "Wakacyjna",
    status: "Zaliczona",
    dateFrom: "2025-07-01",
    dateTo:   "2025-08-31",
    hours: 240,
    hoursDone: 240,
    supervisor: "dr Marek Lewandowski",
    semester: "2024/25 letni",
    grade: 5.0,
  },
  {
    id: "PR-2024-0061",
    company: COMPANIES[1],
    type: "Nadobowiązkowa",
    status: "Zaliczona",
    dateFrom: "2024-10-15",
    dateTo:   "2025-01-31",
    hours: 200,
    hoursDone: 200,
    supervisor: "mgr Joanna Kaczmarek",
    semester: "2024/25 zimowy",
    grade: 4.5,
  },
  {
    id: "PR-2024-0014",
    company: COMPANIES[5],
    type: "Wakacyjna",
    status: "Niezaliczona",
    dateFrom: "2024-07-01",
    dateTo:   "2024-08-30",
    hours: 160,
    hoursDone: 92,
    supervisor: "mgr Piotr Górski",
    semester: "2023/24 letni",
    grade: 2.0,
  },
  {
    id: "PR-2026-0211",
    company: COMPANIES[7],
    type: "Dyplomowa",
    status: "Oczekujaca",
    dateFrom: "2026-06-15",
    dateTo:   "2026-09-30",
    hours: 360,
    hoursDone: 0,
    supervisor: "—",
    semester: "2025/26 letni",
    grade: null,
  },
];

// Admin panel — practices across all students
const ADMIN_PRACTICES = [
  { id: "PR-2026-0211", student: "Tomasz Kowalczyk", studentId: "165142", company: COMPANIES[7], type: "Dyplomowa",      status: "Oczekujaca",  dateFrom: "2026-06-15", dateTo: "2026-09-30", semester: "2025/26 letni",  grade: null },
  { id: "PR-2026-0210", student: "Aleksandra Nowicka", studentId: "163987", company: COMPANIES[0], type: "Obowiązkowa",   status: "WTrakcie",    dateFrom: "2026-03-02", dateTo: "2026-05-29", semester: "2025/26 letni",  grade: null },
  { id: "PR-2026-0204", student: "Kacper Zieliński",   studentId: "164301", company: COMPANIES[3], type: "Obowiązkowa",   status: "WTrakcie",    dateFrom: "2026-03-09", dateTo: "2026-06-05", semester: "2025/26 letni",  grade: null },
  { id: "PR-2026-0199", student: "Julia Mazur",        studentId: "165988", company: COMPANIES[1], type: "Nadobowiązkowa",status: "Zaliczona",   dateFrom: "2026-01-15", dateTo: "2026-04-10", semester: "2025/26 letni",  grade: 5.0 },
  { id: "PR-2026-0187", student: "Mateusz Wojciechowski", studentId: "163212", company: COMPANIES[4], type: "Obowiązkowa", status: "Niezaliczona",dateFrom: "2025-10-01", dateTo: "2026-01-29", semester: "2025/26 zimowy", grade: 2.0 },
  { id: "PR-2026-0142", student: "Anna Wiśniewska",    studentId: "166044", company: COMPANIES[0], type: "Obowiązkowa",   status: "WTrakcie",    dateFrom: "2026-03-02", dateTo: "2026-05-29", semester: "2025/26 letni",  grade: null },
  { id: "PR-2025-0125", student: "Bartosz Kamiński",   studentId: "162004", company: COMPANIES[6], type: "Wakacyjna",     status: "Zaliczona",   dateFrom: "2025-07-01", dateTo: "2025-08-31", semester: "2024/25 letni",  grade: 4.5 },
  { id: "PR-2025-0118", student: "Klaudia Pawlak",     studentId: "164765", company: COMPANIES[2], type: "Wakacyjna",     status: "Zaliczona",   dateFrom: "2025-07-15", dateTo: "2025-09-15", semester: "2024/25 letni",  grade: 5.0 },
  { id: "PR-2025-0099", student: "Damian Szymański",   studentId: "163551", company: COMPANIES[5], type: "Nadobowiązkowa",status: "Odrzucona",   dateFrom: "2025-09-01", dateTo: "2025-12-15", semester: "2025/26 zimowy", grade: null },
  { id: "PR-2025-0088", student: "Anna Wiśniewska",    studentId: "166044", company: COMPANIES[2], type: "Wakacyjna",     status: "Zaliczona",   dateFrom: "2025-07-01", dateTo: "2025-08-31", semester: "2024/25 letni",  grade: 5.0 },
  { id: "PR-2026-0156", student: "Igor Pietrzak",      studentId: "165120", company: COMPANIES[3], type: "Obowiązkowa",   status: "WTrakcie",    dateFrom: "2026-02-20", dateTo: "2026-05-30", semester: "2025/26 letni",  grade: null },
  { id: "PR-2025-0070", student: "Natalia Sobczak",    studentId: "164002", company: COMPANIES[1], type: "Obowiązkowa",   status: "Niezaliczona",dateFrom: "2024-11-02", dateTo: "2025-02-15", semester: "2024/25 zimowy", grade: 2.0 },
];

const STUDENTS_NO_PRACTICE = [
  { id: "165501", name: "Wiktoria Adamczyk", program: "Informatyka, sem. 6", email: "165501@stud.pl", semester: "2025/26 letni", reqType: "Obowiązkowa" },
  { id: "164822", name: "Sebastian Krawczyk", program: "Informatyka, sem. 6", email: "164822@stud.pl", semester: "2025/26 letni", reqType: "Obowiązkowa" },
  { id: "166120", name: "Karolina Dudek",     program: "Inżynieria Danych, sem. 6", email: "166120@stud.pl", semester: "2025/26 letni", reqType: "Obowiązkowa" },
  { id: "165844", name: "Jakub Witkowski",    program: "Informatyka, sem. 4", email: "165844@stud.pl", semester: "2025/26 letni", reqType: "Wakacyjna" },
  { id: "165071", name: "Oliwia Baran",       program: "Cyberbezpieczeństwo, sem. 6", email: "165071@stud.pl", semester: "2025/26 letni", reqType: "Obowiązkowa" },
  { id: "164510", name: "Dawid Stępień",      program: "Informatyka, sem. 6", email: "164510@stud.pl", semester: "2025/26 letni", reqType: "Obowiązkowa" },
];

const STUDENTS_FAILED = [
  { id: "163212", name: "Mateusz Wojciechowski", program: "Informatyka, sem. 7", attempts: 1, lastCompany: "Helion Energy", failedAt: "2026-01-29", grade: 2.0 },
  { id: "164002", name: "Natalia Sobczak",       program: "Informatyka, sem. 6", attempts: 2, lastCompany: "Vistalink Logistics", failedAt: "2025-02-15", grade: 2.0 },
  { id: "163551", name: "Damian Szymański",     program: "Inżynieria Danych, sem. 6", attempts: 1, lastCompany: "PixelForge Studio", failedAt: "2025-12-15", grade: null, note: "Praktyka odrzucona — niezgodna z kierunkiem" },
];

window.AppData = {
  STATUSES, TYPES, COMPANIES, SEMESTERS,
  STUDENT_PRACTICES, ADMIN_PRACTICES,
  STUDENTS_NO_PRACTICE, STUDENTS_FAILED,
};
