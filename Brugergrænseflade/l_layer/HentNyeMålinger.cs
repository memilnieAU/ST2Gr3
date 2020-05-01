using d_layer;
using DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace l_layer
{
    public class HentNyeMålingerFraLocalDB
    {

        public int id_måling { get; set; }
        public string id_medarbejder { get; set; }
        public string borger_cprnr { get; set; }
        public DateTime start_tidspunkt { get; set; }
        public double[] raa_maalepunkter { get; set; }
        public double samplerate_hz { get; set; }
        /// <summary>
        /// Denne klasse skal kunne hente Målinger fra den lokale database tabel "SP_NyeEkger"
        /// </summary>
        List<DTO_EkgMåling> AllSampels;
        Local_DownloadEkg downloadEkg;
        public HentNyeMålingerFraLocalDB()
        {
            AllSampels = new List<DTO_EkgMåling>();
            downloadEkg = new Local_DownloadEkg();
            
            //HentEnMålingFraLocalDB(1);
            HentAlleMålingerFraLocalDB();

        }
        /// <summary>
        /// Denne metoede vil hente alle "nye" målinger sendt fra EKG_måleren
        /// </summary>
        private void HentAlleMålingerFraLocalDB()
        {

            int antalmålinger = downloadEkg.hentAntalletAfMålinger();
            for (int i = 1; i < antalmålinger; i++)
            {
                
                AllSampels.Add(downloadEkg.hentMåling(i));
            }
        }

        /// <summary>
        /// Denne metode vil kun hente en specifik måling i databasen
        /// </summary>
        /// <param name="ID"></param>
        private void HentEnMålingFraLocalDB(int ID)
        {
         
            AllSampels.Add(downloadEkg.hentMåling(ID));
        }

        /// <summary>
        /// Denne metode returnere en specifik måling ud fra dem der er hentet fra databasen tidligere
        /// </summary>
        /// <param name="MåleID"></param>
        /// <returns></returns>
        public DTO_EkgMåling Hent1Måling(int MåleID)
        {
            return AllSampels[MåleID-1];
        }
    }
}
