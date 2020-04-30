using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace l_layer
{
    /// <summary>
    /// Denne klasse er til at teste 
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
