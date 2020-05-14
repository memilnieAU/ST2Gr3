using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using DTOs;


namespace d_layer
{
    public class Up_download_Offentlig
    {
        private SqlConnection connection;
     
        private const string DBlogin = "ST2PRJ2OffEKGDatabase";
        public Up_download_Offentlig()
        {

        }
        public bool upload(DTO_EkgMåling nyMåling)
        {
            //int id = uploadEKGMaeling(nyMåling);
            return uploadEKGMaeling(nyMåling);
        }
        private bool uploadEKGMaeling(DTO_EkgMåling nyMåling)
        {
            try
            {
                //connection = new SqlConnection("Data Source=st-i4dab.uni.au.dk; Initial Catalog=" + DBlogin + "; User ID= " + DBlogin + "; Password=" + DBlogin + "; Connect Timeout=30; Encrypt=False; TrustServerCertificate=False; ApplicationIntent=ReadWrite; MultiSubnetFailover=False");
                connection = new SqlConnection("Data Source = st-i4dab.uni.au.dk;Initial Catalog = " + DBlogin + ";Persist Security Info = True;User ID = " + DBlogin + ";Password = " + DBlogin + "");
                connection.Open();
                //string format = "yyyy-MM-dd HH:mm:ss";
                //string insertStringParam = $"INSERT INTO dbo.EKGMAELING ([dato],[antalmaalinger],[sfp_ansvrmedarbjnr],[sfp_ans_org])" + $" OUTPUT INSERTED.ekgmaaleid VALUES(CAST('{nyMåling.start_tidspunkt.ToString()}' AS Datetime),{nyMåling.antal_maalepunkter},'{nyMåling.id_medarbejder}','Gruppe 3')";
                string insertStringParam = $"INSERT INTO dbo.EKGMAELING ([dato],[antalmaalinger],[sfp_ansvrmedarbjnr],[sfp_ans_org])" + $" OUTPUT INSERTED.ekgmaaleid VALUES(@dato,{nyMåling.antal_maalepunkter},'{nyMåling.id_medarbejder}','Gruppe 3')";

                using (SqlCommand cmd = new SqlCommand(insertStringParam, connection))
                {
                    cmd.Parameters.AddWithValue("@dato", nyMåling.start_tidspunkt);
                    cmd.ExecuteScalar();
                }
                connection.Close();
                return true;
            }
            catch (SqlException)
            {
                return false;
            }
            
        }
    }
}

