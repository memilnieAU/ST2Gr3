using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace d_layer
{
    class Upload_Offentlig
    {
        private SqlConnection connection;
        private SqlDataReader reader;
        private SqlCommand command;
        private const string DBlogin = "ST2PRJ2OffEKGDatabase";
        public Upload_Offentlig()
        {

        }
        public void upload(string Brugernavn, string pw)
        {
            connection = new SqlConnection("Data Source=st-i4dab.uni.au.dk; Initial Catalog=" + DBlogin + "; User ID= " + DBlogin + "; Password=" + DBlogin + "; Connect Timeout=30; Encrypt=False; TrustServerCertificate=False; ApplicationIntent=ReadWrite; MultiSubnetFailover=False");
            //TODO skal passes
            command = new SqlCommand("insert into db_owner.SP_MedarbejderID Values ( '" + Brugernavn + "', '" + pw + "')", connection);

            connection.Open();
            try
            {
                command.ExecuteNonQuery();
            }
            catch (SqlException e)
            { throw e; }

            connection.Close();

        }
    }
}
