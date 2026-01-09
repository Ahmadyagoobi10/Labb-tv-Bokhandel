
// Bokhandel – Console App med Entity Framework

// Beskrivning
C# Console-applikation som använder Entity Framework för att hantera en bokhandel. 
Användaren kan lista, lägga till, uppdatera och ta bort böcker samt lista författare.

// Krav
- .NET 8
- SQL Server
- EF Core

// Databas
- Databasnamn: Bokhandel
- Tabellen `Books`, `Authors`, `BookAuthors` används
- Connection string finns i `BokhandelContext.cs`

// Körning
1. Klona repot
2. Öppna projektet i Visual Studio
3. Kör `Program.cs`
4. Följ menyn i console-appen för CRUD-operationer

// Obs
- Böcker som används i lager (`Inventory`) eller order (`OrderDetails`) kan inte tas bort
- CRUD-operationer fungerar för alla andra böcker
