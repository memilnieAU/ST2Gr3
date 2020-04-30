using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace p_layer
{
    class cprEksempel
    {
        string cpr;
        int maling;
        public cprEksempel(string _cpr, int _maling)
        {
            cpr = _cpr;

        }

        public string CPR
        {
            get
            {
                return cpr;
            }
            set
            {
                cpr = value;
            }
        }

        public int Maling
        {
            get
            {
                return maling;
            }
            set
            {
                maling = value;
            }
        }
    }
}
