using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RM.MailshotsOnline.PCL.Services
{
    public interface ICryptographicService
    {
        /// <summary>
        /// Encrypt an email address using the email address itself to generate a salt.
        /// </summary>
        /// <param name="email">The email address</param>
        /// <returns>The encrypted email address</returns>
        string EncryptEmailAddress(string email);

        /// <summary>
        /// Generate a non-random salt for a given email address
        /// </summary>
        /// <param name="email">The email address</param>
        /// <returns>The salt</returns>
        string GenerateEmailSalt(string email);

        /// <summary>
        /// Encrypts a string using the given salt
        /// </summary>
        /// <param name="input"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        string Encrypt(string input, byte[] salt);

        /// <summary>
        /// Encrypts a string using the given salt.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        string Encrypt(string input, string salt);

        /// <summary>
        /// Decrypts a string using the given salt.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        string Decrypt(string input, byte[] salt);

        /// <summary>
        /// Decrypts a string using the given salt.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        string Decrypt(string input, string salt);

        /// <summary>
        /// Generates a random 16 byte salt
        /// </summary>
        /// <returns></returns>
        string GenerateRandomSaltB64();

        /// <summary>
        /// Generates a random 16 byte salt
        /// </summary>
        /// <returns></returns>
        byte[] GenerateRandomSaltBytes();
    }
}
