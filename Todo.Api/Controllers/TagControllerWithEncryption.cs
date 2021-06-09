using System.Linq;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Todo.Api.DTOs;
using Todo.Api.Models;
using System.Threading.Tasks;
using Todo.Api.Interfaces;
using Todo.Api.Extensions.Encryption;
using System.Text;
using System.Security.Cryptography;
// using Newtonsoft.Json.Linq;

namespace Todo.Api.Controllers
{
    // https://www.c-sharpcorner.com/article/encryption-and-decryption-using-a-symmetric-key-in-c-sharp/
    // [Authorize]
    [ApiController]
    [Route("encryption")]
    public class TagEncryptionController : ControllerBase
    {
        private readonly ITagRepo tagRepo;

        public TagEncryptionController(ITagRepo tagRepo)
        {
            this.tagRepo = tagRepo;
        }
        static byte[] encryptedData;
        private static string encripted;
        void setEncrypted(string myEnc)
        {
            encripted = myEnc;
        }
        string getEncrytpted()
        {
            return encripted;
        }
        byte[] decryptedData = new byte[] { };
        RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
        // RSAParameters parameters = RSA.ExportParameters(false);

        [HttpGet]
        public async Task<ActionResult<string>> GetAllTagsAsync()
        {
            var key = "b14ca5898a4e4133bbce2ea2315a1916";  //can be stored hashed in the db
            // string str = "{\"key\":\"secret_value\"}";
            string str = "Izabela";
            var encryptedString = SymmetricEncryption.EncryptString(key, str); 
            var decryptedString = SymmetricEncryption.DecryptString(key, encryptedString);
            return Ok(decryptedString + "\n" + encryptedString);
        }

        public static byte[] RSAEncrypt(byte[] DataToEncrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
        {
            try
            {
                byte[] encryptedData2;
                //Create a new instance of RSACryptoServiceProvider.
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {

                    //Import the RSA Key information. This only needs
                    //toinclude the public key information.
                    RSA.ImportParameters(RSAKeyInfo);

                    //Encrypt the passed byte array and specify OAEP padding.  
                    //OAEP padding is only available on Microsoft Windows XP or
                    //later.  
                    encryptedData2 = RSA.Encrypt(DataToEncrypt, DoOAEPPadding);
                }
                return encryptedData2;
            }
            //Catch and display a CryptographicException  
            //to the console.
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);

                return null;
            }
        }

        public static byte[] RSADecrypt(byte[] DataToDecrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
        {
            try
            {
                byte[] decryptedData2;
                //Create a new instance of RSACryptoServiceProvider.
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    //Import the RSA Key information. This needs
                    //to include the private key information.
                    RSA.ImportParameters(RSAKeyInfo);

                    //Decrypt the passed byte array and specify OAEP padding.  
                    //OAEP padding is only available on Microsoft Windows XP or
                    //later.  
                    decryptedData2 = RSA.Decrypt(DataToDecrypt, DoOAEPPadding);
                }
                return decryptedData2;
            }
            //Catch and display a CryptographicException  
            //to the console.
            catch (CryptographicException e)
            {
                Console.WriteLine(e.ToString());

                return null;
            }
        }
    }
}