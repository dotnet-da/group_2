using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace frontend
{
    public class Account
    {
        public string ac_username { get; set; }
        public string ac_password { get; set; } 

        public Account(string u, string p)
        {
            this.ac_username = u;
            this.ac_password = p;
        }
    }
}
