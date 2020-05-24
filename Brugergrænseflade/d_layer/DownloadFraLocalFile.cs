using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOs;

namespace d_layer
{
  

    /// <summary>
    /// Ansvar: At hente EKG-datasæt fra en lokal CSV fil på computeren
    /// </summary>
    public class DownloadFraLocalFile
    {
        /// <summary>
        /// Midllertigtig punkter inden de indsættes i et DTO_Objekt
        /// </summary>
        List<double> midlertidigePunkter;
        /// <summary>
        /// Målingens starttidspunkt
        /// </summary>
        double tidstart;
        /// <summary>
        /// Målingens sluttidspunkt
        /// </summary>
        double tidslut;

        /// <summary>
        /// Ansvar: At henter data fra en måling og tilføjer punkter til en liste. 
        /// Hvad: Den udfylder: antal målepunkter, samelrate, starttid og x-antal punkter i mV
        /// </summary>
        /// <param name="FilNavn"> Dette er navnet på selve filen, der skal hentes fra</param>
        /// <returns>
        /// Den retunerer en "ny" DTO_EkgMåling med begrænset indhold       
        /// </returns>
        public DTO_EkgMåling HentFraCsvFil(String FilNavn)  
        {
            midlertidigePunkter = new List<double>();

            // string-objekter til at gemme det som læses fra filen
            string inputRecord;
            string[] inputFields;


            // opret de nødvendige stream-objekter
            // FileStream input = new FileStream("Test_Atrieflimmer_1.csv", FileMode.OpenOrCreate, FileAccess.Read);
            FileStream input = new FileStream(FilNavn, FileMode.OpenOrCreate, FileAccess.Read);
            
            StreamReader fileReader = new StreamReader(input);


            // indlæs sålænge der er data i filen
            while ((inputRecord = fileReader.ReadLine()) != null)
            {
                // split data op i fornavn, efternavn og telefonnummer
                inputFields = inputRecord.Split(',');
                for (int i = 0; i < inputFields.Length; i++)
                {
                    inputFields[i] = inputFields[i].Trim('\'');

                }

                // gem data i listen
                try
                {
                    midlertidigePunkter.Add(Convert.ToDouble(inputFields[1]) * 0.001);
                    if (tidstart == null)
                    {
                        tidstart = Convert.ToDouble(inputFields[0]) * 0.001;
                    }
                    // DateTime t2 = Convert.ToDateTime(inputFields[0]);
                    tidslut = Convert.ToDouble(inputFields[0].Substring(3)) * 0.001;
                }
                catch (Exception)
                {

                    // throw;
                }
            }

            // luk adgang til filen
            fileReader.Close();
            DTO_EkgMåling nyMåling = new DTO_EkgMåling(0, midlertidigePunkter.ToArray());
            nyMåling.antal_maalepunkter = midlertidigePunkter.Count();
            nyMåling.samplerate_hz = (1 / ((tidslut - tidstart) / nyMåling.antal_maalepunkter));

            return nyMåling;
        }
    }
}
