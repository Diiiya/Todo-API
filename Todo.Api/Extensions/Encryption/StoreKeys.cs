// using System.Security.Cryptography;
// using System;
// using System.Text;

// namespace Todo.Api.Extensions.Encryption
// {
//     public class StoreKeys
//     {
//         public void GenKey_SaveInContainer(string containerName)
//         {
//             // Create the csp parameter (cryptographic service provider) object and set the key container
//             // name used to store the RSA key pair
//             var parameters = new CspParameters
//             {
//                 KeyContainerName = containerName
//             };

//             //create a new instance of RSACryptoServiceProvider that accesses
//             // the key container MyKeyContainerName
//             using var rsa = new RSACryptoServiceProvider(parameters);

//             //display the key info
//             Console.WriteLine($"Key added to container: {rsa.ToXmlString(true)}");
//         }


//         public string GetKeyFromContainer(string containerName)
//         {
//             var parameters = new CspParameters
//             {
//                 KeyContainerName = containerName
//             };
//             using var rsa = new RSACryptoServiceProvider(parameters);
//             Console.WriteLine($"Key retrieved from container : \n {rsa.ToXmlString(true)}");
//             // string oje = rsa.ToString(true);
//             // string v = rsa.ToXmlString(true);
//             // Convert.ToByte(v);
//             // byte[] v1 = Encoding.ASCII.GetBytes(v);
//             return rsa.ToXmlString(true);//Convert.ToBase64String(v1);
//         }

//         public void DeleteKeyFromContainer(string containerName)
//         {
//             // Create the CspParameters object and set the key container
//             // name used to store the RSA key pair.
//             var parameters = new CspParameters
//             {
//                 KeyContainerName = containerName
//             };

//             // Create a new instance of RSACryptoServiceProvider that accesses
//             // the key container.
//             using var rsa = new RSACryptoServiceProvider(parameters)
//             {
//                 // Delete the key entry in the container.
//                 PersistKeyInCsp = false
//             };

//             // Call Clear to release resources and delete the key from the container.
//             rsa.Clear();

//             Console.WriteLine("Key deleted.");
//         }
//     }
// }