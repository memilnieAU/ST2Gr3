﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace d_layer
{
    /// <summary>
    /// Ansvar: forbindelse til medarbejderid tabellen i databasen
    /// </summary>
    public class Local_User
    {

        private SqlConnection connection;
        private SqlDataReader reader;
        private SqlCommand command;
        private const string DBlogin = "F20ST2ITS2201908775";
        private bool resultat;

        /// <summary>
        /// ansvar: sammenligner data i databasen med de indtastede data (medarbejderID og password)
        /// </summary>
        /// <param name="Brugernavn">Det brugernavn der sammenlignes med</param>
        /// <param name="pw">Det Password der sammenlignes med</param>
        /// <returns>Retunerer true hvis login oplysninger er gyldige</returns>
        public bool IsUserRegistered(string Brugernavn, string pw)
        {
            connection = new SqlConnection("Data Source=st-i4dab.uni.au.dk; Initial Catalog=" + DBlogin + "; User ID= " + DBlogin + "; Password=" + DBlogin + "; Connect Timeout=30; Encrypt=False; TrustServerCertificate=False; ApplicationIntent=ReadWrite; MultiSubnetFailover=False");

            bool result = false;
            command = new SqlCommand("select * from db_owner.SP_MedarbejderID where MedarbejderID = '" + Brugernavn + "'", connection);

            connection.Open();
            try
            {
                reader = command.ExecuteReader();

                if (reader.Read())
                {
                    if (reader["MedarbejderID"].ToString() == Brugernavn && reader["MedarbejderPW"].ToString() == pw)
                    { result = true; }
                }
            }
            catch (SqlException e)
            { return false; }

            connection.Close();
            return result;

        }
        /// <summary>
        /// opretter nye bruger i databassen med de indtastede data
        /// </summary>
        /// <param name="Brugernavn">Det nye brugernavn</param>
        /// <param name="pw">Det nye password</param>
        public void RegisterNewUser(string Brugernavn, string pw)
        {

            connection = new SqlConnection("Data Source=st-i4dab.uni.au.dk; Initial Catalog=" + DBlogin + "; User ID= " + DBlogin + "; Password=" + DBlogin + "; Connect Timeout=30; Encrypt=False; TrustServerCertificate=False; ApplicationIntent=ReadWrite; MultiSubnetFailover=False");

            command = new SqlCommand("insert into db_owner.SP_MedarbejderID Values ( '" + Brugernavn + "', '" + pw + "')", connection);

            connection.Open();

            try
            {

                command.ExecuteNonQuery();
                connection.Close();
            }
            catch (SqlException e)
            { throw e; }

        }

        /// <summary>
        /// Ansvar: sammenligner det indtastede MedarbejderID med de allerede registerede bruger,
        /// så der ikke kan blive oprettet to brugere med samme login
        /// </summary>
        /// <param name="Brugernavn">medarbejderID</param>
        /// <returns>Retunerer true hvis medarbejder nummer findes i forvejen</returns>
        public bool AlleredeOprettet(string Brugernavn)
        {
            resultat = false;


            connection = new SqlConnection("Data Source=st-i4dab.uni.au.dk; Initial Catalog=" + DBlogin + "; User ID= " + DBlogin + "; Password=" + DBlogin + "; Connect Timeout=30; Encrypt=False; TrustServerCertificate=False; ApplicationIntent=ReadWrite; MultiSubnetFailover=False");
            command = new SqlCommand("select * from db_owner.SP_MedarbejderID where MedarbejderID = '" + Brugernavn + "'", connection);

            connection.Open();


            try
            {
                reader = command.ExecuteReader();

                if (reader.Read())
                {
                    if (reader["MedarbejderID"].ToString() == Brugernavn)
                        resultat = true;
                }
            }
            catch (SqlException e)
            {
                throw e;
            }

            return resultat;
        }


    }

}
