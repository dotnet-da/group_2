using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace frontend.Objects
{
    public class Zutat
    {
        [DisplayName("ID")]
        public int zu_id { get; set; }
        
        [DisplayName("Name")]
        public string zu_name { get; set; }
        
        [DisplayName("Amount")]
        public int zu_amount { get; set; }


    }
}
