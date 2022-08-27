using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using Npgsql;

namespace backend
{
    public class PizzaIngredientModel
    {
        public int p_id { get; set; }
        public int z_id { get; set; }

        internal Database Db { get; set; }

        public PizzaIngredientModel(int p_id, int z_id)
        {
            this.p_id = p_id;
            this.z_id = z_id;
        }

        public PizzaIngredientModel(Database db)
        {
            Db = db;
        }
        public PizzaIngredientModel()
        {
          
        }
        public async Task<List<PizzaIngredientModel>> GetAllAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT * FROM stjucloo.pizza_has_zutat;";
            Console.WriteLine($"PizzaIngredient::GetAllAsync SQL: {cmd.CommandText}");
            return await ReadAllAsync(await cmd.ExecuteReaderAsync());
        }
        public async Task<PizzaIngredientModel> FindThisCombination()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT * FROM stjucloo.pizza_has_zutat WHERE p_id=@p_id AND z_id=@z_id;";
            BindParams(cmd);
            Console.WriteLine($"PizzaIngredient::GetAllAsync SQL: {cmd.CommandText}");
            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }


        public async Task<List<int>> GetAllIngredientIdsForThisPizza(int p_id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT z_id FROM stjucloo.pizza_has_zutat WHERE p_id = @p_id;";
            cmd.Parameters.Add(new NpgsqlParameter
            {
                ParameterName = "@p_id",
                DbType = DbType.Int32,
                Value = p_id,
            });
            Console.WriteLine($"Pizza::GetAllIngredientIdsForThisPizza SQL: {cmd.CommandText}");
            return await ReadAllAsyncInt(await cmd.ExecuteReaderAsync());
        }

        public async Task AddThisCombination()
        {
            
                using var cmd = Db.Connection.CreateCommand();
                cmd.CommandText = @"INSERT INTO stjucloo.pizza_has_zutat (p_id, z_id) VALUES (@p_id, @z_id);";
                BindParams(cmd);
                Console.WriteLine($"Pizza::AddIngredientToThisPizza SQL: {cmd.CommandText}");
                await cmd.ExecuteReaderAsync();
           
            
        }

        public async Task RemoveThisCombination()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM stjucloo.pizza_has_zutat WHERE p_id=@p_id AND z_id=@z_id";
            BindParams(cmd);
            Console.WriteLine($"Pizza::RemoveIngredientFromThisPizza SQL: {cmd.CommandText}");
            await cmd.ExecuteReaderAsync();
        }


        private async Task<List<PizzaIngredientModel>> ReadAllAsync(DbDataReader reader)
        {
            var pizzas = new List<PizzaIngredientModel>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var pizza = new PizzaIngredientModel(Db)
                    {
                        p_id = reader.GetInt32(0),
                        z_id = reader.GetInt32(1),
                    };
                    pizzas.Add(pizza);
                }
            }
            return pizzas;
        }

        private async Task<List<int>> ReadAllAsyncInt(DbDataReader reader)
        {
            var all_ints = new List<int>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    all_ints.Add(reader.GetInt32(0));
                }
            }
            return all_ints;
        }
        //\todo brauchen wir hier Update? Macht in dem Kontext wenig Sinn.
        private void BindParams(NpgsqlCommand cmd)
        {
            cmd.Parameters.Add(new NpgsqlParameter
            {
                ParameterName = "@p_id",
                DbType = DbType.Int32,
                Value = p_id,
            });
            cmd.Parameters.Add(new NpgsqlParameter
            {
                ParameterName = "@z_id",
                DbType = DbType.Int32,
                Value = z_id,
            });
        }
    }
}
