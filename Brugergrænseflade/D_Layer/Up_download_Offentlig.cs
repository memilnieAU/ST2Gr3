using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using DTOs;


namespace d_layer
{
    /// <summary>
    /// Ansvar: kommunikere med den offentlige database
    /// </summary>
    public class Up_download_Offentlig
    {
        private SqlConnection connection;
        private int ekgMaaleID = 0;

        private const string DBlogin = "ST2PRJ2OffEKGDatabase";
        /// <summary>
        /// Ansvar: uploader til den offenlige database, den kalder to hjælpe metoder
        /// </summary>
        /// <param name="nyMåling"></param>
        /// <returns>Retunerer true hvis det lykkes at uploade data til databasen</returns>
        public bool Upload(DTO_EkgMåling nyMåling)
        {
            //int id = uploadEKGMaeling(nyMåling);
            if (UploadEKGMaeling(nyMåling))
            {
                UploadEKGData(nyMåling);
                return true;
            }
            else
            { return false; }
                      
        }
        /// <summary>
        /// Ansvar: At uploade data til Den offentlige DB_EKGMåling
        /// </summary>
        /// <param name="nyMåling"></param>
        /// <returns>Retuenrer true hvis det lykkedes at uploade data</returns>
        private bool UploadEKGMaeling(DTO_EkgMåling nyMåling)
        {
            try
            {
                connection = new SqlConnection("Data Source = st-i4dab.uni.au.dk;Initial Catalog = " + DBlogin + ";Persist Security Info = True;User ID = " + DBlogin + ";Password = " + DBlogin + "");
                connection.Open();
                string insertStringParam = $"INSERT INTO dbo.EKGMAELING ([dato],[antalmaalinger],[sfp_ansvrmedarbjnr],[sfp_ans_org])" + $" OUTPUT INSERTED.ekgmaaleid VALUES(@dato,{nyMåling.antal_maalepunkter},'{nyMåling.id_medarbejder}','Gruppe 3')";

                using (SqlCommand cmd = new SqlCommand(insertStringParam, connection))
                {
                    cmd.Parameters.AddWithValue("@dato", nyMåling.start_tidspunkt);
                    ekgMaaleID = Convert.ToInt32(cmd.ExecuteScalar());
                }
                connection.Close();
                return true;
            }
            catch (SqlException)
            {
                return false;
            }
        }
        /// <summary>
        /// uploader selve måle dataen til den off DB_EKGDATA
        /// </summary>
        /// <param name="nyMåling">Den måling der ønskes at uploade data fra </param>
        private void UploadEKGData(DTO_EkgMåling nyMåling)
        {
            try
            {
                connection = new SqlConnection("Data Source = st-i4dab.uni.au.dk;Initial Catalog = " + DBlogin + ";Persist Security Info = True;User ID = " + DBlogin + ";Password = " + DBlogin + "");
                connection.Open();
                string insertStringParam = $"INSERT INTO dbo.EKGDATA ([raa_data],[samplerate_hz],[interval_sec],[data_format],[bin_eller_tekst],[maaleformat_type],[start_tid],[kommentar],[ekgmaaleid])" + $" OUTPUT INSERTED.ekgmaaleid VALUES(@data, {Convert.ToInt32(nyMåling.samplerate_hz)},'{Convert.ToInt32(nyMåling.antal_maalepunkter*(1/nyMåling.samplerate_hz))}','andet','b','{nyMåling.raa_data[0].GetType()}',@dato,'{nyMåling.kommentar}', {ekgMaaleID})";

                using (SqlCommand cmd = new SqlCommand(insertStringParam, connection))
                {
                    cmd.Parameters.AddWithValue("@data",
                    nyMåling.raa_data.SelectMany(value =>
                    BitConverter.GetBytes(value)).ToArray());
                    cmd.Parameters.AddWithValue("@dato", nyMåling.start_tidspunkt);
                   
                    //TODO Skal vi tilføje, borger information
                    cmd.ExecuteNonQuery();
                }
                connection.Close();
                
                
            }
            catch (SqlException)
            {
                throw;
            }

        }

    }
}


