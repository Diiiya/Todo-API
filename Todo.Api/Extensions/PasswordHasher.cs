using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Todo.Api.Extensions
{
    public class PasswordHasher
    {
        private static int saltLengthLimit = 30;
        private static byte[] GetSalt(int maximumSaltLength)
        {
            var salt = new byte[maximumSaltLength];
            using (var random = new RNGCryptoServiceProvider())
            {
                random.GetNonZeroBytes(salt);
            }
            return salt;
        }

        public string hashPass(string password, byte[] salt)
        {
            byte[] getSalt;
            if(salt.Length == 0){
                getSalt = GetSalt(saltLengthLimit);
            } else {
                getSalt = salt;
            }
            string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: getSalt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 1000,
                numBytesRequested: 45
            ));
            var myHash = hashedPassword + Convert.ToBase64String(getSalt);
            return myHash;
        }

        public bool VerifyPassword(string storedHash, string enteredPassword)
        {
            var passwordHash = storedHash.Substring(0, 60);
            var salt = Convert.FromBase64String(storedHash.Substring(60));
            var hashOfEntered = hashPass(enteredPassword, salt);
            return hashOfEntered == storedHash;
        }
    }
}