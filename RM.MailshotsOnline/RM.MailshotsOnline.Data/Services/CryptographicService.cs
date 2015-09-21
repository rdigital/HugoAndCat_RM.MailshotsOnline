using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using HC.RM.Common;
using RM.MailshotsOnline.PCL.Services;

namespace RM.MailshotsOnline.Data.Services
{
    public class CryptographicService : ICryptographicService
    {
        private static readonly Encoding Encoding = Encoding.UTF8;
        private static readonly string EncryptionKey = Constants.Constants.Encryption.EncryptionKey;
        private static readonly RNGCryptoServiceProvider RandomNumberGenerator = new RNGCryptoServiceProvider();


        public CryptographicService()
        {
            
        }

        public string EncryptEmailAddress(string email)
        {
            return Encrypt(email, GenerateEmailSalt(email));
        }

        public string GenerateEmailSalt(string email)
        {
            email = email.ToLower();

            var salt = Encryption.ComputedSalt(email, email);
            var saltBytes = Encoding.GetBytes(salt);

            return Convert.ToBase64String(saltBytes);
        }

        public string Encrypt(string input, string salt)
        {
            var saltBytes = Convert.FromBase64String(salt);

            return Encryption.Encrypt(input, EncryptionKey, saltBytes);
        }

        public string Encrypt(string input, byte[] salt)
        {
            return Encryption.Encrypt(input, EncryptionKey, salt);
        }

        public string Decrypt(string input, string salt)
        {
            var saltBytes = Convert.FromBase64String(salt);

            return Decrypt(input, saltBytes);
        }

        public string Decrypt(string input, byte[] salt)
        {
            return Encryption.Decrypt(input, EncryptionKey, salt);
        }

        public string GenerateRandomSaltB64()
        {
            return Convert.ToBase64String(GenerateRandomSaltBytes());
        }

        public byte[] GenerateRandomSaltBytes()
        {
            var salt = new byte[16];
            RandomNumberGenerator.GetBytes(salt);

            return salt;
        }
    }
}
