using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using d_layer;
using System.Security.Cryptography;

namespace l_layer
{

    /// <summary>
    /// Ansvar står for kommunikationen mellem d_layer og p_layer. Derudover kryptere den også passwordet så det der bliver indtastet kan sammenlignes med det der står i databasen
    /// hashing metoden er meget inspireret af https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.hashalgorithm.computehash?view=netcore-3.1, dog har vi ændret i hashing metoden
    /// </summary>
    public class LoginRequest
    {
        private Local_User dataObject;

        public LoginRequest ()
        { dataObject = new Local_User();   }

        /// <summary>
        /// Ansvar: sender informationer videre til d_layer
        /// </summary>
        /// <param name="BrugerNavn"> det indtastede medarbejderID</param>
        /// <param name="pw">det indtastede password</param>
        /// <returns></returns>
        public bool checkLogin(String BrugerNavn, String pw)
        {
           using (SHA512 sha512Hash = SHA512.Create())
            {
                string krypteret = GetHash(sha512Hash, pw);
                return dataObject.IsUserRegistered(BrugerNavn, krypteret);
            }

           
            // return true;
        }

        /// <summary>
        /// opretter nye brugere og hasher deres password
        /// </summary>
        /// <param name="Brugernavn">MedarbejderID</param>
        /// <param name="pw">password</param>
        public void registerNewUser(string Brugernavn, string pw)
        {

            using (SHA512 sha512Hash = SHA512.Create())
            {
                string krypteret = GetHash(sha512Hash, pw);
                dataObject.RegisterNewUser(Brugernavn, krypteret);
            }
            
        }
        /// <summary>
        /// sender brugernavn videre til d_layer
        /// </summary>
        /// <param name="Brugernavn">MedarbejderID</param>
        /// <returns>Retunerer true hvis medarbejder nummer findes i forvejen, </returns>
        public bool BrugerAlleredeOprettet(string Brugernavn)
        {
               return dataObject.AlleredeOprettet(Brugernavn);
        }

        /// <summary>
        /// Ansvar: kryptere password
        /// </summary>
        /// <param name="hashAlgorithm">Den algorithme som bruges til at krypterer</param>
        /// <param name="input">Password/Det indhold der skal krypteres</param>
        /// <returns></returns>
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

