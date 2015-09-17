using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RM.MailshotsOnline.PCL.Services
{
    public interface ICryptographicService
    {
        string EncryptEmailAddress(string email);

        string GenerateEmailSalt(string email);

        string Encrypt(string input, byte[] salt);

        string Encrypt(string input, string salt);

        string Decrypt(string input, byte[] salt);

        string Decrypt(string input, string salt);

        string GenerateRandomSaltB64();

        byte[] GenerateRandomSaltBytes();
    }
}
