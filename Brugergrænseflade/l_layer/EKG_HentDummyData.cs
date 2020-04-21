using DTOs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace l_layer
{
    /// <summary>
    /// Denne klasse er en logic stump
    /// Denne skal hente data som bruges til at teste "EKG_Analyse" med.
    /// På sigt skal den skal dataen komme fra en database (Lokal/Offentlig)
    /// 
    /// </summary>
    class EKG_HentDummyData
    {
        List<DTO_EkgMåling> AllSampels;
        public EKG_HentDummyData()
        {
            AllSampels = new List<DTO_EkgMåling>();
        }


        void HentFraCsvFil()
        {
            AllSampels.Add(new DTO_EkgMåling());
            int sidsteTilføjet = AllSampels.Count - 1;


            // string-objekter til at gemme det som læses fra filen
            string inputRecord;
            string[] inputFields;


            // opret de nødvendige stream-objekter
            FileStream input = new FileStream("Test_Atrieflimmer_1.txt", FileMode.Open, FileAccess.Read);
            StreamReader fileReader = new StreamReader(input);


            // indlæs sålænge der er data i filen
            while ((inputRecord = fileReader.ReadLine()) != null)
            {
                // split data op i fornavn, efternavn og telefonnummer
                inputFields = inputRecord.Split(';');

                // gem data i listen
                if (Convert.ToDateTime(inputFields[0]) != null)
                {
                    AllSampels[sidsteTilføjet].Tilføjmålepunkt(Convert.ToDateTime(inputFields[0]), Convert.ToInt32(inputFields[1]));
                }
            }

            // luk adgang til filen
            fileReader.Close();

            // udskriv de indlæste data på skærmen
            //Console.WriteLine("Disse kontakter blev indlæst:\n");

            //foreach (Person p in contacts)
            //{
            //    Console.WriteLine(p);
            //}

            //Console.WriteLine();
        }

    }
}
