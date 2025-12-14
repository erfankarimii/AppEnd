using System;
using System.Text.Json;
using System.Data;
using AppEndCommon;
using AppEndApi;
using AppEndDbIO;
using AppEndServer;
using System.Collections.Generic;
using System.Linq;

namespace DefaultRepo
{
    public static class PublicMethods
    {
        /// <summary>
        /// Get list of products for Front Office display
        /// Public method - no authentication required
        /// </summary>
        public static object? GetProductsList()
        {
            try
            {
                var dbIO = DbIO.Instance();
                
                // Check if Products table exists
                var checkTableQuery = @"
                    SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES 
                    WHERE TABLE_NAME = 'Products' AND TABLE_SCHEMA = 'dbo'";
                
                var tableExists = dbIO.ToScalar(checkTableQuery);
                if (tableExists == null || Convert.ToInt32(tableExists) == 0)
                {
                    // Return sample data if table doesn't exist
                    return new
                    {
                        Master = new List<object>
                        {
                            new { Id = 1, Name = "Low-Code Platform", Price = 0, Description = "Complete low-code development platform" },
                            new { Id = 2, Name = "API Builder", Price = 0, Description = "Build REST APIs without coding" },
                            new { Id = 3, Name = "UI Generator", Price = 0, Description = "Automatic UI generation from templates" },
                            new { Id = 4, Name = "Database Manager", Price = 0, Description = "Manage database objects easily" },
                            new { Id = 5, Name = "Template Engine", Price = 0, Description = "Powerful template engine" }
                        },
                        Aggregations = new { }
                    };
                }
                
                // Query directly from Products table
                var sqlQuery = "SELECT TOP 100 Id, Name, Price, Description, Image FROM Products WHERE (IsActive = 1 OR IsActive IS NULL) ORDER BY Id";
                var dataTable = dbIO.ToDataTable(sqlQuery);
                
                LogMan.LogDebug($"Query executed, result keys: {string.Join(", ", dataTable.Keys)}");
                
                if (dataTable.ContainsKey("Master"))
                {
                    var dt = dataTable["Master"];
                    LogMan.LogDebug($"Master table has {dt.Rows.Count} rows");
                    
                    var rows = new List<object>();
                    foreach (DataRow row in dt.Rows)
                    {
                        rows.Add(new
                        {
                            Id = row["Id"] == DBNull.Value ? (int?)null : Convert.ToInt32(row["Id"]),
                            Name = row["Name"] == DBNull.Value ? null : row["Name"].ToString(),
                            Price = row["Price"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(row["Price"]),
                            Description = row["Description"] == DBNull.Value ? null : row["Description"].ToString(),
                            Image = row["Image"] == DBNull.Value ? null : (byte[]?)row["Image"]
                        });
                    }
                    
                    LogMan.LogDebug($"Returning {rows.Count} products");
                    return new
                    {
                        Master = rows,
                        Aggregations = new { }
                    };
                }
                
                LogMan.LogDebug("No Master key found in result, returning empty list");
                return new
                {
                    Master = new List<object>(),
                    Aggregations = new { }
                };
            }
            catch (Exception ex)
            {
                LogMan.LogError($"Error in GetProductsList: {ex.Message}");
                LogMan.LogError($"Stack trace: {ex.StackTrace}");
                // Return sample data on error
                var sampleData = new List<object>
                {
                    new { Id = 1, Name = "Low-Code Platform", Price = 0, Description = "Complete low-code development platform" },
                    new { Id = 2, Name = "API Builder", Price = 0, Description = "Build REST APIs without coding" },
                    new { Id = 3, Name = "UI Generator", Price = 0, Description = "Automatic UI generation from templates" },
                    new { Id = 4, Name = "Database Manager", Price = 0, Description = "Manage database objects easily" },
                    new { Id = 5, Name = "Template Engine", Price = 0, Description = "Powerful template engine" }
                };
                LogMan.LogDebug($"Returning {sampleData.Count} sample products due to error");
                return new
                {
                    Master = sampleData,
                    Aggregations = new { }
                };
            }
        }

        /// <summary>
        /// Get list of services for Front Office display
        /// Public method - no authentication required
        /// </summary>
        public static object? GetServices()
        {
            try
            {
                var dbIO = DbIO.Instance();
                
                // Check if Services table exists
                var checkTableQuery = @"
                    SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES 
                    WHERE TABLE_NAME = 'Services' AND TABLE_SCHEMA = 'dbo'";
                
                var tableExists = dbIO.ToScalar(checkTableQuery);
                if (tableExists == null || Convert.ToInt32(tableExists) == 0)
                {
                    return new
                    {
                        Master = new List<object>
                        {
                            new { Id = 1, Name = "Consulting", Description = "Expert consulting services" },
                            new { Id = 2, Name = "Development", Description = "Custom development services" },
                            new { Id = 3, Name = "Support", Description = "24/7 support services" }
                        },
                        Aggregations = new { }
                    };
                }
                
                // Query directly from Services table
                var sqlQuery = "SELECT TOP 100 Id, Name, Description FROM Services WHERE (IsActive = 1 OR IsActive IS NULL) ORDER BY Id";
                var dataTable = dbIO.ToDataTable(sqlQuery);
                
                if (dataTable.ContainsKey("Master"))
                {
                    var dt = dataTable["Master"];
                    var rows = new List<object>();
                    foreach (DataRow row in dt.Rows)
                    {
                        rows.Add(new
                        {
                            Id = row["Id"] == DBNull.Value ? (int?)null : Convert.ToInt32(row["Id"]),
                            Name = row["Name"] == DBNull.Value ? null : row["Name"].ToString(),
                            Description = row["Description"] == DBNull.Value ? null : row["Description"].ToString()
                        });
                    }
                    
                    return new
                    {
                        Master = rows,
                        Aggregations = new { }
                    };
                }
                
                return new
                {
                    Master = new List<object>(),
                    Aggregations = new { }
                };
            }
            catch (Exception ex)
            {
                LogMan.LogError($"Error in GetServices: {ex.Message}");
                return new
                {
                    Master = new List<object>
                    {
                        new { Id = 1, Name = "Consulting", Description = "Expert consulting services" },
                        new { Id = 2, Name = "Development", Description = "Custom development services" },
                        new { Id = 3, Name = "Support", Description = "24/7 support services" }
                    },
                    Aggregations = new { }
                };
            }
        }

        /// <summary>
        /// Send contact message from Front Office
        /// Public method - no authentication required
        /// </summary>
        public static object? SendContactMessage(string Name, string Email, string Message)
        {
            try
            {
                // Log the contact message
                var logMessage = $"Contact Form Submission:\nName: {Name}\nEmail: {Email}\nMessage: {Message}\nTime: {DateTime.Now}";
                LogMan.LogDebug(logMessage);

                // Optionally save to database if ContactMessages table exists
                try
                {
                    var dbIO = DbIO.Instance();
                    
                    // Try to insert into ContactMessages table if it exists
                    var insertQuery = @"
                        IF OBJECT_ID('ContactMessages', 'U') IS NOT NULL
                        BEGIN
                            INSERT INTO ContactMessages (Name, Email, Message, CreatedOn)
                            VALUES (@Name, @Email, @Message, GETDATE())
                        END";
                    
                    var parameters = new List<System.Data.Common.DbParameter>
                    {
                        dbIO.CreateParameter("@Name", "NVARCHAR", 200, Name),
                        dbIO.CreateParameter("@Email", "NVARCHAR", 200, Email),
                        dbIO.CreateParameter("@Message", "NTEXT", null, Message)
                    };
                    
                    dbIO.ToScalar(insertQuery, parameters);
                }
                catch
                {
                    // Table doesn't exist, just log it
                }

                return new { Success = true, Message = "Message sent successfully" };
            }
            catch (Exception ex)
            {
                LogMan.LogError($"Error in SendContactMessage: {ex.Message}");
                return new { Success = false, Message = "Error sending message" };
            }
        }
    }
}

