using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using Npgsql;

namespace backend
{
    public class IngredientNameValueModel
    {
        public string zutat_name { get; set; }
        public short verringern_um { get; set; }

        public IngredientNameValueModel()
        {

        }
    }
}
