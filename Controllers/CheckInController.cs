using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using HR_Workspace_System.Data;

namespace HR_Workspace_System.Controllers
{
    public class CheckInController : Controller
    {
        private readonly DatabaseHelper _dbHelper;
        private readonly string _approvedOfficeSubnet = "192.168.10."; 

        // 1. Centralized dependency injection pattern
        public CheckInController(DatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Submit(int employeeId, int shiftId, string deviceId)
        {
            // 2. Extrapolate incoming connection IP telemetry
            string clientIpAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";

            // 3. Security Subnet Rules Evaluation
            string verificationStatus = "Verified";
            if (!clientIpAddress.StartsWith(_approvedOfficeSubnet) && clientIpAddress != "::1") 
            {
                verificationStatus = "Flagged_Offsite"; 
            }

            // 4. Construct SQL Parameters Array safely for the helper
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@ShiftID", SqlDbType.Int) { Value = shiftId },
                new SqlParameter("@EmployeeID", SqlDbType.Int) { Value = employeeId },
                new SqlParameter("@NetworkIP", SqlDbType.VarChar) { Value = clientIpAddress },
                new SqlParameter("@DeviceIdentifier", SqlDbType.VarChar) { Value = deviceId ?? "Web_Browser" },
                new SqlParameter("@VerificationStatus", SqlDbType.VarChar) { Value = verificationStatus }
            };

            string query = @"INSERT INTO CheckInLogs (ShiftID, EmployeeID, CheckInTime, NetworkIP, DeviceIdentifier, VerificationStatus) 
                             VALUES (@ShiftID, @EmployeeID, GETDATE(), @NetworkIP, @DeviceIdentifier, @VerificationStatus);";

            try
            {
                // 5. Execute using the clean abstraction layer
                _dbHelper.ExecuteNonQuery(query, parameters);
                
                if (verificationStatus == "Flagged_Offsite")
                {
                    TempData["Warning"] = "Check-in logged, but flagged! You are not on office Wi-Fi.";
                }
                else
                {
                    TempData["Success"] = "Check-in successful and verified on office network!";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Central Infrastructure Handshake Error: {ex.Message}";
            }

            return RedirectToAction("Index");
        }
    }
}
