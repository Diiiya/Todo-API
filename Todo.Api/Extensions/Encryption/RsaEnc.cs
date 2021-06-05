// using System;
// using System.IO;
// using System.Security.Cryptography;
// using System.Text;
// using System.Xml.Serialization;

// namespace Todo.Api.Extensions.Encryption
// {
//     public class RsaEnc
//     {
//         private static RSACryptoServiceProvider csp = new RSACryptoServiceProvider(2048);//keysize
//         private RSAParameters _privateKey;
//         private RSAParameters _publicKey;

//         public RsaEnc()
//         {
//             _privateKey = csp.ExportParameters(true); 
//             _publicKey = csp.ExportParameters(false); //we are not exporting params
//         }

//         public string PublicKeyString(){
//             var sw = new StringWriter();
//             var xs = new XmlSerializer(typeof(RSAParameters));
//             xs.Serialize(sw, _publicKey);
//             return sw.ToString();
//         }
//         public string PrivateKeyString(){
//             var sw = new StringWriter();
//             var xs = new XmlSerializer(typeof(RSAParameters));
//             xs.Serialize(sw, _privateKey);
//             return sw.ToString();
//         }

//         public string Encrypt(string plainText){
//             csp = new RSACryptoServiceProvider();
//             csp.ImportParameters(_publicKey);

//             var data = Encoding.Unicode.GetBytes(plainText);
//             var cypher =  csp.Encrypt(data, false);
//             return Convert.ToBase64String(cypher);
//         }

//         public string Derypt(string cypherText){
//             var dataBytes = Convert.FromBase64String(cypherText);
//             csp.ImportParameters(_privateKey);
//             var plainText = csp.Decrypt(dataBytes, false);
//             return Encoding.Unicode.GetString(plainText);
//         }
//     }
// }