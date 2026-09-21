using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace HR_Workspace_System.Controllers
{
    public class ShiftController : Controller
    {
        private readonly string _connectionString = "Server=YOUR_WINDOWS_SERVER_IP;Database=HR_Workspace_DB;User Id=sa;Password=YourSecurePassword;TrustServerCertificate=True;";

        // READ: Display all scheduled shifts alongside desk configurations
        [HttpGet]
        public IActionResult Index()
        {
            DataSet dashboardData = new DataSet();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                // Query 1: Fetch active shifts with Employee and Desk names using Relational Joins
                string shiftQuery = @"
                    SELECT s.ShiftID, s.ShiftDate, s.StartTime, s.EndTime, s.Status,
                           e.FirstName + ' ' + e.LastName AS EmployeeName,
                           d.DeskCode + ' (Fl ' + CAST(d.FloorNumber AS VARCHAR) + ')' AS DeskDetails
                    FROM Shifts s
                    INNER JOIN Employees e ON s.EmployeeID = e.EmployeeID
                    INNER JOIN WorkspaceDesks d ON s.ScheduledDeskID = d.DeskID
                    ORDER BY s.ShiftDate DESC, s.StartTime ASC";

                // Query 2: Fetch available desks for the dropdown list
                string deskQuery = "SELECT DeskID, DeskCode, FloorNumber FROM WorkspaceDesks WHERE IsAvailable = 1";

                // Query 3: Fetch active employees for the dropdown list
                string employeeQuery = "SELECT EmployeeID, FirstName + ' ' + LastName AS FullName FROM Employees WHERE IsActive = 1";

                using (SqlCommand command = new SqlCommand(shiftQuery + ";" + deskQuery + ";" + employeeQuery, connection))
                {
                    try
                    {
                        connection.Open();
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            // Fill all three datasets to populate the manager dashboard UI dynamically
                            adapter.Fill(dashboardData);
                            dashboardData.Tables[0].TableName = "ShiftsTable";
                            dashboardData.Tables[1].TableName = "DesksTable";
                            dashboardData.Tables[2].TableName = "EmployeesTable";
                        }
                    }
                    catch (Exception ex)
                    {
                        TempData["Error"] = $"Infrastructure read error: {ex.Message}";
                    }
                }
            }
            return View(dashboardData);
        }

        // CREATE: Schedule a shift and automatically bind a Hot-Desk asset
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AllocateShift(int employeeId, int deskId, DateTime shiftDate, TimeSpan startTime, TimeSpan endTime)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                // Uses unique constraint safeguards to block duplicate shift bookings per employee per day
                string query = @"INSERT INTO Shifts (EmployeeID, ShiftDate, StartTime, EndTime, ScheduledDeskID, Status) 
                                 VALUES (@EmployeeID, @ShiftDate, @StartTime, @EndTime, @DeskID, 'Scheduled');";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@EmployeeID", SqlDbType.Int).Value = employeeId;
                    command.Parameters.Add("@ShiftDate", SqlDbType.Date).Value = shiftDate;
                    command.Parameters.Add("@StartTime", SqlDbType.Time).Value = startTime;
                    command.Parameters.Add("@EndTime", SqlDbType.Time).Value = endTime;
                    command.Parameters.Add("@DeskID", SqlDbType.Int).Value = deskId;

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        TempData["Success"] = "Shift transaction bound and allocated successfully.";
                    }
                    catch (SqlException ex) when (ex.Number == 2627) // Catch unique constraint violations directly
                    {
                        TempData["Error"] = "Scheduling Conflict: This employee is already booked for a shift on this date.";
                    }
                    catch (Exception ex)
                    {
                        TempData["Error"] = $"Relational execution failed: {ex.Message}";
                    }
                }
            }
            return RedirectToAction("Index");
        }
    }
}
