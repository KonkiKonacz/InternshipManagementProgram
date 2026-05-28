// Testy bazodanowe wspoldziela jedna instancje SQL Server - uruchamiamy je
// sekwencyjnie, by transakcje na osobnych polaczeniach nie blokowaly sie wzajemnie.
[assembly: DoNotParallelize]
