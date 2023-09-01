using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TokenAuth.API.Data;
using TokenAuth.API.Models;

namespace TokenAuth.API.UserRepository
{
    public class UserRepo : IDisposable
    {
        ApplicationDbContext dbContext = new ApplicationDbContext();

        public User ValidateUser(string username, string password)
        {
            // Encrypt the password before comparing
            string encryptedPassword = AesEncryption.EncryptString("ThisIsMyKey12345", password);

            return dbContext.Users.FirstOrDefault(user =>
                user.UserName.Equals(username, StringComparison.OrdinalIgnoreCase) &&
                user.Password == encryptedPassword);
        }

        public User GetUserByUsername(string username)
        {
            return dbContext.Users.FirstOrDefault(user => user.UserName.Equals(username, StringComparison.OrdinalIgnoreCase));
        }

        public void Dispose()
        {
            dbContext.Dispose();
        }
    }
}
