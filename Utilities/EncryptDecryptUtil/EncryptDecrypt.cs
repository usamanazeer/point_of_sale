using System;
using System.Security.Cryptography;
using System.Text;

namespace Utilities.EncryptDecryptUtil
{
    public class EncryptDecrypt
    {
        private const string SPwd = "@@$@#$#@";


        /// <summary>
        ///     Encrypt a string using dual encryption method. Return a encrypted cipher Text
        /// </summary>
        /// <param name="toEncrypt">string to be encrypted</param>
        /// <param name="useHashing">use hashing? send to for extra security</param>
        /// <returns></returns>
        public static string Encrypt(string toEncrypt,
                                     bool useHashing)
        {
            byte[] keyArray;
            var toEncryptArray = Encoding.UTF8.GetBytes(s: toEncrypt);

            //System.Configuration.AppSettingsReader settingsReader = new AppSettingsReader();
            // Get the key from config file
            var key = SPwd; // (string)settingsReader.GetValue("SecurityKey", typeof(String));
            //System.Windows.Forms.MessageBox.Show(key);
            if (useHashing)
            {
                var hashMd5 = new MD5CryptoServiceProvider();
                keyArray = hashMd5.ComputeHash(buffer: Encoding.UTF8.GetBytes(s: key));
                hashMd5.Clear();
            }
            else
            {
                keyArray = Encoding.UTF8.GetBytes(s: key);
            }

            var tripleDesCryptoServiceProvider = new TripleDESCryptoServiceProvider
                       {
                           Key = keyArray,
                           Mode = CipherMode.ECB,
                           Padding = PaddingMode.PKCS7
                       };

            var cTransform = tripleDesCryptoServiceProvider.CreateEncryptor();
            var resultArray = cTransform.TransformFinalBlock(inputBuffer: toEncryptArray,
                                                             inputOffset: 0,
                                                             inputCount: toEncryptArray.Length);
            tripleDesCryptoServiceProvider.Clear();
            return Convert.ToBase64String(inArray: resultArray,
                                          offset: 0,
                                          length: resultArray.Length);
        }


        /// <summary>
        ///     DeCrypt a string using dual encryption method. Return a DeCrypted clear string
        /// </summary>
        /// <param name="cipherString">encrypted string</param>
        /// <param name="useHashing">Did you use hashing to encrypt this data? pass true is yes</param>
        /// <returns></returns>
        public static string Decrypt(string cipherString,
                                     bool useHashing)
        {
            byte[] keyArray;
            var toEncryptArray = Convert.FromBase64String(s: cipherString);

            //System.Configuration.AppSettingsReader settingsReader = new AppSettingsReader();
            //Get your key from config file to open the lock!
            const string key = SPwd; // (string)settingsReader.GetValue("SecurityKey", typeof(String));

            if (useHashing)
            {
                var hashMd5 = new MD5CryptoServiceProvider();
                keyArray = hashMd5.ComputeHash(buffer: Encoding.UTF8.GetBytes(s: key));
                hashMd5.Clear();
            }
            else
            {
                keyArray = Encoding.UTF8.GetBytes(s: key);
            }

            var tripleDesCryptoServiceProvider = new TripleDESCryptoServiceProvider
                       {
                           Key = keyArray,
                           Mode = CipherMode.ECB,
                           Padding = PaddingMode.PKCS7
                       };

            var cTransform = tripleDesCryptoServiceProvider.CreateDecryptor();
            var resultArray = cTransform.TransformFinalBlock(inputBuffer: toEncryptArray,
                                                             inputOffset: 0,
                                                             inputCount: toEncryptArray.Length);

            tripleDesCryptoServiceProvider.Clear();
            return Encoding.UTF8.GetString(bytes: resultArray);
        }
    }
}