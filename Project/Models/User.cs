using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace Project.Models
{
    /// <summary>
    /// This class contains static data that the User class uses.
    /// </summary>
    public class CryptoData
    {
        public static HashAlgorithm HashFunc = SHA256.Create();
        public static RNGCryptoServiceProvider PRG = new RNGCryptoServiceProvider();
    }

    /// <summary>
    /// This class purpose is to be a record of all the registered users.
    /// And to validate username and password.
    /// </summary>
    public class User
    {
        [Key]
        public string UserName { get; set; }
        [Required]
        public string Salt { get; set; }
        [Required]
        public string HashValue { get; set; }
        [Required]
        public string Email { get; set; }
        public short Wins { get; set; }
        public short Loses { get; set; }

        /// <summary>
        /// Empty constructor for the database use.
        /// </summary>
        private User()
        {
        }

        /// <summary>
        /// Constructor.
        /// initialize the UserName with username, the Salt with a random 8 bytes value,
        /// the HashValue with CryptoData.HashFunc(password + Salt), the Email with email
        /// and the Wins and Loses with 0.
        /// </summary>
        /// <param name="username"> string </param>
        /// <param name="password"> string with length in the range [8,20] - otherwise, exception is thrown </param>
        /// <param name="email"> the user email address </param>
        public User(string username, string password, string email)
        {
            if (password.Length < 8 || password.Length > 20)
            {
                throw new ArgumentException("Password has bad length. It should be at least 8 digits and up to 20.\n");
            }
            this.UserName = username;
            this.Email = email;
            this.Wins = 0;
            this.Loses = 0;
            // crypto stuff
            byte[] saltTemp = new byte[8];
            CryptoData.PRG.GetBytes(saltTemp);
            this.Salt = Encoding.ASCII.GetString(saltTemp);
            byte[] input = Encoding.ASCII.GetBytes(password + Salt);
            this.HashValue = Encoding.ASCII.GetString(CryptoData.HashFunc.ComputeHash(input));
        }

        /// <summary>
        /// Checks if the argument is the user password.
        /// </summary>
        /// <param name="password"> string </param>
        /// <returns> True/False - True if the password is this user password </returns>
        public bool IsPassword(string password)
        {
            byte[] input = Encoding.ASCII.GetBytes(password + Salt);
            password = Encoding.ASCII.GetString(CryptoData.HashFunc.ComputeHash(input));
            return password == this.HashValue;
        }
    }
}