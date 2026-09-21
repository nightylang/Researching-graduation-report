# System Implementation & Source Code Repository Blueprint
**Architecture:** Multi-Tier Enterprise Web Application (System Dashboard & Public Portal)
**Language Ecosystem:** C# ASP.NET Core MVC & Microsoft SQL Server

---

## 📅 Chronological Coding & Chapter Mapping Timeline

[Week 1: SQL Infrastructure] ──> [Week 2: Data Access Layer] ──> [Week 3: Management Engines] ──> [Week 4: Telemetry Gateway]

---

## 🗄️ Week 1: Relational Database Schema Infrastructure
*   **Corresponding Thesis Section:**
*       Chapter 3, Section 3.2 (Relational Database Schema Design)
*   **Target Objective:**
*       To build an ACID-compliant transaction backend optimized for local hosting environments.

Execute this script inside your **SQL Server Management Studio (SSMS)** to generate your data structures and seed files:

```sql
CREATE DATABASE HR_Workspace_DB;
GO
USE HR_Workspace_DB;
GO

-- 1. Employees Table (Managed via System Admin CRUD Engine)
CREATE TABLE Employees (
    EmployeeID INT IDENTITY(1,1) PRIMARY KEY,
    FirstName VARCHAR(50) NOT NULL,
    LastName VARCHAR(50) NOT NULL,
    Email VARCHAR(100) UNIQUE NOT NULL,
    Role VARCHAR(30) NOT NULL DEFAULT 'Employee',
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME DEFAULT GETDATE()
);

-- 2. Workspace Desks Table (The Hot-Desking Structural Schema)
CREATE TABLE WorkspaceDesks (
    DeskID INT IDENTITY(1,1) PRIMARY KEY,
    FloorNumber INT NOT NULL,
    DeskCode VARCHAR(10) UNIQUE NOT NULL,
    IsAvailable BIT NOT NULL DEFAULT 1,
    ZoneGroup VARCHAR(50) NOT NULL
);

-- 3. Shift Schedules Table (The Manager Dashboard Controller)
CREATE TABLE Shifts (
    ShiftID INT IDENTITY(1,1) PRIMARY KEY,
    EmployeeID INT FOREIGN KEY REFERENCES Employees(EmployeeID),
    ShiftDate DATE NOT NULL,
    StartTime TIME NOT NULL,
    EndTime TIME NOT NULL,
    ScheduledDeskID INT FOREIGN KEY REFERENCES WorkspaceDesks(DeskID) NULL,
    Status VARCHAR(20) DEFAULT 'Scheduled',
    CONSTRAINT UC_Employee_Shift UNIQUE (EmployeeID, ShiftDate)
);

-- 4. Real-Time Telemetry Check-In Ledger (The Check-In App Database Engine)
CREATE TABLE CheckInLogs (
    LogID INT IDENTITY(1,1) PRIMARY KEY,
    ShiftID INT FOREIGN KEY REFERENCES Shifts(ShiftID),
    EmployeeID INT FOREIGN KEY REFERENCES Employees(EmployeeID),
    CheckInTime DATETIME DEFAULT GETDATE(),
    NetworkIP VARCHAR(45) NOT NULL,
    DeviceIdentifier VARCHAR(100) NOT NULL,
    VerificationStatus VARCHAR(20) NOT NULL DEFAULT 'Pending'
);

-- Speed-Optimization Indexes (Crucial for Chapter 5 Performance Benchmarking!)
CREATE INDEX IDX_Shifts_Date ON Shifts(ShiftDate);
CREATE INDEX IDX_CheckIn_Time ON CheckInLogs(CheckInTime);
GO

-- Seed Data for UI Component Testing
INSERT INTO Employees (FirstName, LastName, Email, Role, IsActive) VALUES 
('Sokha', 'Meng', 'sokha.meng@workspace.kh', 'Administrator', 1),
('Borey', 'Chan', 'borey.chan@workspace.kh', 'Manager', 1),
('Sophea', 'Phan', 'sophea.phan@workspace.kh', 'Employee', 1);

INSERT INTO WorkspaceDesks (FloorNumber, DeskCode, IsAvailable, ZoneGroup) VALUES 
(1, 'DSK-101', 1, 'Engineering'),
(1, 'DSK-102', 1, 'Engineering'),
(2, 'DSK-201', 1, 'Sales');
GO
```

---

## 🛡️ Week 2: Centralized App Setup & Optimized Data Access Layer
*   **Corresponding Thesis Section:**
*       Chapter 3, Section 3.5 (Data Access Optimization & Code Refactoring Framework)
*   **Target Objective:**
*       To resolve memory-leak hazards on localized servers by implementing thread-safe global connection pooling.

### File A: `/appsettings.json`

### File B: `/Data/DatabaseHelper.cs`

### File C: `/Program.cs` (Global Core Pipeline Infrastructure)

---

## 💻 Week 3: Dashboard Control Panels & CRUD Engines
*   **Corresponding Thesis Section:**
*       Chapter 4, Section 4.2 (Component Blueprint & Identity Ledgers)
*   **Target Objective:**
*       To construct the administrative dashboard logic managing employees and hot-desking resource allocations.

### File A: `/Controllers/EmployeeController.cs`

### File B: `/Views/Employee/Index.cshtml`

---

## 🌐 Week 4: Public Mobile Portal & Security Telemetry Handshake
*    **Corresponding Thesis Section:**
*         Chapter 4, Section 4.3 (Network Geofencing & Fraud-Prevention Interception)
*    **Target Objective:**
*         To validate employee locations at runtime via IP address logging, preventing off-site time fraud without physical hardware.

### File A: `/Controllers/CheckInController.cs`

### File B: `/Views/CheckIn/Index.cshtml`

---

<FollowUp>
Now that your complete code framework, including the text/html layouts, is restored, let me know if you would like me to:
* Prepare a **targeted list of sample evaluation datasets** for Chapter 5 to mock your latencies.
* Or detail the **database schema configuration** for the `Shifts` and `Desks` views to finalize your dashboard features.
</FollowUp>
