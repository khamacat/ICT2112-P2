# PRO-RENTAL PROJECT: STRICT ARCHITECTURE & AI AGENT INSTRUCTIONS
**TARGET AUDIENCE:** Developers and AI Coding Assistants (Team P2-3 / Module 2).
**SYSTEM PROMPT:** You are an expert .NET enterprise developer. You MUST strictly adhere to the following architectural constraints, Domain-Driven Design (DDD) principles, file structures, and naming conventions. Deviation is strictly prohibited.

## 1. THE 3-LAYERED ARCHITECTURE (STRICT ENFORCEMENT)
* **FORBIDDEN:** Controllers MUST NEVER instantiate or inject `AppDbContext` directly. 
* **REQUIRED FLOW:** Controllers -> Interface (`I...Control` / Facade) -> Control Class -> Interface (`I...Mapper`) -> Mapper Class -> `AppDbContext`.
* **DEPENDENCY INJECTION:** If a feature "works" but isn't registered in `Program.cs`, it is a violation. All Interfaces, Controls, and Mappers MUST be explicitly registered in `Program.cs` under the `//Team P2-3` section.

## 2. ENTITIES & ENCAPSULATION (SIMPLE DOMAIN MODEL)
* **SCAFFOLDED ENTITIES:** Files inside `Domain/Entities/` are auto-generated. **DO NOT EDIT THEM.**
* **MANUAL PARTIALS:** All custom entity logic MUST go into `Domain/P2-3/Entities/`.
* **NO ANEMIC ENTITIES:** Entity properties with capitalized names (e.g., `Name`, `Status`) are strictly for EF Core mapping. You MUST NOT use `public set` on these properties.
* **ENCAPSULATION RULE:** State changes MUST only occur through explicit, named domain methods (e.g., `.UpdateStatus()`, `.CompleteReturn()`). Raw setters must be `private` or `internal` (e.g., `internal void SetSerialNumber(...)`).
* **DB CONTEXT:** `Data/UnitOfWork/AppDbContext.cs` is auto-generated. **DO NOT EDIT IT.** All custom model building goes in `Data/UnitOfWork/AppDbContext.custom.cs`. Ideally, do not touch these files unless absolutely necessary.

## 3. BOUNDED CONTEXTS & DTOs (CROSS-MODULE DATA TRANSFER)
* **INTERNAL BOUNDARY (Within Module 2):** Controllers, Controls, and Mappers inside Module 2 MUST pass the live, raw EF-tracked Domain Entities (e.g., `Product`, `Returnrequest`) to each other. DO NOT create "Internal DTOs" to move data within our own module.
* **EXTERNAL BOUNDARY (Module 2 to Module 1):** Module 1 (Order Processing/Customer UI) and Module 3 (Environmental and Carbon Footprint Service) MUST NEVER receive a live Domain Entity. 
* **THE DTO RULE:** When exposing data to other modules, Control/Facade classes must map the Entity to an immutable C# `record` (DTO).
  * *1-to-1 Relationships:* Flatten the data (e.g., combine `Product` and `Productdetail` into one `ProductInfo` record).
  * *1-to-Many Relationships:* Nest the data (e.g., `ReturnRequestInfo` contains a `List<ReturnItemBasicInfo>`).
* **READ-ONLY UPDATES:** DTOs are strictly read-only snapshots. DO NOT accept DTOs back as arguments for database updates. Cross-module updates must happen via specific parameterized method calls (e.g., `UpdateProductStatus(int id, string status)`).

## 4. STRICT NAMESPACE MAPPING
You must use the exact namespaces below based on the class type. Do not invent new namespaces.
* **Data Gateways & Mappers:** `namespace ProRental.Data;`
* **Scaffolded & Manual Entities:** `namespace ProRental.Domain.Entities;`
* **Control Classes:** `namespace ProRental.Domain.Controls;`
* **All Interfaces:** `namespace ProRental.Interfaces.Data;` OR `namespace ProRental.Interfaces.Domain;`
* **DTO Records:** `namespace ProRental.Interfaces.DTOs;`
* **Controllers:** `namespace ProRental.Controllers;`

## 5. STRICT FILE LOCATION DIRECTORY
Place new files exactly in these directories.
* **Mappers:** `Domain/Module2/Gateways/*.cs`
* **Data Interfaces (IMappers):** `Domain/Module2/Interfaces/*.cs`
* **DbContexts:** `Data/UnitOfWork/`
* **Scaffolded Entities (Read-Only):** `Domain/Entities/`
* **Manual Partial Entities (Edit Here):** `Domain/P2-3/Entities/`
* **Control Classes:** `Domain/Module2/Controls/` (Subfolders allowed)
* **Domain Interfaces (IControls):** `Interfaces/P2-3/`
* **DTO Records:** `Interfaces/DTOs/*.cs`
* **Controller Classes:** `Controllers/Module2/*.cs`
* **Views:** `Views/Home/` AND `Views/Module2/`
* **Program Registration:** `Program.cs` (Root directory)

## 6. DATETIME & TIMESTAMPTZ HANDLING (STRICT POSTGRESQL RULES)
PostgreSQL strictly enforces time zones. You MUST follow these rules to prevent Npgsql conversion crashes:
* **The Rule of UTC:** NEVER use `DateTime.Now`. All internal system times must be generated using `DateTime.UtcNow`.
* **User Input (Forms/Views):** Any `DateTime` received from an HTTP request or form submission MUST be explicitly forced to UTC before sending it to a Mapper or Domain Control using `DateTime.SpecifyKind(dateVariable, DateTimeKind.Utc)` or `.ToUniversalTime()`.
* **Calculations:** Perform all date math (duration, expiry) strictly in UTC.
* **Displaying to Users:** Only convert UTC dates to local time at the very last step in the Presentation Layer using `.ToLocalTime()`.

## 7. DATABASE SEEDING & TESTING CONSTRAINTS
* **The PostgreSQL Sequence Desync Trap:** If you write raw SQL that uses `OVERRIDING SYSTEM VALUE` to hardcode IDs (e.g., `INSERT INTO LoanList (LoanListId...) VALUES (1...)`), it breaks EF Core's auto-incrementing Sequences, resulting in `23505 duplicate key violates unique constraint` errors.
* **The Fix:** Any SQL script or test that hardcodes IDs MUST end with a sequence resync command. *(Example: `SELECT setval(pg_get_serial_sequence('loanlist', 'loanlistid'), COALESCE((SELECT MAX(loanlistid) FROM loanlist), 1));`)*

## 8. CODE REVIEW CHECKLIST FOR AI AGENTS
Before generating or finalizing code, verify:
1. Did I bypass the Domain layer by putting DbContext in a Controller? (If yes, rewrite).
2. Did I use an Entity's raw public property (e.g. `entity.Name = "X"`) instead of an explicit business method or `private`/`internal` setter? (If yes, rewrite).
3. Did I pass a live Entity to an external module instead of mapping it to an immutable `record` DTO? (If yes, rewrite to use a DTO).
4. Did I put a new interface in the `ProRental.Data` namespace instead of `ProRental.Interfaces.Data`? (If yes, fix the namespace).
5. Did I modify a scaffolded entity instead of the partial class in `P2-3`? (If yes, move the code).
6. Did I use `DateTime.Now` or pass an unspecified DateTime to the database instead of enforcing `DateTime.UtcNow`? (If yes, rewrite).