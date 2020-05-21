using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOs;
using d_layer;

namespace l_layer
{
    /// <summary>
    /// Ansvar: At opdatere den lokale database med et nyt CPR-nummer
    /// </summary>
    public class OpdaterLocalDB
    {
        Local_UploadEkg Local_Upload;
        public OpdaterLocalDB()
        {
            Local_Upload = new Local_UploadEkg();

        }
        /// <summary>
        /// Ansvar: At opdatere en specefik måling med en ny kommentar
        /// </summary>
        /// <param name="måling">Den specfikke måling der ønskes at der opdateres</param>
        public void OpdaterKommentar(DTO_EkgMåling måling)
        {
            Local_Upload.OpdaterKommentar(måling);
        }
        /// <summary>
        /// Ansvar: At opdatere en specefik måling med et nyt cprnummer
        /// </summary>
        /// <param name="måling">Den måling med det nye cpr-nummer</param>
        public void OpdaterCpr(DTO_EkgMåling måling)
        {
            Local_Upload.Opdatercpr(måling);
        }
        /// <summary>
        /// Ansvar: At slette en specefik målling
        /// </summary>
        /// <param name="måling">Den måling der ønskes at slettes</param>
        public void DeleteEKG(DTO_EkgMåling måling)
        {
            Local_Upload.DeleteEkg(måling);
        }
    }
}
