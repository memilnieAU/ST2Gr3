using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using d_layer;

namespace l_layer
{
    public class hentPatientinformationer
    {
        Local_DownloadEkg hentInformation;
        public hentPatientinformationer()
        {
            hentInformation = new Local_DownloadEkg();
        }
        /// <summary>
        /// henter patientinformaioner fra d_layer og sender videre til P_layer
        /// </summary>
        /// <param name="socSecNb"> det cpr, der sendes med</param>
        /// <returns>returnere navn, alder og adresse på det valgte cpr</returns>
        public string hentPinfo(string socSecNb)
        {
            return hentInformation.HentPatientinfo(socSecNb);
        }

    }
}
