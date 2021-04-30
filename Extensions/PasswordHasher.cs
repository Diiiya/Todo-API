using System;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace ToDoAPI.PasswordHasher
{
    public class PasswordHasher
    {
        byte[] salt = new byte[128 / 8];
        
        public string hashPass(string password)
        {
            string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 1000,
                numBytesRequested: 256 / 8
            ));
            return hashedPassword;
        }
        
        public bool VerifyPassword(string storedHash, string enteredPassword )
        {
            var hashOfEntered = hashPass(enteredPassword);
            return hashOfEntered == storedHash;
        }
    }
}