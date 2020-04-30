using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using d_layer;

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
        double[] midArr;
        public UploadNewDataFraLocalFile()
        {
            downloadFraLocalFile = new DownloadFraLocalFile();
            local_Upload = new Local_UploadEkg();
            //HentDataFraFil();
            //UploadDateTilLocalDB();
        }
        private void HentDataFraFil()
        {
            midArr = downloadFraLocalFile.HentFraCsvFil("Alm80bpm.csv");
        }
      private void UploadDateTilLocalDB()
        {
        local_Upload.uploadNewEKG(midArr);

        }

        
            
    }
}
