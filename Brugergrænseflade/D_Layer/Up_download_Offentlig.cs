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
        public Up_download_Offentlig()
        {

        }

        /// <summary>
        /// Ansvar: uploader måling til databasen
        /// </summary>
        /// <param name="nyMåling"></param>
        /// <returns></returns>
        public bool upload(DTO_EkgMåling nyMåling)
        {
            //int id = uploadEKGMaeling(nyMåling);
            if (uploadEKGMaeling(nyMåling))
            {
                uploadEKGData(nyMåling);
                return true;
            }
            else
            { return false; }
                      
        }
        private bool uploadEKGMaeling(DTO_EkgMåling nyMåling)
        {
            try
            {
                connection = new SqlConnection("Data Source = st-i4dab.uni.au.dk;Initial Catalog = " + DBlogin + ";Persist Security Info = True;User ID = " + DBlogin + ";Password = " + DBlogin + "");
                connection.Open();
                //TODO skal den ikke også sætte information ind i dbo.EKGDATA
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
        private void uploadEKGData(DTO_EkgMåling nyMåling)
        {
            try
            {
                connection = new SqlConnection("Data Source = st-i4dab.uni.au.dk;Initial Catalog = " + DBlogin + ";Persist Security Info = True;User ID = " + DBlogin + ";Password = " + DBlogin + "");
                connection.Open();
                //TODO skal den ikke også sætte information ind i dbo.EKGDATA
                string insertStringParam = $"INSERT INTO dbo.EKGDATA ([raa_data],[samplerate_hz],[interval_sec],[data_format],[bin_eller_tekst],[maaleformat_type],[start_tid],[kommentar],[ekgmaaleid])" + $" OUTPUT INSERTED.ekgmaaleid VALUES(@data, {Convert.ToInt32(nyMåling.samplerate_hz)},'{Convert.ToInt32(nyMåling.antal_maalepunkter*(1/nyMåling.samplerate_hz))}','andet','b','{nyMåling.raa_data[0].GetType()}',@dato,'{nyMåling.kommentar}', {ekgMaaleID})";

                using (SqlCommand cmd = new SqlCommand(insertStringParam, connection))
                {
                    cmd.Parameters.AddWithValue("@data",
                    nyMåling.raa_data.SelectMany(value =>
                    BitConverter.GetBytes(value)).ToArray());
                    cmd.Parameters.AddWithValue("@dato", nyMåling.start_tidspunkt);
                    //cmd.Parameters.AddWithValue("@maaleid", ekgMaaleID);
                    cmd.ExecuteScalar();
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


