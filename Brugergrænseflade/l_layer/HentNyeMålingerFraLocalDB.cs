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
        public List<DTO_EkgMåling> nyeMålinger;

        Local_DownloadEkg downloadEkg;
        public HentNyeMålingerFraLocalDB()
        {
            nyeMålinger = new List<DTO_EkgMåling>();
            downloadEkg = new Local_DownloadEkg();
            
            

        }
        /// <summary>
        /// Denne metoede vil hente alle "nye" målinger sendt fra EKG_måleren
        /// </summary>
        public void HentAlleMålingerFraLocalDB()
        {

            //int antalmålinger = downloadEkg.hentAntalletAfMålinger();
            //for (int i = 1; i < antalmålinger; i++)
            //{
                
            //    AllSampels.Add(downloadEkg.hentMåling(i));
            //}

            int[] id_målinger = downloadEkg.hentID_MålingerAfMålinger();
            foreach (int item in id_målinger)
            {
                nyeMålinger.Add(downloadEkg.hentMåling(item));

            }
            
        }

        /// <summary>
        /// Denne metode vil kun hente en specifik måling i databasen
        /// </summary>
        /// <param name="ID"></param>
        public void HentEnMålingFraLocalDB(int ID)
        {
         
            nyeMålinger.Add(downloadEkg.hentMåling(ID));
        }

        /// <summary>
        /// Denne metode returnere en specifik måling ud fra dem der er hentet fra databasen tidligere
        /// </summary>
        /// <param name="MåleID"></param>
        /// <returns></returns>
        public DTO_EkgMåling Hent1Måling(int MåleID)
        {
            foreach (DTO_EkgMåling item in nyeMålinger)
            {
                if (item.id_måling == MåleID)
                {
                    return item;
                }
            }
            return nyeMålinger[0];
        }
        public DTO_EkgMåling Hent1MålingUdFraAll(int placering)
        {
            return nyeMålinger[placering];
        }
    }
}
