using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
    public class DTO_EkgMåling
    {
        public int id_måling { get; set; }
        public string id_medarbejder { get; set; }
        public string borger_cprnr { get; set; }
        public DateTime start_tidspunkt { get; set; }
        public double[] raa_data { get; set; }
        public int antal_maalepunkter { get; set; }
        public double samplerate_hz { get; set; }
        

        public DTO_EkgMåling(int id_måling, double[] raa_maalepunkter)
        {
            this.id_måling = id_måling;
            this.id_medarbejder = "NaN";
            this.borger_cprnr = "NaN";
            this.start_tidspunkt = DateTime.Now;
            this.raa_data = raa_maalepunkter;
            this.antal_maalepunkter = 0;
            this.samplerate_hz = 0;
        }
        public void TilføjArrayAfPunkter(double[] måleArray)
        {
            raa_data = måleArray;
        }

        
        

    }
}
