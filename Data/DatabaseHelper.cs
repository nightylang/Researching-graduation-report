using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;

namespace HR_Workspace_System.Data
{
    public class DatabaseHelper
    {
        private readonly string _connectionString;

        // 1. Core Dependency Injection fetches connection settings automatically from appsettings.json
        public DatabaseHelper(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("WorkspaceEnterpriseDB") 
                ?? throw new InvalidOperationException("Infrastructure Exception: Target connection string is unassigned.");
        }

        // 2. Optimized ExecuteNonQuery for CREATE, UPDATE, and DELETE operations
        public int ExecuteNonQuery(string query, SqlParameter[] parameters = null)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    try
                    {
                        connection.Open();
                        return command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Database Engine Error during non-query execution: {ex.Message}", ex);
                    }
                }
            }
        }

        // 3. Optimized ExecuteReader to fetch DataTables for READ operations
        public DataTable ExecuteReader(string query, SqlParameter[] parameters = null)
        {
            DataTable dataTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    try
                    {
                        connection.Open();
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            adapter.Fill(dataTable);
                        }
                        return dataTable;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Database Engine Error during read retrieval: {ex.Message}", ex);
                    }
                }
            }
        }
    }
}
