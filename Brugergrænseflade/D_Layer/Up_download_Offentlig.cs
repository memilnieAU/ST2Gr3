using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using DTOs;


namespace d_layer
{
    class Up_download_Offentlig
    {
        private SqlConnection connection;
        private SqlDataReader reader;
        private SqlCommand command;
        private const string DBlogin = "ST2PRJ2OffEKGDatabase";
        public Up_download_Offentlig()
        {

        }
        //public void upload(DTO_EkgMåling nyMåling)
        //{
        //    connection = new SqlConnection("Data Source=st-i4dab.uni.au.dk; Initial Catalog=" + DBlogin + "; User ID= " + DBlogin + "; Password=" + DBlogin + "; Connect Timeout=30; Encrypt=False; TrustServerCertificate=False; ApplicationIntent=ReadWrite; MultiSubnetFailover=False");
        //    //TODO skal rettes til
        //    command = new SqlCommand("insert into dbo.EKGMAELING Values ( '" +  + "', '" + pw + "')", connection);

        //    int retur;
        //    connection = new SqlConnection("Data Source = st-i4dab.uni.au.dk;Initial Catalog = " + DBlogin + ";Persist Security Info = True;User ID = " + DBlogin + ";Password = " + DBlogin + "");
        //    connection.Open();
        //    string format = "yyyy-MM-dd HH:mm:ss";
        //    string insertStringParam = $"INSERT INTO SP_NyeEkger ([raa_data],[id_medarbejder],[borger_cprnr],[start_tidspunkt],[antal_maalepunkter],[samplerate_hz]) OUTPUT INSERTED.id_måling VALUES(@data,'{nyMåling.id_medarbejder}','{nyMåling.borger_cprnr}','{nyMåling.start_tidspunkt.ToString(format)}',{nyMåling.antal_maalepunkter},@hz)";
        //    using (SqlCommand cmd = new SqlCommand(insertStringParam, connection))
        //    {
        //        cmd.Parameters.AddWithValue("@data",
        //        nyMåling.raa_data.SelectMany(value =>
        //        BitConverter.GetBytes(value)).ToArray());

        //        cmd.Parameters.AddWithValue("@hz", (float)nyMåling.samplerate_hz);


        //        retur = (int)cmd.ExecuteScalar();
        //    }
        //    connection.Close();

        //    return retur;
        //}
    //}
    }
}
