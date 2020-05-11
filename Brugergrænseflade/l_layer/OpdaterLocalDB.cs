using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOs;
using d_layer;

namespace l_layer
{
    public class OpdaterLocalDB
    {
        Local_UploadEkg Local_Upload;
        public OpdaterLocalDB()
        {
            Local_Upload = new Local_UploadEkg();

        }

        public void OpdaterKommentar(DTO_EkgMåling måling)
        {
            Local_Upload.OpdaterEKG(måling);
        }
    }
}
