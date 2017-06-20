using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Linq;
using System.Web;
using System.Text;

namespace Project.Models
{
    public class CryptoData
    {
        public static HashAlgorithm HashFunc = SHA256.Create();
        public static RNGCryptoServiceProvider PRG = new RNGCryptoServiceProvider();
    }

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

        private User()
        {

        }

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

        public bool IsPassword(string password)
        {
            byte[] input = Encoding.ASCII.GetBytes(password + Salt);
            password = Encoding.ASCII.GetString(CryptoData.HashFunc.ComputeHash(input));
            return password == this.HashValue;
        }
    }
}