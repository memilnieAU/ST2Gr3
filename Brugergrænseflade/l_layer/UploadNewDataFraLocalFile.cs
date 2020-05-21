using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using d_layer;
using DTOs;

namespace l_layer
{
    /// <summary>
    /// Denne klasse skal kunne hente en måling fra en csv fil (Kun målepunkter)
    /// Og tilføje den til den lokale database tabel "NyeEkger" med Dummydata alle tomme felter
    /// </summary>
    public class UploadNewDataFraLocalFile
    {
        Local_UploadEkg local_Upload;
        DownloadFraLocalFile downloadFraLocalFile;
        DTO_EkgMåling nyMåling;
        public int sidsteMålingUpladede;
        public UploadNewDataFraLocalFile()
        {
            downloadFraLocalFile = new DownloadFraLocalFile();
            local_Upload = new Local_UploadEkg();
        }
        /// <summary>
        /// Denne metode vil hente fra en given fil
        /// filnavnet skal skrives som parameter i HentFraCsvFil("****")
        /// Den filerne skal være placeret i p_layer\bin\debug mappen 
        /// </summary>
        public void HentDataFraFil(int nummer)
        {
            string filNavn = "";
            int filnummer = nummer;

            switch (filnummer)
            {
                case 0:
                    {
                        filNavn = "0. TestSignal.csv";
                        break;
                    }
                default:
                case 1:
                    {
                        filNavn = "1. Normal 30 BPM.csv";
                        break;
                    }
                case 2:
                    {
                        filNavn = "2. Normal 80 BPM.csv";
                        break;
                    }
                case 16:
                    {

                        filNavn = "16. Atrial flutter.csv";
                        break;
                    }
                case 17:
                    {
                        filNavn = "17. Coarse atrial fibrillation.csv";
                        break;
                    }
                case 18:
                    {
                        filNavn = "18. Fine atrial fibrillation.csv";
                        break;
                    }

                case 25:
                    {
                        filNavn = "25. ST elevation of 0.6 mV.csv";
                        break;
                    }
            }

            nyMåling = downloadFraLocalFile.HentFraCsvFil(filNavn);
            UploadDataTilLocalDB();
        }
        /// <summary>
        /// Denne metode uploader den hentede csv fil den lokale database som også EKG-måleren uploader data til
        /// </summary>
        public void UploadDataTilLocalDB()
        {

            sidsteMålingUpladede = local_Upload.UploadNewEKGFromFile(nyMåling);

        }



    }
}
