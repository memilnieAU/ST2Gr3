using d_layer;
using DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace l_layer
{
    /// <summary>
    /// Ansvar: At hente obejkter fra den lokale database
    /// Denne klasse skal kunne hente Målinger fra den lokale database tabel "SP_NyeEkger"
    /// </summary>
    public class HentFraLocalDB
    {
        /// <summary>
        /// Liste over alle nye målinger
        /// </summary>
        public List<DTO_EkgMåling> nyeMålinger;
        Local_DownloadEkg downloadEkg;
        
       
        public HentFraLocalDB()
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
       /// <summary>
       /// Henter alle CPR nummer der forkommer i den lokale database
       /// </summary>
       /// <returns>Retunerer et ararry med alle nummerne</returns>
        public string[] HentAlleCprNr()
        {
            string[] alleCprNr  = downloadEkg.HentAlleCPRNr();
            return alleCprNr;
        }
        /// <summary>
        /// Ansvar: At hente alle måleID'er udfra specefikt cpr_nummer
        /// </summary>
        /// <param name="cpr">Det specefikke cpr nummer der som, ønske måleID'er for</param>
        /// <returns>Et array der indeholder alle måleID'er der er tilknyttet det specefikke CPR_nummer </returns>

        public int[] HentMåleIdUdfracpr(string cpr)
        {
            int[] måleIder = downloadEkg.HentMåleIDCPR(cpr);
            
            return måleIder;
        }
        /// <summary>
        /// Ansvar: At hente alle de måleId'er der ikke er kommenteret i databasen
        /// </summary>
        /// <returns> returnerer antallet af nye målinger </returns>
        public int HentantalAfNyeMålingerUKommentar()
        {
            string[] id_målinger = downloadEkg.HentAlleMåleIDerUKommentar();
            return id_målinger.Length;
        }

        /// <summary>
        /// Ansvar: At hente alle de måleId'er der ikke er kommenteret i databasen
        /// </summary>
        /// <returns> returnerer alle "Cpr: xxxxxx-xxxx  MåleId: xx " som string array </returns>
        public string[] HentMåleIdPåNyeMålingerUKommentar()
        {
           
            string[] id_målinger = downloadEkg.HentAlleMåleIDerUKommentar();

            return id_målinger;
        }

        /// <summary>
        /// Denne metode vil kun hente en specifik måling i databasen
        /// Den metode tilføjer den hentede måling så den nu fremgår lokalt på computeren
        /// </summary>
        /// <param name="Måle_ID">Det måle id man ønsker at hente data fra</param>
        public void HentEnMålingFraLocalDB(int Måle_ID)
        {
         
            nyeMålinger.Add(downloadEkg.HentEnMåling(Måle_ID));
        }

        /// <summary>
        /// Denne metode returnere en specifik måling ud fra dem der er hentet fra databasen tidligere
        /// </summary>
        /// <param name="MåleID">Det måle ID som man ønsker at hente</param>
        /// <returns>Retunerer en måling med alle relevante dataer</returns>
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

    }
}
