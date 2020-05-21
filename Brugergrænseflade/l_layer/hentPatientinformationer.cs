using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using d_layer;

namespace l_layer
{
    /// <summary>
    /// Ansvar: At hente informationer om en patient
    /// </summary>
    public class HentPatientInfo
    {
        Local_DownloadEkg hentInformation;
        public HentPatientInfo()
        {
            hentInformation = new Local_DownloadEkg();
        }
        /// <summary>
        /// henter patientinformaioner fra d_layer og sender videre til P_layer
        /// </summary>
        /// <param name="socSecNb"> det cpr, der sendes med</param>
        /// <returns>returnere navn, alder og adresse på det valgte cpr</returns>
        public string HentPinfo(string socSecNb)
        {
            return hentInformation.HentPatientinfo(socSecNb);
        }

    }
}
