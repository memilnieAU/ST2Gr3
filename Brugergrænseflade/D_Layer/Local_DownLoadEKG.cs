using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOs;

namespace d_layer
{
    public class Local_DownloadEkg
    {
        public int id_måling { get; set; }
        public string id_medarbejder { get; set; }
        public string borger_cprnr { get; set; }
        public DateTime start_tidspunkt { get; set; }
        public double[] raa_maalepunkter { get; set; }
        public double samplerate_hz { get; set; }

        DTO_EkgMåling målingFraDB;

        public Local_DownloadEkg()
        {

        }
        public double[] hentEkgData(int index)
        {
            SqlConnection conn;
            const String db = "F20ST2ITS2201908775";

            conn = new SqlConnection("Data Source = st-i4dab.uni.au.dk;Initial Catalog = " + db + ";Persist Security Info = True;User ID = " + db + ";Password = " + db + "");
            conn.Open();
            SqlDataReader rdr;
            byte[] bytesArr = new byte[8];
            double[] tal;
            string selectString = "Select * from SP_NyeEkger where id_måling = " + index;
            using (SqlCommand cmd = new SqlCommand(selectString, conn))
            {
                rdr = cmd.ExecuteReader();
                if (rdr.Read())
                    bytesArr = (byte[])rdr["raa_data"];


                tal = new double[bytesArr.Length / 8];

                for (int i = 0, j = 0; i < bytesArr.Length; i += 8, j++)
                    tal[j] = BitConverter.ToDouble(bytesArr, i);
            }
            conn.Close();

            return tal;
        }
        public int hentAntalletAfMålinger()
        {
            SqlConnection conn;
            const String db = "F20ST2ITS2201908775";

            conn = new SqlConnection("Data Source = st-i4dab.uni.au.dk;Initial Catalog = " + db + ";Persist Security Info = True;User ID = " + db + ";Password = " + db + "");
            conn.Open();
            SqlDataReader rdr;

            int tal = 0;
            string selectString = "Select COUNT(id_måling) as AntalMålinger From SP_NyeEkger";
            using (SqlCommand cmd = new SqlCommand(selectString, conn))
            {
                rdr = cmd.ExecuteReader();
                if (rdr.Read())
                    tal = (int)rdr["AntalMålinger"];

                //tal = (int)rdr["AntalMålinger"];
            }
            conn.Close();
            return tal;
        }
        public int[] hentID_MålingerAfMålinger()
        {
            SqlConnection conn;
            const String db = "F20ST2ITS2201908775";

            conn = new SqlConnection("Data Source = st-i4dab.uni.au.dk;Initial Catalog = " + db + ";Persist Security Info = True;User ID = " + db + ";Password = " + db + "");
            conn.Open();
            SqlDataReader rdr;

            List<int> tal = new List<int>();
            
            string selectString = "Select id_måling From SP_NyeEkger";
            using (SqlCommand cmd = new SqlCommand(selectString, conn))
            {
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    tal.Add((int)rdr["id_måling"]);

                }
                //tal = (int)rdr["AntalMålinger"];
            }
            conn.Close();
            return tal.ToArray();
        }

        //Select id_måling From SP_NyeEkger

        public DTO_EkgMåling hentMåling(int id_måling)
        {

            målingFraDB = new DTO_EkgMåling(id_måling, hentEkgData(id_måling));
            SqlConnection conn;
            const String db = "F20ST2ITS2201908775";

            conn = new SqlConnection("Data Source = st-i4dab.uni.au.dk;Initial Catalog = " + db + ";Persist Security Info = True;User ID = " + db + ";Password = " + db + "");
            conn.Open();
            SqlDataReader rdr;

            string selectString = "Select * from SP_NyeEkger where id_måling = " + id_måling;
            using (SqlCommand cmd = new SqlCommand(selectString, conn))
            {
                rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    målingFraDB.id_måling = (int)rdr["id_måling"];
                    if (rdr["id_medarbejder"] != DBNull.Value)
                        målingFraDB.id_medarbejder = (string)rdr["id_medarbejder"];


                    if (rdr["borger_cprnr"] != DBNull.Value)
                        målingFraDB.borger_cprnr = (string)rdr["borger_cprnr"];

                    if (rdr["start_tidspunkt"] != DBNull.Value)
                    {
                        string tid = (string)rdr["start_tidspunkt"];
                        målingFraDB.start_tidspunkt = DateTime.FromBinary(long.Parse(tid));
                    }
                    if (rdr["antal_maalepunkter"] != DBNull.Value)
                        målingFraDB.antal_maalepunkter = (int)rdr["antal_maalepunkter"];
                    if (rdr["samplerate_hz"] != DBNull.Value)
                        målingFraDB.samplerate_hz = Convert.ToDouble( rdr["samplerate_hz"]);


                }


            }
            conn.Close();



            return målingFraDB;
        }

    }
}
