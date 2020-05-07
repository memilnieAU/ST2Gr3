using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOs;
namespace d_layer
{
    public class Local_UploadEkg
    {
        public int UploadNewEKGFromFile(DTO_EkgMåling nyMåling)
        {
            SqlConnection conn;
            const String db = "F20ST2ITS2201908775";
            int retur;
            conn = new SqlConnection("Data Source = st-i4dab.uni.au.dk;Initial Catalog = " + db + ";Persist Security Info = True;User ID = " + db + ";Password = " + db + "");
            conn.Open();
            string format = "yyyy-MM-dd HH:mm:ss";
            string insertStringParam = $"INSERT INTO SP_NyeEkger ([raa_data],[id_medarbejder],[borger_cprnr],[start_tidspunkt],[antal_maalepunkter],[samplerate_hz]) OUTPUT INSERTED.id_måling VALUES(@data,'{nyMåling.id_medarbejder}','{nyMåling.borger_cprnr}','{nyMåling.start_tidspunkt.ToString(format)}',{nyMåling.antal_maalepunkter},@hz)";
            using (SqlCommand cmd = new SqlCommand(insertStringParam, conn))
            {
                cmd.Parameters.AddWithValue("@data",
                nyMåling.raa_data.SelectMany(value =>
                BitConverter.GetBytes(value)).ToArray());

                cmd.Parameters.AddWithValue("@hz", (float)nyMåling.samplerate_hz);
                
                
                retur = (int)cmd.ExecuteScalar();
            }
            conn.Close();
            
            return retur;
        }
    }
}
