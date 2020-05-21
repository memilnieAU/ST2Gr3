using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{   /// <summary>
    /// Et DTO som indholder relveante data'er som tilhører en målling
    /// </summary>
    public class DTO_EkgMåling
    {
        /// <summary>
        /// Det specifikke måle id på målingen
        /// </summary>
        public int id_måling { get; set; }
        /// <summary>
        /// Det specefikke medarbejder ID på den der har taget mållingen
        /// </summary>
        public string id_medarbejder { get; set; }
        /// <summary>
        /// CPR-nummer på den patient som målingen er taget på
        /// </summary>
        public string borger_cprnr { get; set; }
        /// <summary>
        /// Start tidspunktet for mållingen
        /// </summary>
        public DateTime start_tidspunkt { get; set; }
        /// <summary>
        /// Selve måle data'en i Volt
        /// </summary>
        public double[] raa_data { get; set; }
        /// <summary>
        /// Antalet af målepunkter
        /// </summary>
        public int antal_maalepunkter { get; set; }
        /// <summary>
        /// Sampelrates i HZ
        /// </summary>
        public double samplerate_hz { get; set; }
        /// <summary>
        /// Dette er den kommentar som kommer fra lægen
        /// </summary>
        public string kommentar { get; set; }
        /// <summary>
        /// Et DTO som indholder relveante data'er som tilhører en målling
        /// </summary>
        /// <param name="id_måling">Specefikt måleID</param>
        /// <param name="raa_maalepunkter">Punkter som udgangspunkt</param>
        public DTO_EkgMåling(int id_måling, double[] raa_maalepunkter)
        {
            this.id_måling = id_måling;
            this.id_medarbejder = "NaN.";
            this.borger_cprnr = "000000-0000";
            this.start_tidspunkt = DateTime.Now;
            this.raa_data = raa_maalepunkter;
            this.antal_maalepunkter = 0;
            this.samplerate_hz = 0;
            this.kommentar = null;
        }
        /// <summary>
        /// Tilføjer punkter til DTO_Objektet
        /// </summary>
        /// <param name="måleArray">De måle punkter man vil tilføje til raa_data </param>
        public void TilføjArrayAfPunkter(double[] måleArray)
        {
            raa_data = måleArray;
        }




    }
}
