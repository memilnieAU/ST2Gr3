using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
    public class DTO_EkgMåling
    {
        static int ekgTestIdStatic = 1;
        int ekgTestId = 0;
      

        public Dictionary<string, double> målepunkter;

        public DTO_EkgMåling()
        {
            ekgTestIdStatic++;
            ekgTestId = ekgTestIdStatic;

            målepunkter = new Dictionary<string, double>();
        }

        public void Tilføjmålepunkt(string tid, double størelse)
        {
            målepunkter.Add(tid, størelse);

        }
        public Dictionary<string, double> FåAlleMålePunkter()
        {
            return målepunkter;
        }

    }
}
