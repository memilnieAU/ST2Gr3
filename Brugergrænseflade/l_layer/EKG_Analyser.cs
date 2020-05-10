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
        KeyValuePair<double, int> baseline;
        double pulsPrMin;

        public DTOs.DTO_EkgMåling Måling
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
            FindPuls();
            string sygdom = AnalyserSygdom();
            return sygdom;
        }
        private void FindPuls()
        {
            double threshold = 0.6;
            bool underThreshold = true;

            List<double> pulsslag = new List<double>();
            int antalpulsslag = 0;
            pulsPrMin = 0;
            double tidligerepunkt = måling.raa_data[0];
            
            for (int i = 0; i < måling.raa_data.Length; i++)
            {
                double nytpunkt = måling.raa_data[i];
                if (threshold < nytpunkt && underThreshold)
                {
                    antalpulsslag++;
                    pulsslag.Add(i);
                    underThreshold = false;
                }

                if (threshold > nytpunkt)
                {
                    underThreshold = true;
                }
                else
                {
                    underThreshold = false;
                }
                

                
            
            }

            double samepltid = 1 / måling.samplerate_hz;
            double antalfaktiskePunkter = (måling.antal_maalepunkter - pulsslag[0] - (måling.antal_maalepunkter - pulsslag[pulsslag.Count - 1]));
            double målingsLængde = antalfaktiskePunkter * samepltid;
            pulsPrMin = ((antalpulsslag-1)/målingsLængde)*60;


        }
        private void HoizontalHistogram()
        {
            foreach (double item in måling.raa_data)
            {

                //afrundetTal = 0.001       0.123       0.37
                double afrundetTal = Math.Round(item, 1);
                //afrundetTal = 0.0         0.1         0.4
                if (hoizontalHistogram.ContainsKey(afrundetTal))
                {
                    hoizontalHistogram[afrundetTal]++;
                }
                else
                {
                    hoizontalHistogram.Add(afrundetTal, 1);
                }
            }
            foreach (KeyValuePair<double, int> item in hoizontalHistogram)
            {
                if (item.Value > baseline.Value)
                {
                    baseline = item;
                }
            }
        }
        private string AnalyserSygdom()
        {
            KeyValuePair<double, int> baseline1 = baseline;
            KeyValuePair<double, int> baseline2 = baseline;
            foreach (KeyValuePair<double, int> item in hoizontalHistogram)
            {
                if (item.Key == baseline.Key-0.1)
                {
                    baseline1 = item;
                }
                if (item.Key == baseline.Key + 0.1)
                {
                    baseline2 = item;
                }
            }

            //  [-0.2-0.1] 131 > [-0.1-0.0]   3300*0.9=3000 ||  [-0.1-0.0] 320 > [0.0-0.1]   3500*0.9=3000
            //  [-0.1-0.0] 1288 > [0.0-0.1]   1580*0.9=1422 ||  [0.1-0.2] 1518 > [0.0-0.1]   1580*0.9=1422

            if (baseline1.Value > baseline.Value*0.9 || baseline2.Value > baseline.Value*0.9)
            {
                return $"Der er tegn på atrieflimmer \nPuls: {pulsPrMin.ToString("f2")}";
            }
            return $"Der er ingen tegn på atrieflimmer \nPuls: {pulsPrMin.ToString("f2")}";
        }


    }
}
