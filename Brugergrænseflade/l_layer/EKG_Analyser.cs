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
        public EKG_Analyser(double[] målingTilAnalyse)
        {
            hoizontalHistogram = new Dictionary<double, int>();
            hoizontalHistogram.Add(0, 0);
            råMåling = målingTilAnalyse;
            HoizontalHistogram();
        }

        private void HoizontalHistogram()
        {
            foreach (double item in råMåling)
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
