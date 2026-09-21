USE HR_Workspace_DB;
GO

-- 1. Insert Initial Employee Data (Populates the Employee Dropdown)
INSERT INTO Employees (FirstName, LastName, Email, Role, IsActive) VALUES 
('Sokha', 'Meng', 'sokha.meng@workspace.kh', 'Administrator', 1),
('Borey', 'Chan', 'borey.chan@workspace.kh', 'Manager', 1),
('Sophea', 'Phan', 'sophea.phan@workspace.kh', 'Employee', 1),
('Vannak', 'Kim', 'vannak.kim@workspace.kh', 'Employee', 1),
('Srey', 'Sok', 'srey.sok@workspace.kh', 'Employee', 0); -- Suspended user to test validation

-- 2. Insert Physical Workspace Desks (Populates the Hot-Desking Assets)
INSERT INTO WorkspaceDesks (FloorNumber, DeskCode, IsAvailable, ZoneGroup) VALUES 
(1, 'DSK-101', 1, 'Engineering'),
(1, 'DSK-102', 1, 'Engineering'),
(1, 'DSK-103', 0, 'Engineering'), -- Currently booked/unavailable asset
(2, 'DSK-201', 1, 'Sales'),
(2, 'DSK-202', 1, 'Sales'),
(3, 'DSK-301', 1, 'Management');

-- 3. Insert Baseline Scheduled Shifts (Populates the Master Ledger Table)
-- Note: Adjust dates to match your current testing window if needed
INSERT INTO Shifts (EmployeeID, ShiftDate, StartTime, EndTime, ScheduledDeskID, Status) VALUES 
(3, CAST(GETDATE() AS DATE), '08:00:00', '17:00:00', 1, 'Scheduled'),
(4, CAST(GETDATE() AS DATE), '09:00:00', '18:00:00', 4, 'Scheduled');

-- 4. Verify Data Insertion
SELECT 'Employees Loaded' AS TableName, COUNT(*) FROM Employees
UNION ALL
SELECT 'Desks Loaded', COUNT(*) FROM WorkspaceDesks
UNION ALL
SELECT 'Shifts Loaded', COUNT(*) FROM Shifts;
GO
