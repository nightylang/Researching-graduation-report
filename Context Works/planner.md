# Feature Implementation & Deployment Planner
**Project Structure:** Two Interconnected Sub-Projects (Admin Control Dashboard & Public Website Portal)  
**Core Modules:** Control, Manager, CRUD, and Check-In  
**Target Environment:** Local Hybrid Server Network (Ubuntu Server + Windows Server + SQL Server)

---

## 🗺️ High-Level Project Architecture Flow

*  [Public Web Portal ] ──> (Internet/LAN) ──> [ C# ASP.NET Core MVC Engine ]
*  (PHP on Ubuntu Server)  (Windows Server Host via Port 1433)
*  │
*  ▼
*  [ SQL Server ACID Ledger ]

---

## 🗓️ 30-Day Feature Roadmap & Implementation Phases

*   [Phase 1: DB Infrastructure] ──> [Phase 2: Administrative CRUD] ──> [Phase 3: Operations Manager] ──> [Phase 4: Telemetry Check-In]

### 🗄️ Phase 1: Relational Data Infrastructure (Days 1–7)
Focuses on establishing the primary relational tables, performance indexes, and multi-tier server validation pipelines.

*   **Feature Goal:** Deploy an ACID-compliant database architecture capable of surviving unexpected regional network or power fluctuations.
*   **Tasks & Objectives:**
    *   Initialize `HR_Workspace_DB` on the Windows Server instance.
    *   Build out normalized core schemas: `Employees`, `WorkspaceDesks`, `Shifts`, and `CheckInLogs`.
    *   Inject composite unique constraints to mathematically block duplicate scheduling errors (`UC_Employee_Shift`).
    *   Create targeted performance indexes (`IDX_Shifts_Date`) to guarantee low search latencies during query peaks.

### 💻 Phase 2: Administrative Control Dashboard & Core CRUD (Days 8–14)
Focuses on the internal admin control tool built on **C# ASP.NET Core MVC** to manage organizational data safely.

*   **Feature Goal:** Provide an isolated data administration dashboard with parameters that block SQL Injection attempts automatically.
*   **Tasks & Objectives:**
    *   Configure global dependency injection inside `Program.cs` to spin up a thread-safe `DatabaseHelper.cs` singleton layer.
    *   Deploy the **Employee Profile CRUD Engine**: Build out programmatic Create, Read, Update, and Delete endpoints inside `EmployeeController.cs`.
    *   Design the secure backend data-mapping routines using parameterized `SqlParameter` arrays.
    *   Develop the Admin management UI panel to review committed employee files and trigger operational state toggles (Active vs. Suspended).

### 📅 Phase 3: Workspace Resource Allocator & Manager Engine (Days 15–21)
Focuses on the core logic mapping human capital to physical corporate infrastructure items (Hot-Desking).

*   **Feature Goal:** Mitigate office real estate waste by dynamically binding schedules to physical workspace locations.
*   **Tasks & Objectives:**
    *   Develop the `ShiftController.cs` operations manager engine.
    *   Build multi-table relational join requests (`INNER JOIN`) to dynamically map staff identities to desk locations, structural layers, and floor plans.
    *   Construct transaction exception handlers (`SqlException 2627`) to instantly catch scheduling conflicts and return clear errors to the operator.
    *   Design the scheduling dashboard frontend view with data-bound dropdown pickers listing unallocated office inventory.

### 🛡️ Phase 4: Public Portal, Telemetry Check-In & Anti-Fraud Gateway (Days 22–30)
Focuses on the user-facing check-in application where employees interact on their mobile devices.

*   **Feature Goal:** Prevent time-theft ("buddy-punching") at the software level through network telemetry verification, eliminating the need for expensive physical hardware.
*   **Tasks & Objectives:**
    *   Deploy the **Public Website Portal Interface** using a lightweight, mobile-responsive layout.
    *   Build the security telemetry interceptor (`HttpContext.Connection.RemoteIpAddress`) inside `CheckInController.cs`.
    *   Implement the network geofence validation rule checking if the incoming request matches the corporate LAN subnet threshold (`192.168.10.`).
    *   Inject browser-level hardware fingerprint capture scripts using client JavaScript telemetry hooks.
    *   Run stress-testing benchmarking protocols (**Target: <250ms Response Latency under 100 concurrent loads**) to compile final metric charts for Chapter 5.

---

## 📈 System Metrics & Feature Validation Matrix
To pass your defense panel review successfully, each planned feature must meet specific engineering benchmarks during testing:

| Planned Feature Module | Academic Validation Term | Primary Target Metric | Failure Prevention Safeguard |
| :--- | :--- | :--- | :--- |
| **Check-In Gateway** | Network Telemetry Handshake | **100% Detection Rate** | Instantly catches and flags off-site connections as `Flagged_Offsite`. |
| **Admin CRUD Tool** | Isolated Identity Transaction Engine | **< 50ms Database Write** | Parameterized queries block all raw SQL injection security threats. |
| **Management Panel** | Distributed Resource Allocator | **0% Duplicate Bookings** | Relational database unique keys stop double-booking user mistakes. |
| **Public Portal Connection** | Heterogeneous Tier Routing | **< 250ms System Latency** | Central connection pooling stops local server crashes under high loads. |

