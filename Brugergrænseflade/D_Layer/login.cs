using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace d_layer
{
    public class login
    {
        private SqlConnection connection;
        private SqlDataReader reader;
        private SqlCommand command;
        private const string DBlogin = "F20ST2ITS2201908775";

        public bool isUserRegistered(string Brugernavn, string pw)
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
            { throw e; }

            connection.Close();
            return result;

        }
    }
}
