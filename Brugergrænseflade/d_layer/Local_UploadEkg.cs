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
    /// <summary>
    /// Ansvar: At uploade en måling til den lokale database
    /// </summary>
    public class Local_UploadEkg
    {
        /// <summary>
        /// Dette er en hjælpe metode for at uploade data direkte fra Physionet
        /// Ansvar: At uploade en specifik måling til den lokale database, efter den er blevet hentet fra en csv fil.
        /// </summary>
        /// <param name="nyMåling">En specefik måling</param>
        /// <returns>Det specefikke MåleId som mållingen fik tilknyttet af databasen</returns>
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
        /// <summary>
        /// Ansvar: At opdatere en specefik måling med en ny kommentar
        /// </summary>
        /// <param name="måling">Den specfikke måling der ønskes at der opdateres</param>
        public void OpdaterKommentar(DTO_EkgMåling måling)
        {
            SqlConnection conn;
            const String db = "F20ST2ITS2201908775";
            conn = new SqlConnection("Data Source = st-i4dab.uni.au.dk;Initial Catalog = " + db + ";Persist Security Info = True;User ID = " + db + ";Password = " + db + "");
            conn.Open();
            string insertStringParam = $"UPDATE SP_NyeEkger set kommentar = '{måling.kommentar}' where id_måling = {måling.id_måling}";
            using (SqlCommand cmd = new SqlCommand(insertStringParam, conn))
            {
                cmd.ExecuteNonQuery();
            }
            conn.Close();
        }
        /// <summary>
        /// Ansvar: At opdatere en specefik måling med et nyt cprnummer
        /// </summary>
        /// <param name="måling">Den måling med det nye cpr-nummer</param>
        public void Opdatercpr(DTO_EkgMåling måling)
        {
            SqlConnection conn;
            const String db = "F20ST2ITS2201908775";
            conn = new SqlConnection("Data Source = st-i4dab.uni.au.dk;Initial Catalog = " + db + ";Persist Security Info = True;User ID = " + db + ";Password = " + db + "");
            conn.Open();
            string insertStringParam = $"UPDATE SP_NyeEkger set borger_cprnr = '{måling.borger_cprnr}' where id_måling = {måling.id_måling}";
            using (SqlCommand cmd = new SqlCommand(insertStringParam, conn))
            {
                cmd.ExecuteNonQuery();
            }
            conn.Close();
        }
        /// <summary>
        /// Ansvar: At slette en specefik målling
        /// </summary>
        /// <param name="måling">Den måling der ønskes at slettes</param>
        public void deleteEkg(DTO_EkgMåling måling)
        {
            SqlConnection conn;
            const String db = "F20ST2ITS2201908775";
            conn = new SqlConnection("Data Source = st-i4dab.uni.au.dk;Initial Catalog = " + db + ";Persist Security Info = True;User ID = " + db + ";Password = " + db + "");
            conn.Open();
            string insertStringParam = $"delete from SP_NyeEkger where id_måling = '{måling.id_måling}'";
            using (SqlCommand cmd = new SqlCommand(insertStringParam, conn))
            {
                cmd.ExecuteNonQuery();
            }
            conn.Close();
        }

    }
}
