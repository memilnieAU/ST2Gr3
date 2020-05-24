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
    /// <example>
    /// <code>
    /// Pseudo Kode:
    /// 1. Laver et horisontalt histogram med opdeling i bins af 0.1V
    /// 2. Find baseline ved at tage den bin med flest forekomster
    /// 3. Find dynamisk threshold så man kan detektere R-takker, threshold er 50% af amplituden
    /// 4. Ud fra threshold skal der tælles R-takker (Pulsslag), der tages højde for støj
    /// 5. Ud fra sampleraten [hz] kan målingens længde beregnes, derefter fraregnes alt frem til anden R-tak
    /// 6. Derefter tages antallet af pulsslag og divideres med målingens længde,
    ///     og ganges efterfølgende med 60 for at få det i pulsslag pr. minut 
    /// 7. Analyse af atrieflimren, sker ved at kigge på de 2 nærmeste bins omkring baseline.
    ///     Hvis der er mere end 50% af forekomsterne end baseline, indikerer det atrieflimren
    /// 8. Det returneres om der er sygdom eller ej
    /// </code>
    /// </example>
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
       
        private DTO_EkgMåling måling;
        /// <summary>
        /// Det måling, som der gennems i undervejs i løbet af analysen
        /// </summary>
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
        /// <summary>
        /// Hvis punkter er over dette nivoue betyder det at det er en r-tak, altså et plus slag
        /// </summary>
        public double threshold;
        /// <summary>
        /// Denne metode beregner pulsen
        /// </summary>
        private void FindPuls()
        {
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


            //TODO OBS det er ikke den rigtige samplerate vi får med fra uPc
            if (måling.samplerate_hz == 0.0005)
            {
                måling.samplerate_hz = 500;
            }

            double samepltid = 1 / måling.samplerate_hz;
            double antalfaktiskePunkter = 0;
            if (pulsslag.Count != 0)
            {
                antalfaktiskePunkter = (måling.antal_maalepunkter - pulsslag2[0] - (måling.antal_maalepunkter - pulsslag2[pulsslag2.Count - 1]));

            }

            double målingsLængde = antalfaktiskePunkter * samepltid;
            pulsPrMin = ((antalpulsslag2 - 1) / målingsLængde) * 60;


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

            if (baseline1.Value > baseline.Value * 0.5 || baseline2.Value > baseline.Value * 0.5)
            {
                return $"ADVARSEL: \r\nEKG-målingen indikerer atrieflimmer \nPuls: {pulsPrMin.ToString("f2")}";
            }
            return $"Der er ingen tegn på atrieflimmer \nPuls: {pulsPrMin.ToString("f2")}";
        }


    }
}
