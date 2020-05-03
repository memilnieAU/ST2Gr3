using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace l_layer
{
    /// <summary>
    /// Denne klasse vil kunne analysere et ekg signal
    /// Jeg finder baseline ved at inddele alle punkter i grupper af 0.1mV
    /// Et Dictionary er et 2Dimensionelt liste/array system som er, som er hurtigt at søge igennem
    /// </summary>
    public class EKG_Analyser
    {
        Dictionary<double, int> hoizontalHistogram; //fordeling af amplityder
        Dictionary<string, double> verticalHistogram;
        double[] råMåling;
        private DTOs.DTO_EkgMåling måling;

        public DTOs.DTO_EkgMåling  Måling
        {
            get { return måling; }
            private set { måling = value; }
        }

        
        public EKG_Analyser()
        {
        }
        /// <summary>
        /// Analysere en graf og retunere dianosen
        /// </summary>
        /// <param name="målingTilAnalyse"></param>
        /// <returns></returns>
        public string AnalyserEnMåling(DTOs.DTO_EkgMåling målingTilAnalyse)
        {
            måling = målingTilAnalyse;
            hoizontalHistogram = new Dictionary<double, int>();
            hoizontalHistogram.Add(0, 0);
            
            HoizontalHistogram();
            return "Sygdom";
        }

        private void HoizontalHistogram()
        {
            foreach (double item in måling.raa_data)
            {
                double afrundetTal = Math.Round(item, 1);
                if (hoizontalHistogram.ContainsKey(afrundetTal))
                {
                    hoizontalHistogram[afrundetTal]++;
                }
                else
                {
                    hoizontalHistogram.Add(afrundetTal, 1);
                }
            }
        }


    }
}
