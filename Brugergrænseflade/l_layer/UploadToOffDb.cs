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
    
    public class UploadToOffDb
    {
        Up_download_Offentlig up_Download_Offentlig;
        public UploadToOffDb()
        {
            up_Download_Offentlig = new Up_download_Offentlig();
        }
        public bool uploadToOff(DTO_EkgMåling ekgmåling)
        {
            return up_Download_Offentlig.Upload(ekgmåling);
        }
    }
}
