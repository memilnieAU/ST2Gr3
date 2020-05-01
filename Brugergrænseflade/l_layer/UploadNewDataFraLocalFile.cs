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
        public UploadNewDataFraLocalFile()
        {
            downloadFraLocalFile = new DownloadFraLocalFile();
            local_Upload = new Local_UploadEkg();
            HentDataFraFil();
            UploadDateTilLocalDB();
        }
        /// <summary>
        /// Denne metode vil hente fra en given fil
        /// filnavnet skal skrives som parameter i HentFraCsvFil("****")
        /// Den filerne skal være placeret i p_layer\bin\debug mappen
        /// </summary>
        private void HentDataFraFil()
        {
            nyMåling = downloadFraLocalFile.HentFraCsvFil("Alm80bpm.csv");
        }
        /// <summary>
        /// Denne metode uploader den hentede csv fil den lokale database som også EKG-måleren uploader data til
        /// 
        /// Denne Metode er ikke færdig endnu, da den skal kunne udfylde dummydata i de tomme felter
        /// </summary>
        private void UploadDateTilLocalDB()
        {
            
            local_Upload.UploadNewEKGFromFile(nyMåling);

        }



    }
}
