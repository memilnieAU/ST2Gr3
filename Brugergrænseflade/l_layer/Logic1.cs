using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using d_layer;

namespace l_layer
{
    public class Logic1
    {
        private login dataObject;

        public Logic1 ()
        { dataObject = new login();   }

        public bool checkLogin(String BrugerNavn, String pw)
        {
            return dataObject.isUserRegistered(BrugerNavn, pw);
        }
    }
}
