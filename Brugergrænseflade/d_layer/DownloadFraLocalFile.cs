using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOs;

namespace d_layer
{
    public class DownloadFraLocalFile
    {
        List<double> midlertidigePunkter;

        public DTO_EkgMåling HentFraCsvFil(String FilNavn)  //Henter data fra en måling og tilføjer punkter til en liste
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

                }
                catch (Exception)
                {

                    // throw;
                }

            }

            // luk adgang til filen
            fileReader.Close();
            DTO_EkgMåling nyMåling = new DTO_EkgMåling(0, midlertidigePunkter.ToArray());

            return nyMåling;
        }
    }
}
