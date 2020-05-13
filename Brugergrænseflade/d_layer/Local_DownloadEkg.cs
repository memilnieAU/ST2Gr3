using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOs;

namespace d_layer
{
    /// <summary>
    /// Ansvar: At hente EKG-målinger fra en lokal database
    /// </summary>
    public class Local_DownloadEkg
    {

        DTO_EkgMåling målingFraDB;


        /// <summary>
        /// Ansvar: At hente en specifik måling i den lokale database
        /// </summary>
        /// <param name="måleId">MåleID på den målling man ønsker at hente</param>
        /// <returns> Rådata fra en secifik måling </returns>
        public double[] HentEkgDataRaaData(int måleId)
        {

            SqlConnection conn;
            const String db = "F20ST2ITS2201908775";

            conn = new SqlConnection("Data Source = st-i4dab.uni.au.dk;Initial Catalog = " + db + ";Persist Security Info = True;User ID = " + db + ";Password = " + db + "");
            conn.Open();
            SqlDataReader rdr;
            byte[] bytesArr = new byte[8];
            double[] tal;
            string selectString = "Select * from SP_NyeEkger where id_måling = " + måleId;
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

        /// <summary>
        /// Ansvar: At tælle hvor mange målinger der er i den lokale database
        /// </summary>
        /// <returns> Antallet af elementer i databasen </returns>
        public int HentAntalletAfMålinger()
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
        /// <summary>
        /// Ansvar: At hente alle de måleId'er der er i databasen
        /// </summary>
        /// <returns>Et array der indeholder alle måleID'er</returns>
        public int[] HentAlleMåleIDer()
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

        /// <summary>
        /// Ansvar: At hente en specifik måling fra den lokale database, ud fra Måle_id
        /// </summary>
        /// <param name="måle_Id">Måle_ID fra den målling man ønsker</param>
        /// <returns>Et DTO_objekt med alle de informationer der fremgår i database for den enkle måling</returns>
        public DTO_EkgMåling HentEnMåling(int måle_Id)
        {

            målingFraDB = new DTO_EkgMåling(måle_Id, HentEkgDataRaaData(måle_Id));
            SqlConnection conn;
            const String db = "F20ST2ITS2201908775";

            conn = new SqlConnection("Data Source = st-i4dab.uni.au.dk;Initial Catalog = " + db + ";Persist Security Info = True;User ID = " + db + ";Password = " + db + "");
            conn.Open();
            SqlDataReader rdr;

            string selectString = "Select * from SP_NyeEkger where id_måling = " + måle_Id;
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
                        DateTime tid = Convert.ToDateTime(rdr["start_tidspunkt"]);
                        målingFraDB.start_tidspunkt = tid;
                    }
                    if (rdr["antal_maalepunkter"] != DBNull.Value)
                        målingFraDB.antal_maalepunkter = (int)rdr["antal_maalepunkter"];

                    if (rdr["samplerate_hz"] != DBNull.Value)
                        målingFraDB.samplerate_hz = Convert.ToDouble(rdr["samplerate_hz"]);

                    if (rdr["kommentar"] != DBNull.Value)
                        målingFraDB.kommentar = (string)(rdr["kommentar"]);
                }
            }
            conn.Close();

            return målingFraDB;
        }
    }
}
