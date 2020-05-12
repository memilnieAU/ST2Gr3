using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using d_layer;
using System.Security.Cryptography;

namespace l_layer
{
    public class LoginRequest
    {
        private Local_User dataObject;

        public LoginRequest ()
        { dataObject = new Local_User();   }

        public bool checkLogin(String BrugerNavn, String pw)
        {
           using (SHA512 sha512Hash = SHA512.Create())
            {
                string krypteret = GetHash(sha512Hash, pw);
                return dataObject.isUserRegistered(BrugerNavn, krypteret);
            }

           
            // return true;
        }

        public void registerNewUser(string Brugernavn, string pw)
        {

            using (SHA512 sha512Hash = SHA512.Create())
            {
                string krypteret = GetHash(sha512Hash, pw);
                dataObject.registerNewUser(Brugernavn, krypteret);
            }
        }

        private static string GetHash(HashAlgorithm hashAlgorithm, string input)
        {

            // Konventere indputtet (pw) til et byte array og laver hashen
            byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));


            // her laves en stringbuilder, der samler bytesne og laver en string
            var sBuilder = new StringBuilder();


            // løkker igennem hver byte af den hashede data, og formatere til en haxadecimal string
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Returnere hexadecimal string
            return sBuilder.ToString();
        }
        
    }
}

