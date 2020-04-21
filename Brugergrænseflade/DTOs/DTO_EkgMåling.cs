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
        int størelse;
        DateTime tid;

        Dictionary<DateTime, int> målepunkter;

        public DTO_EkgMåling()
        {
            ekgTestIdStatic++;
            ekgTestId = ekgTestIdStatic;

            målepunkter = new Dictionary<DateTime, int>();
        }

        public void Tilføjmålepunkt(DateTime tid, int størelse)
        {
            målepunkter.Add(tid, størelse);

        }

    }
}
