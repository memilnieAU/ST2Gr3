using System;

namespace l_layer
{
    public class Logic
    {
        public Logic()
        { }
        public bool checkLogin(String socSecNb, String pw)
        {
            return (socSecNb == "999999-0000" && pw == "testpw");
        }
    }
}
