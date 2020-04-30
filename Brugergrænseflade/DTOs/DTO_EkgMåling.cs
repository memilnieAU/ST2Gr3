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
      

        
        public double[] målepunkterArr;

        public DTO_EkgMåling()
        {
            ekgTestIdStatic++;
            ekgTestId = ekgTestIdStatic;

           
            
        }
        public void TilføjArrayAfPunkter(double[] måleArray)
        {
            målepunkterArr = måleArray;
        }

        
        

    }
}
