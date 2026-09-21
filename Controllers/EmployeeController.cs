using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace HR_Workspace_System.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly string _connectionString = "Server=YOUR_WINDOWS_SERVER_IP;Database=HR_Workspace_DB;User Id=sa;Password=YourSecurePassword;TrustServerCertificate=True;";

        // 1. READ: Display list of all employees in the system
        [HttpGet]
        public IActionResult Index()
        {
            DataTable employeeTable = new DataTable();
            
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT EmployeeID, FirstName, LastName, Email, Role, IsActive FROM Employees ORDER BY EmployeeID DESC";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            adapter.Fill(employeeTable);
                        }
                    }
                    catch (Exception ex)
                    {
                        TempData["Error"] = $"Failed to fetch records: {ex.Message}";
                    }
                }
            }
            return View(employeeTable);
        }

        // 2. CREATE: Process form submission to add a new employee profile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(string firstName, string lastName, string email, string role)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"INSERT INTO Employees (FirstName, LastName, Email, Role, IsActive) 
                                 VALUES (@FirstName, @LastName, @Email, @Role, 1);";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@FirstName", SqlDbType.VarChar).Value = firstName;
                    command.Parameters.Add("@LastName", SqlDbType.VarChar).Value = lastName;
                    command.Parameters.Add("@Email", SqlDbType.VarChar).Value = email;
                    command.Parameters.Add("@Role", SqlDbType.VarChar).Value = role;

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        TempData["Success"] = "New employee record committed successfully.";
                    }
                    catch (Exception ex)
                    {
                        TempData["Error"] = $"Insert transaction aborted: {ex.Message}";
                    }
                }
            }
            return RedirectToAction("Index");
        }

        // 3. UPDATE: Alter employee structural roles or status states
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateStatus(int employeeId, bool isActive)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "UPDATE Employees SET IsActive = @IsActive WHERE EmployeeID = @EmployeeID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@EmployeeID", SqlDbType.Int).Value = employeeId;
                    command.Parameters.Add("@IsActive", SqlDbType.Bit).Value = isActive;

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        TempData["Success"] = "Employee state updated in database cache.";
                    }
                    catch (Exception ex)
                    {
                        TempData["Error"] = $"Update failed: {ex.Message}";
                    }
                }
            }
            return RedirectToAction("Index");
        }

        // 4. DELETE: Formally purge a record from the database architecture
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int employeeId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "DELETE FROM Employees WHERE EmployeeID = @EmployeeID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@EmployeeID", SqlDbType.Int).Value = employeeId;

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        TempData["Success"] = "Employee entry purged from system clusters.";
                    }
                    catch (Exception ex)
                    {
                        TempData["Error"] = $"Delete rejected due to relational foreign constraints: {ex.Message}";
                    }
                }
            }
            return RedirectToAction("Index");
        }
    }
}
