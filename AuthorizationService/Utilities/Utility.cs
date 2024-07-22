using AuthorizationService.Models;

namespace AuthorizationService.Utilities;
public static class Utility
{
    public static string INSERT = $"INSERT INTO REGISTER (Id, FirstName ,LastName, Address, City, " +
            $"State, PostalCode, Phone, Email, Username, Password) Values (@Id, " +
            $"@FirstName,@LastName, @Address, @City, @State," +
            $"@PostalCode, @Phone, @Email, @Username, @Password)";
   
}
