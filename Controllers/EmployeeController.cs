using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using HR_Workspace_System.Data;

namespace HR_Workspace_System.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly DatabaseHelper _dbHelper;

        // Dependency injection handles connection infrastructure automatically
        public EmployeeController(DatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        // 1. READ ALL EMPLOYEES
        [HttpGet]
        public IActionResult Index()
        {
            string query = "SELECT EmployeeID, FirstName, LastName, Email, Role, IsActive FROM Employees ORDER BY EmployeeID DESC";
            try
            {
                DataTable employeeTable = _dbHelper.ExecuteReader(query);
                return View(employeeTable);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Identity lookup cluster failure: {ex.Message}";
                return View(new DataTable());
            }
        }

        // 2. CREATE EMPLOYEE (CRUD Engine)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(string firstName, string lastName, string email, string role)
        {
            string query = @"INSERT INTO Employees (FirstName, LastName, Email, Role, IsActive) 
                             VALUES (@FirstName, @LastName, @Email, @Role, 1);";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@FirstName", SqlDbType.VarChar) { Value = firstName },
                new SqlParameter("@LastName", SqlDbType.VarChar) { Value = lastName },
                new SqlParameter("@Email", SqlDbType.VarChar) { Value = email },
                new SqlParameter("@Role", SqlDbType.VarChar) { Value = role }
            };

            try
            {
                _dbHelper.ExecuteNonQuery(query, parameters);
                TempData["Success"] = "New profile successfully committed to the database.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Relational insertion aborted: {ex.Message}";
            }
            return RedirectToAction("Index");
        }

        // 3. UPDATE EMPLOYEE STATUS STATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateStatus(int employeeId, bool isActive)
        {
            string query = "UPDATE Employees SET IsActive = @IsActive WHERE EmployeeID = @EmployeeID";
            
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@EmployeeID", SqlDbType.Int) { Value = employeeId },
                new SqlParameter("@IsActive", SqlDbType.Bit) { Value = isActive }
            };

            try
            {
                _dbHelper.ExecuteNonQuery(query, parameters);
                TempData["Success"] = "Employee active state updated successfully.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"State modifications failed: {ex.Message}";
            }
            return RedirectToAction("Index");
        }

        // 4. PURGE/DELETE EMPLOYEE RECORD
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int employeeId)
        {
            string query = "DELETE FROM Employees WHERE EmployeeID = @EmployeeID";
            
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@EmployeeID", SqlDbType.Int) { Value = employeeId }
            };

            try
            {
                _dbHelper.ExecuteNonQuery(query, parameters);
                TempData["Success"] = "Record successfully purged from relational nodes.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Purge rejected due to relational foreign key constraints: {ex.Message}";
            }
            return RedirectToAction("Index");
        }
    }
}
