using AuthorizationService.Models;
using AuthorizationService.Utilities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace AuthorizationService.Data;
public class DataOperations
{
    private SqlConnection _connection;
    private readonly ILogger _logger;

    public DataOperations(string dbConnection, ILogger logger)
    {  
        _connection = new SqlConnection(Environment.GetEnvironmentVariable($"ConnectionStrings:{dbConnection}"));
        _logger = logger;
    }

    public void SaveMessage(User user)
    {
        _connection.Open();
        try
        {
            string insert = Utility.INSERT;
            using (SqlCommand command = new SqlCommand(
                    insert, _connection))
            {
                command.Parameters.Add("@Id", SqlDbType.VarChar).Value = Guid.NewGuid().ToString();
                command.Parameters.Add("@FirstName", SqlDbType.VarChar).Value = user.FirstName;
                command.Parameters.Add("@LastName", SqlDbType.VarChar).Value = user.LastName;
                command.Parameters.Add("@Address", SqlDbType.VarChar).Value = user.Address;
                command.Parameters.Add("@City", SqlDbType.VarChar).Value = user.City;
                command.Parameters.Add("@State", SqlDbType.VarChar).Value = user.State;
                command.Parameters.Add("@PostalCode", SqlDbType.VarChar).Value = user.PostalCode;
                command.Parameters.Add("@Phone", SqlDbType.VarChar).Value = user.Phone;
                command.Parameters.Add("@Email", SqlDbType.VarChar).Value = user.Email;
                command.Parameters.Add("@Username", SqlDbType.VarChar).Value = user.Username;
                command.Parameters.Add("@Password", SqlDbType.VarChar).Value = user.Password;
                command.ExecuteNonQuery();
            }
            

        }
        catch (Exception exc)
        {
            _logger.LogError($"Could not save to the Database {exc.Message}");
            throw;
        }
    }
}
