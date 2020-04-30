using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace d_layer
{
    public class Local_UploadEkg
    {
        public int uploadNewEKG(double[] arr)
        {
            SqlConnection conn;
            const String db = "F20ST2ITS2201908775";
            int retur;
            conn = new SqlConnection("Data Source = st-i4dab.uni.au.dk;Initial Catalog = " + db + ";Persist Security Info = True;User ID = " + db + ";Password = " + db + "");
            conn.Open();
            string insertStringParam = @"INSERT INTO SP_NyeEkger ([raa_data],[id_medarbejder],[start_tidspunkt],[antal_maalepunkter]) OUTPUT INSERTED.id_måling VALUES(@data,2,CAST(1930 AS datetime),1)";
            //INSERT INTO MyFirstData([Tid],[Værdi]) VALUES(CAST(2212 AS datetime),CAST(3.10 AS Decimal(3, 2)))
            using (SqlCommand cmd = new SqlCommand(insertStringParam, conn))
            {
                cmd.Parameters.AddWithValue("@data",
                arr.SelectMany(value =>
                BitConverter.GetBytes(value)).ToArray());

                retur = (int)cmd.ExecuteScalar();
            }
            conn.Close();
            return retur;
        }
    }
}
