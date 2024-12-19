using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.IO;
using Core.ExceptionLog;

namespace Core.Database
{
    public class SqlDatabaseManager : IDisposable
    {
        private static SqlDatabaseManager _instance = null;
        private static readonly object _lock = new object();
        private SqlConnection _connection;
        private bool _disposed = false;
        FileExceptionLog exceptionLogger = new FileExceptionLog("log.txt");

        private SqlDatabaseManager(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
        }
		
        // Constructor initializes the connection using the parameters
        public SqlDatabaseManager(string serverName, string databaseName, string userName, string password)
        {
			lock (_lock) 
            {
                if (_instance == null)
                {
					string connectionString = $"Server={serverName};Database={databaseName};User Id={userName};Password={password};";
					_connection = new SqlConnection(connectionString);
					_instance = new SqlDatabaseManager(connectionString);
				}
			}
        }

   //     // Constructor that initializes using a configuration file
   //     public SqlDatabaseManager(string configFilePath)
   //     {
			//lock (_lock) 
   //         {
   //             if (_instance == null)
   //             {
			//		if (!File.Exists(configFilePath))
			//			throw new FileNotFoundException("Configuration file not found");
		
			//		var lines = File.ReadAllLines(configFilePath);
			//		string serverName = lines[0];
			//		string databaseName = lines[1];
			//		string userName = lines[2];
			//		string password = lines[3];
		
			//		string connectionString = $"Server={serverName};Database={databaseName};User Id={userName};Password={password};";
			//		_connection = new SqlConnection(connectionString);
			//		_instance = new SqlDatabaseManager(connectionString);
			//	}
			//	return _instance;
			//}
   //     }

        // Bind SQL parameters to the SQL command
        private bool BindParameters(SqlCommand command, List<SqlParameter> parameters)
        {
            try
            {
                foreach (var parameter in parameters)
                {
                    command.Parameters.Add(parameter);
                }
                return true;
            }
            catch (Exception ex)
            {
                var logResult = exceptionLogger.LogException(ex);
                Console.WriteLine($"Error binding parameters: {logResult}");
                return false;
            }
        }

        // Execute a SELECT query
        public DataTable ExecuteSelectQuery(string sql, List<SqlParameter> parameters = null)
        {
            DataTable dataTable = new DataTable();
            try
            {
                using (SqlCommand command = new SqlCommand(sql, _connection))
                {
                    if (parameters != null && !BindParameters(command, parameters))
                        throw new Exception("Error binding parameters");

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(dataTable);
                }
            }
            catch (Exception ex)
            {
                var logResult = exceptionLogger.LogException(ex);
                Console.WriteLine($"Error executing SELECT query: {logResult}");
            }
            return dataTable;
        }

        // Execute an INSERT query
        public int ExecuteInsertQuery(string sql, List<SqlParameter> parameters)
        {
            return ExecuteNonQuery(sql, parameters);
        }

        // Execute an UPDATE query
        public int ExecuteUpdateQuery(string sql, List<SqlParameter> parameters)
        {
            return ExecuteNonQuery(sql, parameters);
        }

        // Execute a DELETE query
        public int ExecuteDeleteQuery(string sql, List<SqlParameter> parameters)
        {
            return ExecuteNonQuery(sql, parameters);
        }

        // Generic function to execute non-query SQL commands (INSERT, UPDATE, DELETE)
        private int ExecuteNonQuery(string sql, List<SqlParameter> parameters)
        {
            int affectedRows = 0;
            try
            {
                using (SqlCommand command = new SqlCommand(sql, _connection))
                {
                    if (parameters != null && !BindParameters(command, parameters))
                        throw new Exception("Error binding parameters");

                    _connection.Open();
                    affectedRows = command.ExecuteNonQuery();
                    _connection.Close();
                }
            }
            catch (Exception ex)
            {
                var logResult = exceptionLogger.LogException(ex);
                Console.WriteLine($"Error executing non-query: {logResult}");
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }
            return affectedRows;
        }

        // Retrieve the SQL Server date
        public DateTime GetSqlServerDate()
        {
            DateTime serverDate = DateTime.MinValue;
            try
            {
                string sql = "SELECT GETDATE()";
                using (SqlCommand command = new SqlCommand(sql, _connection))
                {
                    _connection.Open();
                    serverDate = (DateTime)command.ExecuteScalar();
                    _connection.Close();
                }
            }
            catch (Exception ex)
            {
                var logResult = exceptionLogger.LogException(ex);
                Console.WriteLine($"Error retrieving server date: {logResult}");
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }
            return serverDate;
        }

        // Dispose method to free resources
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected dispose pattern implementation
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Dispose managed resources
                    if (_connection != null)
                    {
                        _connection.Dispose();
                        _connection = null;
                    }
                }
                _disposed = true;
            }
        }

        // Destructor to ensure Dispose gets called
        ~SqlDatabaseManager()
        {
            Dispose(false);
        }
    }
}
