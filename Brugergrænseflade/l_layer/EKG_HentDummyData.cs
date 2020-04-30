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
    public class EKG_HentDummyData
    {
        List<DTO_EkgMåling> AllSampels;
        public EKG_HentDummyData()
        {
            AllSampels = new List<DTO_EkgMåling>();
            HentFraCsvFil();
        }


        void HentFraCsvFil()
        {
            AllSampels.Add(new DTO_EkgMåling());
            int sidsteTilføjet = AllSampels.Count - 1;


            // string-objekter til at gemme det som læses fra filen
            string inputRecord;
            string[] inputFields;


            // opret de nødvendige stream-objekter
           // FileStream input = new FileStream("Test_Atrieflimmer_1.csv", FileMode.OpenOrCreate, FileAccess.Read);
            FileStream input = new FileStream("Alm80bpm.csv", FileMode.OpenOrCreate, FileAccess.Read);

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
                    AllSampels[sidsteTilføjet].Tilføjmålepunkt(inputFields[0], Convert.ToDouble(inputFields[1])*0.001);

                }
                catch (Exception)
                {

                   // throw;
                }
                
            }

            // luk adgang til filen
            fileReader.Close();

           
        }

        public Dictionary<string,double> GetOneSampel(int id)
        {
            return AllSampels[id].FåAlleMålePunkter();
        }
        
    }
}
