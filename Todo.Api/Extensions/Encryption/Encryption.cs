// using System;
// using System.IO;
// using System.Security.Cryptography;

// namespace Todo.Api.Extensions.Encryption
// {
//     public class Encryption
//     {
//         //https://docs.microsoft.com/en-us/dotnet/standard/security/walkthrough-creating-a-cryptographic-application


//         // Declare CspParmeters and RsaCryptoServiceProvider
//         // objects with global scope of your Form class.
//         CspParameters cspp = new CspParameters();
//         RSACryptoServiceProvider rsa;

//         // Path variables for source, encryption, and
//         // decryption folders. Must end with a backslash.
//         const string EncrFolder = @"c:\Encrypt\";
//         const string DecrFolder = @"c:\Decrypt\";
//         const string SrcFolder = @"c:\docs\";

//         // Public key file
//         const string PubKeyFile = @"c:\encrypt\rsaPublicKey.txt";

//         // Key container name for
//         // private/public key value pair.
//         const string keyName = "Key01";

//         private void CreateAsmKeys()
//         {
//             // Stores a key pair in the key container.
//             cspp.KeyContainerName = keyName;
//             rsa = new RSACryptoServiceProvider(cspp);
//             rsa.PersistKeyInCsp = true;
//             if (rsa.PublicOnly == true)
//                 System.Console.WriteLine("Key: " + cspp.KeyContainerName + " - Public Only");
//             else
//                 System.Console.WriteLine("Key: " + cspp.KeyContainerName + " - Full Key Pair");
//         }

//         // private void EncryptFile()
//         // {
//         //     if (rsa == null)
//         //     {
//         //         System.Console.WriteLine("Key not set.");
//         //     }
//         //     else
//         //     {

//         //         // Display a dialog box to select a file to encrypt.
//         //         openFileDialog1.InitialDirectory = SrcFolder;
//         //         if (openFileDialog1.ShowDialog() == DialogResult.OK)
//         //         {
//         //             string fName = openFileDialog1.FileName;
//         //             if (fName != null)
//         //             {
//         //                 FileInfo fInfo = new FileInfo(fName);
//         //                 // Pass the file name without the path.
//         //                 string name = fInfo.FullName;
//         //                 EncryptFile(name);
//         //             }
//         //         }
//         //     }
//         // }

//         private void EncryptFile(string inFile)
//         {

//             // Create instance of Aes for
//             // symmetric encryption of the data.
//             Aes aes = Aes.Create();
//             ICryptoTransform transform = aes.CreateEncryptor();

//             // Use RSACryptoServiceProvider to
//             // encrypt the AES key.
//             // rsa is previously instantiated:
//             //    rsa = new RSACryptoServiceProvider(cspp);
//             byte[] keyEncrypted = rsa.Encrypt(aes.Key, false);

//             // Create byte arrays to contain
//             // the length values of the key and IV.
//             byte[] LenK = new byte[4];
//             byte[] LenIV = new byte[4];

//             int lKey = keyEncrypted.Length;
//             LenK = BitConverter.GetBytes(lKey);
//             int lIV = aes.IV.Length;
//             LenIV = BitConverter.GetBytes(lIV);

//             // Write the following to the FileStream
//             // for the encrypted file (outFs):
//             // - length of the key
//             // - length of the IV
//             // - ecrypted key
//             // - the IV
//             // - the encrypted cipher content

//             int startFileName = inFile.LastIndexOf("\\") + 1;
//             // Change the file's extension to ".enc"
//             string outFile = EncrFolder + inFile.Substring(startFileName, inFile.LastIndexOf(".") - startFileName) + ".enc";

//             using (FileStream outFs = new FileStream(outFile, FileMode.Create))
//             {

//                 outFs.Write(LenK, 0, 4);
//                 outFs.Write(LenIV, 0, 4);
//                 outFs.Write(keyEncrypted, 0, lKey);
//                 outFs.Write(aes.IV, 0, lIV);

//                 // Now write the cipher text using
//                 // a CryptoStream for encrypting.
//                 using (CryptoStream outStreamEncrypted = new CryptoStream(outFs, transform, CryptoStreamMode.Write))
//                 {

//                     // By encrypting a chunk at
//                     // a time, you can save memory
//                     // and accommodate large files.
//                     int count = 0;
//                     int offset = 0;

//                     // blockSizeBytes can be any arbitrary size.
//                     int blockSizeBytes = aes.BlockSize / 8;
//                     byte[] data = new byte[blockSizeBytes];
//                     int bytesRead = 0;

//                     using (FileStream inFs = new FileStream(inFile, FileMode.Open))
//                     {
//                         do
//                         {
//                             count = inFs.Read(data, 0, blockSizeBytes);
//                             offset += count;
//                             outStreamEncrypted.Write(data, 0, count);
//                             bytesRead += blockSizeBytes;
//                         }
//                         while (count > 0);
//                         inFs.Close();
//                     }
//                     outStreamEncrypted.FlushFinalBlock();
//                     outStreamEncrypted.Close();
//                 }
//                 outFs.Close();
//             }
//         }

//         // private void buttonDecryptFile_Click(object sender, EventArgs e)
//         // {
//         //     if (rsa == null)
//         //     {
//         //        System.Console.WriteLine("Key not set.");
//         //     }
//         //     else
//         //     {
//         //         // Display a dialog box to select the encrypted file.
//         //         openFileDialog2.InitialDirectory = EncrFolder;
//         //         if (openFileDialog2.ShowDialog() == DialogResult.OK)
//         //         {
//         //             string fName = openFileDialog2.FileName;
//         //             if (fName != null)
//         //             {
//         //                 FileInfo fi = new FileInfo(fName);
//         //                 string name = fi.Name;
//         //                 DecryptFile(name);
//         //             }
//         //         }
//         //     }
//         // }

//         private void DecryptFile(string inFile)
//         {

//             // Create instance of Aes for
//             // symetric decryption of the data.
//             Aes aes = Aes.Create();

//             // Create byte arrays to get the length of
//             // the encrypted key and IV.
//             // These values were stored as 4 bytes each
//             // at the beginning of the encrypted package.
//             byte[] LenK = new byte[4];
//             byte[] LenIV = new byte[4];

//             // Construct the file name for the decrypted file.
//             string outFile = DecrFolder + inFile.Substring(0, inFile.LastIndexOf(".")) + ".txt";

//             // Use FileStream objects to read the encrypted
//             // file (inFs) and save the decrypted file (outFs).
//             using (FileStream inFs = new FileStream(EncrFolder + inFile, FileMode.Open))
//             {

//                 inFs.Seek(0, SeekOrigin.Begin);
//                 inFs.Seek(0, SeekOrigin.Begin);
//                 inFs.Read(LenK, 0, 3);
//                 inFs.Seek(4, SeekOrigin.Begin);
//                 inFs.Read(LenIV, 0, 3);

//                 // Convert the lengths to integer values.
//                 int lenK = BitConverter.ToInt32(LenK, 0);
//                 int lenIV = BitConverter.ToInt32(LenIV, 0);

//                 // Determine the start postition of
//                 // the ciphter text (startC)
//                 // and its length(lenC).
//                 int startC = lenK + lenIV + 8;
//                 int lenC = (int)inFs.Length - startC;

//                 // Create the byte arrays for
//                 // the encrypted Aes key,
//                 // the IV, and the cipher text.
//                 byte[] KeyEncrypted = new byte[lenK];
//                 byte[] IV = new byte[lenIV];

//                 // Extract the key and IV
//                 // starting from index 8
//                 // after the length values.
//                 inFs.Seek(8, SeekOrigin.Begin);
//                 inFs.Read(KeyEncrypted, 0, lenK);
//                 inFs.Seek(8 + lenK, SeekOrigin.Begin);
//                 inFs.Read(IV, 0, lenIV);
//                 Directory.CreateDirectory(DecrFolder);
//                 // Use RSACryptoServiceProvider
//                 // to decrypt the AES key.
//                 byte[] KeyDecrypted = rsa.Decrypt(KeyEncrypted, false);

//                 // Decrypt the key.
//                 ICryptoTransform transform = aes.CreateDecryptor(KeyDecrypted, IV);

//                 // Decrypt the cipher text from
//                 // from the FileSteam of the encrypted
//                 // file (inFs) into the FileStream
//                 // for the decrypted file (outFs).
//                 using (FileStream outFs = new FileStream(outFile, FileMode.Create))
//                 {

//                     int count = 0;
//                     int offset = 0;

//                     // blockSizeBytes can be any arbitrary size.
//                     int blockSizeBytes = aes.BlockSize / 8;
//                     byte[] data = new byte[blockSizeBytes];

//                     // By decrypting a chunk a time,
//                     // you can save memory and
//                     // accommodate large files.

//                     // Start at the beginning
//                     // of the cipher text.
//                     inFs.Seek(startC, SeekOrigin.Begin);
//                     using (CryptoStream outStreamDecrypted = new CryptoStream(outFs, transform, CryptoStreamMode.Write))
//                     {
//                         do
//                         {
//                             count = inFs.Read(data, 0, blockSizeBytes);
//                             offset += count;
//                             outStreamDecrypted.Write(data, 0, count);
//                         }
//                         while (count > 0);

//                         outStreamDecrypted.FlushFinalBlock();
//                         outStreamDecrypted.Close();
//                     }
//                     outFs.Close();
//                 }
//                 inFs.Close();
//             }
//         }

//         void ExportPublicKey()
//         {
//             // Save the public key created by the RSA
//             // to a file. Caution, persisting the
//             // key to a file is a security risk.
//             Directory.CreateDirectory(EncrFolder);
//             StreamWriter sw = new StreamWriter(PubKeyFile, false);
//             sw.Write(rsa.ToXmlString(false));
//             sw.Close();
//         }

//         void ImportPublicKey()
//         {
//             StreamReader sr = new StreamReader(PubKeyFile);
//             cspp.KeyContainerName = keyName;
//             rsa = new RSACryptoServiceProvider(cspp);
//             string keytxt = sr.ReadToEnd();
//             rsa.FromXmlString(keytxt);
//             rsa.PersistKeyInCsp = true;
//             if (rsa.PublicOnly == true)
//                 System.Console.WriteLine("Key: " + cspp.KeyContainerName + " - Public Only");
//             else
//                 System.Console.WriteLine("Key: " + cspp.KeyContainerName + " - Full Key Pair");
//             sr.Close();
//         }

//         private void GetPrivateKey()
//         {
//             cspp.KeyContainerName = keyName;

//             rsa = new RSACryptoServiceProvider(cspp);
//             rsa.PersistKeyInCsp = true;

//             if (rsa.PublicOnly == true)
//                 System.Console.WriteLine("Key: " + cspp.KeyContainerName + " - Public Only");
//             else
//                 System.Console.WriteLine("Key: " + cspp.KeyContainerName + " - Full Key Pair");
//         }
//         public void encrypt()
//         {
//             //Initialize the byte arrays to the public key information.
//             byte[] modulus =
//             {
//             214,46,220,83,160,73,40,39,201,155,19,202,3,11,191,178,56,
//             74,90,36,248,103,18,144,170,163,145,87,54,61,34,220,222,
//             207,137,149,173,14,92,120,206,222,158,28,40,24,30,16,175,
//             108,128,35,230,118,40,121,113,125,216,130,11,24,90,48,194,
//             240,105,44,76,34,57,249,228,125,80,38,9,136,29,117,207,139,
//             168,181,85,137,126,10,126,242,120,247,121,8,100,12,201,171,
//             38,226,193,180,190,117,177,87,143,242,213,11,44,180,113,93,
//             106,99,179,68,175,211,164,116,64,148,226,254,172,147
//         };

//             byte[] exponent = { 1, 0, 1 };

//             //Create values to store encrypted symmetric keys.
//             byte[] encryptedSymmetricKey;
//             byte[] encryptedSymmetricIV;

//             //Create a new instance of the RSA class.
//             RSA rsa = RSA.Create();

//             //Create a new instance of the RSAParameters structure.
//             RSAParameters rsaKeyInfo = new RSAParameters();

//             //Set rsaKeyInfo to the public key values.
//             rsaKeyInfo.Modulus = modulus;
//             rsaKeyInfo.Exponent = exponent;

//             //Import key parameters into rsa.
//             rsa.ImportParameters(rsaKeyInfo);

//             //Create a new instance of the default Aes implementation class.
//             Aes aes = Aes.Create();

//             //Encrypt the symmetric key and IV.
//             encryptedSymmetricKey = rsa.Encrypt(aes.Key, RSAEncryptionPadding.Pkcs1);
//             encryptedSymmetricIV = rsa.Encrypt(aes.IV, RSAEncryptionPadding.Pkcs1);
//         }

//         public void createSigmanture()
//         {
//             //The hash value to sign.
//             byte[] hashValue = { 59, 4, 248, 102, 77, 97, 142, 201, 210, 12, 224, 93, 25, 41, 100, 197, 213, 134, 130, 135 };

//             //The value to hold the signed value.
//             byte[] signedHashValue;

//             //Generate a public/private key pair.
//             RSA rsa = RSA.Create();

//             //Create an RSAPKCS1SignatureFormatter object and pass it the
//             //RSA instance to transfer the private key.
//             RSAPKCS1SignatureFormatter rsaFormatter = new RSAPKCS1SignatureFormatter(rsa);

//             //Set the hash algorithm to SHA1.
//             rsaFormatter.SetHashAlgorithm("SHA1");

//             //Create a signature for hashValue and assign it to
//             //signedHashValue.
//             signedHashValue = rsaFormatter.CreateSignature(hashValue);
//         }

//         // public void decrypt()
//         // {
//         //     //Create a new instance of the RSA class.
//         //     RSA rsa = RSA.Create();

//         //     // Export the public key information and send it to a third party.
//         //     // Wait for the third party to encrypt some data and send it back.

//         //     //Decrypt the symmetric key and IV.
//         //     symmetricKey = rsa.Decrypt(encryptedSymmetricKey, RSAEncryptionPadding.Pkcs1);
//         //     symmetricIV = rsa.Decrypt(encryptedSymmetricIV, RSAEncryptionPadding.Pkcs1);
//         // }

//         // public void verifySignature()
//         // {
//         //     RSA rsa = RSA.Create();
//         //     rsa.ImportParameters(rsaKeyInfo);
//         //     RSAPKCS1SignatureDeformatter rsaDeformatter = new RSAPKCS1SignatureDeformatter(rsa);
//         //     rsaDeformatter.SetHashAlgorithm("SHA1");
//         //     if (rsaDeformatter.VerifySignature(hashValue, signedHashValue))
//         //     {
//         //         Console.WriteLine("The signature is valid.");
//         //     }
//         //     else
//         //     {
//         //         Console.WriteLine("The signature is not valid.");
//         //     }
//         // }

//     }
// }