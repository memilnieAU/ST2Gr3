using d_layer;
using DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace l_layer
{
    public class HentNyeMålinger
    {
        /// <summary>
        /// Denne klasse skal kunne hente Målinger fra den lokale database tabel "SP_NyeEkger"
        /// </summary>
        List<DTO_EkgMåling> AllSampels;
        Local_DownloadEkg downloadEkg;
        public HentNyeMålinger()
        {
            AllSampels = new List<DTO_EkgMåling>();
            //HentDataFraCVS();
            //HentEnMålingFraLocalDB(1);
            HentAlleMålingerFraLocalDB();

        }
        /// <summary>
        /// Denne metoede vil hente alle "nye" målinger sendt fra EKG_måleren
        /// </summary>
        private void HentAlleMålingerFraLocalDB()
        {

            downloadEkg = new Local_DownloadEkg();
            int antalmålinger = downloadEkg.hentAntalletAfMålinger();
            for (int i = 0; i < antalmålinger; i++)
            {
                AllSampels.Add(new DTO_EkgMåling());
                int sidsteTilføjet = AllSampels.Count - 1;

                AllSampels[sidsteTilføjet].TilføjArrayAfPunkter(downloadEkg.hentEkgData(i));
            }
        }

        /// <summary>
        /// Denne metode vil kun hente en specifik måling i databasen
        /// </summary>
        /// <param name="ID"></param>
        private void HentEnMålingFraLocalDB(int ID)
        {
            AllSampels.Add(new DTO_EkgMåling());
            int sidsteTilføjet = AllSampels.Count - 1;

            downloadEkg = new Local_DownloadEkg();
            AllSampels[sidsteTilføjet].TilføjArrayAfPunkter(downloadEkg.hentEkgData(ID));
        }

        /// <summary>
        /// Denne metode returnere en specifik måling ud fra dem der er hentet fra databasen tidligere
        /// </summary>
        /// <param name="MåleID"></param>
        /// <returns></returns>
        public DTO_EkgMåling Hent1Måling(int MåleID)
        {
            return AllSampels[MåleID];
        }
    }
}
