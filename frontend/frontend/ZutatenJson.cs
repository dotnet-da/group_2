using frontend.Objects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace frontend
{
    public class ZutatenJson
    {
        [JsonProperty("zutatenList")]
        public List<Zutat> zutatenList { get; set; }
        public int count { get; set; }
    }
}
