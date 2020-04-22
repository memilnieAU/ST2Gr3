using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace l_layer
{
    public class Logic1
    {
        public bool checkLogin(String BrugerNavn, String pw)
        {
            //return true;
            return (BrugerNavn == "999999-0000" && pw == "testpw");
        }
    }
}
