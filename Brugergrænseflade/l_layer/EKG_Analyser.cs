using DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace l_layer
{
    /// <summary>
    /// Denne klasse vil kunne analysere et ekg signal
    /// Vi finder baseline ved at inddele alle punkter i grupper af 0.1mV
    /// Et Dictionary er et 2Dimensionelt liste/array system som er, som er hurtigt at søge igennem
    /// </summary>
    public class EKG_Analyser
    {
        /// <summary>
        /// Fordeling af amplityder
        /// </summary>
        Dictionary<double, int> hoizontalHistogram;
        /// <summary>
        /// Ikke i brug endnu
        /// </summary>
        Dictionary<string, double> verticalHistogram;
        /// <summary>
        /// Baseline, hvor graffen "svinger" omkring
        /// </summary>
        public KeyValuePair<double, int> baseline;
        /// <summary>
        /// Patientens puls
        /// </summary>
        double pulsPrMin;
        /// <summary>
        /// Det måling, som der gennems i undervejs i løbet af analysen
        /// </summary>
        private DTO_EkgMåling måling;

        public DTO_EkgMåling Måling
        {
            get { return måling; }
            private set { måling = value; }
        }


        /// <summary>
        /// Analysere en graf og retunere dianosen
        /// </summary>
        /// <param name="målingTilAnalyse">Den måling som skal analyseres</param>
        /// <returns>Returunerer svaret på analysen</returns>
        public string AnalyserEnMåling(DTO_EkgMåling målingTilAnalyse)
        {
            if (målingTilAnalyse.raa_data.Length == 0)
            {
                return "Kunne ikke analyseres";
            }
            måling = målingTilAnalyse;
            hoizontalHistogram = new Dictionary<double, int>();
            //hoizontalHistogram.Add(0, 0);

            HoizontalHistogram();
            FindPuls();
            string sygdom = AnalyserSygdom();
            return sygdom;
        }
        public double threshold;
        /// <summary>
        /// Denne metode beregner pulsen
        /// </summary>
        private void FindPuls()
        {
            //TODO Find en dynamisk måde at berenge threshold
            threshold = baseline.Key + 0.6;
            bool underThreshold = true;
            double toppunkt = baseline.Key;
            double bundpunkt = baseline.Key;
            double amplitude = 0;

            foreach (KeyValuePair<double,int> item in hoizontalHistogram)
            {
                if (item.Key > toppunkt)
                {
                    toppunkt = item.Key;
                }
                if (item.Key < bundpunkt)
                {
                    bundpunkt = item.Key;
                }
            }
            amplitude = toppunkt - baseline.Key;
            threshold = baseline.Key + amplitude * 0.5;


            List<double> pulsslag = new List<double>();
            int antalpulsslag = 0;
            List<double> pulsslag2 = new List<double>();
            int antalpulsslag2 = 0;
            pulsPrMin = 0;
            double tidligerepunkt = måling.raa_data[0];
            int gangeoverthres = 0;
            int overThreshold2 = 0;
            for (int i = Convert.ToInt32(måling.raa_data.Length*0.1); i < Convert.ToInt32(måling.raa_data.Length); i++)
            {
                double nytpunkt = måling.raa_data[i];

              
                if (threshold < nytpunkt && underThreshold)
                {
                    antalpulsslag++;
                    pulsslag.Add(i);
                    underThreshold = false;
                }
                if (threshold < nytpunkt && overThreshold2>15)
                {
                    antalpulsslag2++;
                    pulsslag2.Add(i);
                    overThreshold2 = 0;
                }

                if (threshold > nytpunkt)
                {
                    underThreshold = true;
                    overThreshold2 ++;
                }
                else
                {
                    overThreshold2 = 0;
                    underThreshold = false;
                }


            }



            if (måling.samplerate_hz == 0.0005)
            {
                måling.samplerate_hz = 200;
            }

            double samepltid = 1 / måling.samplerate_hz;
            double antalfaktiskePunkter = 0;
            if (pulsslag.Count != 0)
            {
                antalfaktiskePunkter = (måling.antal_maalepunkter - pulsslag2[0] - (måling.antal_maalepunkter - pulsslag2[pulsslag2.Count - 2]));

            }

            double målingsLængde = antalfaktiskePunkter * samepltid;
            pulsPrMin = ((antalpulsslag2 - 2) / målingsLængde) * 60;


        }
        /// <summary>
        /// Opretter det hoizontale diagram
        /// </summary>
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
        /// <summary>
        /// Analysere dataen udfra det hoizontale diagraam
        /// </summary>
        /// <returns></returns>
        private string AnalyserSygdom()
        {
            KeyValuePair<double, int> baseline1 = baseline;
            KeyValuePair<double, int> baseline2 = baseline;
            foreach (KeyValuePair<double, int> item in hoizontalHistogram)
            {
                if (item.Key == baseline.Key - 0.1)
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

            if (baseline1.Value > baseline.Value * 0.6 || baseline2.Value > baseline.Value * 0.6)
            {
                return $"ADVARSEL: \r\nEKG-målingen indikerer atrieflimmer \nPuls: {pulsPrMin.ToString("f2")}";
            }
            return $"Der er ingen tegn på atrieflimmer \nPuls: {pulsPrMin.ToString("f2")}";
        }


    }
}
