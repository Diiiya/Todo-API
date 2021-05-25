using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Todo.Api.Extensions
{
    public class PasswordHasher
    {
        private static byte[] salt = new byte[]{};
        private static int saltLengthLimit = 30;
        private static byte[] GetSalt(int maximumSaltLength)
        {
            salt = new byte[maximumSaltLength];
            using (var random = new RNGCryptoServiceProvider())
            {
                random.GetNonZeroBytes(salt);
            }
            return salt;
        }

        public string hashPass(string password)
        {
            if(salt.Length == 0){
                salt = GetSalt(saltLengthLimit);
            }
            
            string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 1000,
                numBytesRequested: 45
            ));
            var myHash = hashedPassword + Convert.ToBase64String(salt);
            salt = new byte[]{};
            return myHash;
        }

        public bool VerifyPassword(string storedHash, string enteredPassword)
        {
            var saltFromHash = Convert.FromBase64String(storedHash.Substring(60));
            salt = saltFromHash;
            var hashOfEntered = hashPass(enteredPassword);
            return hashOfEntered == storedHash;
        }
    }
}