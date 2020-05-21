using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using d_layer;
using DTOs;

namespace l_layer
{
    /// <summary>
    /// Ansvar: At uploade data til den offentlige database
    /// </summary>
    public class UploadToOffDb
    {
        Up_download_Offentlig up_Download_Offentlig;
        public UploadToOffDb()
        {
            up_Download_Offentlig = new Up_download_Offentlig();
        }
        /// <summary>
        /// Ansvar: uploader til den offenlige database, den kalder to hjælpe metoder
        /// </summary>
        /// <param name="ekgmåling"></param>
        /// <returns>Retunerer true hvis det lykkes at uploade data til databasen</returns>
        public bool UploadToOff(DTO_EkgMåling ekgmåling)
        {
            return up_Download_Offentlig.Upload(ekgmåling);
        }
    }
}
