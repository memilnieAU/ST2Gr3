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
        public int HentAlleMålingerFraLocalDB()
        {

           
            int[] id_målinger = downloadEkg.HentAlleMåleIDer();

            int AntalNyeMålinger = 0;

            foreach (int item in id_målinger)
            {
                DTO_EkgMåling dTO_EkgMåling = (downloadEkg.HentEnMåling(item));
                nyeMålinger.Add(dTO_EkgMåling);
                if (string.IsNullOrEmpty(dTO_EkgMåling.kommentar))
                {
                    AntalNyeMålinger++;
                }
            }
            return AntalNyeMålinger;
        }
       
        public string[] HentAlleCprNr()
        {


            string[] alleCprNr  = downloadEkg.HentAlleCPRNr();

            
            return alleCprNr;
        }
        public int[] HentMåleIdUdfracpr(string cpr)
        {
            int[] måleid = downloadEkg.HentMåleIDCPR(cpr);

            return måleid;
        }
        public int HentantalAfNyeMålingerUKommentar()
        {


            string[] id_målinger = downloadEkg.HentAlleMåleIDerUKommentar();

            
            return id_målinger.Length;
        }
        public string[] HentMåleIdPåNyeMålingerUKommentar()
        {


            string[] id_målinger = downloadEkg.HentAlleMåleIDerUKommentar();


            return id_målinger;
        }

        /// <summary>
        /// Denne metode vil kun hente en specifik måling i databasen
        /// </summary>
        /// <param name="ID"></param>
        public void HentEnMålingFraLocalDB(int ID)
        {
         
            nyeMålinger.Add(downloadEkg.HentEnMåling(ID));
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
