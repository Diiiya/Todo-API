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
            string str = "{\"key\":\"secret_value\"}";
            var encryptedString = SymmetricEncryption.EncryptString(key, str); 
            var decryptedString = SymmetricEncryption.DecryptString(key, encryptedString);
            return Ok(decryptedString);
            // // RsaEnc rsaEnc = new RsaEnc();
            // // System.Console.WriteLine(rsaEnc.PublicKeyString());
            // // System.Console.WriteLine(rsaEnc.PrivateKeyString());
            // //Create a UnicodeEncoder to convert between byte array and string.
            // UnicodeEncoding ByteConverter = new UnicodeEncoding();

            // //Create byte arrays to hold original, encrypted, and decrypted data.
            // byte[] dataToEncrypt = ByteConverter.GetBytes("Data to Encrypt");
            // var parameters = new CspParameters
            // {
            //     KeyContainerName = "myContainer"
            // };

            // //Create a new instance of RSACryptoServiceProvider to generate
            // //public and private key data.
            // using (var rsa = new RSACryptoServiceProvider(parameters))
            // {

            //     //Pass the data to ENCRYPT, the public key information 
            //     //(using RSACryptoServiceProvider.ExportParameters(false),
            //     //and a boolean flag specifying no OAEP padding.

            //     encryptedData = RSAEncrypt(dataToEncrypt, rsa.ExportParameters(false), false);//RSA.ExportParameters(false)
            //     setEncrypted(Convert.ToBase64String(encryptedData));
            //     System.Console.WriteLine(rsa.ToXmlString(true));

            //     //Pass the data to DECRYPT, the private key information 
            //     //(using RSACryptoServiceProvider.ExportParameters(true),
            //     //and a boolean flag specifying no OAEP padding.
            //     // decryptedData = RSADecrypt(encryptedData, RSA.ExportParameters(true), false);

            //     //Display the decrypted plaintext to the console. 
            //     Console.WriteLine("Decrypted plaintext: {0}", ByteConverter.GetString(encryptedData));
            //     return Ok(ByteConverter.GetString(encryptedData));
            // }
            // // // JObject request = JObject.Parse(frombodyrequest);
            // // //     Encryption encryption = new Encryption();
            // //     StoreKeys storeKeys = new StoreKeys();
            // // storeKeys.GenKey_SaveInContainer("myKeyContainer");
            // // // var myKey = Convert.ToBase64String(storeKeys.GetKeyFromContainer("myKeyContainer"));
            // // // var allTags = (await tagRepo.GetAll()).Select(tag => tag.TagAsDTO());
            // // // return allTags;
            // // // Assuming you have your JSON string already
            // // string json = "{\"key\":\"secret_value\"}";


            // // // Get the "pk" request parameter from the http request however you need to

            // // // string base64PublicKey = storeKeys.GetKeyFromContainer("myKeyContainer");//request.getParameter("pk");
            // // // string publicXmlKey = Encoding.ASCII.GetString(Convert.FromBase64String(base64PublicKey));

            // // // TODO: If you want the extra validation, insert "publicXmlKey" into the json value before 
            // // //       converting it to bytes
            // // // var jo = parse(json); jo.pk = publicXmlKey; json = jo.ToString();

            // // // Convert the string to bytes
            // // byte[] jsonBytes = Encoding.ASCII.GetBytes(json);

            // // // Encrypt the json using the public key provided by the client
            // // RSACryptoServiceProvider rsaEncrypt = new RSACryptoServiceProvider();
            // // rsaEncrypt.FromXmlString(storeKeys.GetKeyFromContainer("myKeyContainer"));
            // // byte[] encryptedJsonBytes = rsaEncrypt.Encrypt(jsonBytes, false);

            // // byte[] decrypted = rsaEncrypt.Decrypt(encryptedJsonBytes, false);
            // // string myResponse = Convert.ToBase64String(encryptedJsonBytes);

            // // // Send the encrypted json back to the client
            // // // return encryptedJsonBytes;
            // // return Ok(myResponse);
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

        [HttpGet("{id}")]
        public async Task<ActionResult<TagDTO>> GetTagAsync()
        {
            UnicodeEncoding ByteConverter = new UnicodeEncoding();
            //  byte[] decryptedData;

            //Create a new instance of RSACryptoServiceProvider to generate
            //public and private key data.
            var parameters = new CspParameters
            {
                KeyContainerName = "myContainer"
            };
            using (var rsa = new RSACryptoServiceProvider(parameters))
            {
                Console.WriteLine("encryption: {0}", ByteConverter.GetString(encryptedData));

                //Pass the data to ENCRYPT, the public key information 
                //(using RSACryptoServiceProvider.ExportParameters(false),
                //and a boolean flag specifying no OAEP padding.
                // encryptedData = RSAEncrypt(dataToEncrypt, RSA.ExportParameters(false), false);

                //Pass the data to DECRYPT, the private key information 
                //(using RSACryptoServiceProvider.ExportParameters(true),
                //and a boolean flag specifying no OAEP padding.
                // var enc = getEncrytpted();
                decryptedData = RSADecrypt(encryptedData, rsa.ExportParameters(true), false);

                //Display the decrypted plaintext to the console. 
                Console.WriteLine("Decrypted plaintext: {0}", ByteConverter.GetString(decryptedData));
                return Ok(ByteConverter.GetString(decryptedData));
            }
            // var tag = await tagRepo.Get(id);

            // if (tag is null)
            // {
            //     return NotFound();
            // }

            // return Ok(tag.TagAsDTO());
        }

        [HttpPost]
        public async Task<ActionResult<TagDTO>> CreateTagAsync(CreateTagDTO tag)
        {
            Tag newTag = new()
            {
                Id = Guid.NewGuid(),
                TagName = tag.TagName,
                TagColor = tag.TagColor,
                FkUserId = tag.FkUserId
            };

            var myCreatedEntity = await tagRepo.Add(newTag);

            return CreatedAtAction(nameof(GetTagAsync), new { id = newTag.Id }, newTag.TagAsDTO());

        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateTagAsync(Guid id, CreateTagDTO tag)
        {
            Tag existingTag = await tagRepo.Get(id);

            if (existingTag is null)
            {
                return NotFound();
            }

            Tag updatedTag = existingTag with
            {
                Id = existingTag.Id,
                TagName = tag.TagName,
                TagColor = tag.TagColor,
                FkUserId = existingTag.FkUserId
            };

            await tagRepo.Update(updatedTag);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTagAsync(Guid id)
        {
            Tag existingTag = await tagRepo.Get(id);

            if (existingTag is null)
            {
                return NotFound();
            }

            await tagRepo.Delete(id);

            return NoContent();
        }
    }
}