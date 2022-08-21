using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using Npgsql;

namespace backend
{
    public class Pizza
    {
        public int p_id { get; set; }
        public string p_name { get; set; }

        internal Database Db { get; set; }

        public Pizza()
        {
        }

        internal Pizza(Database db)
        {
            Db = db;
        }

        public async Task<Pizza> FindOneAsync(int id_pizza)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT * FROM pizza WHERE p_id = @id_pizza;";
            cmd.Parameters.Add(new NpgsqlParameter
            {
                ParameterName = "@id_pizza",
                DbType = DbType.Int32,
                Value = id_pizza,
            });
            Console.WriteLine($"Pizza::FindOneAsync SQL: {cmd.CommandText}");
            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }

        public async Task<List<Pizza>> GetAllAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT * FROM pizza;";
            Console.WriteLine($"Pizza::GetAllAsync SQL: {cmd.CommandText}");
            return await ReadAllAsync(await cmd.ExecuteReaderAsync());
        }

        public async Task<List<int>> GetAllIngredientIdsForThisPizza()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT z_id FROM pizza_has_zutat WHERE p_id = @p_id;";
            BindId(cmd);
            Console.WriteLine($"Pizza::GetAllIngredientIdsForThisPizza SQL: {cmd.CommandText}");
            return await ReadAllAsyncInt(await cmd.ExecuteReaderAsync());
        }

        public async Task<List<int>> AddIngredientToThisPizza(int z_id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO pizza_has_zutat (p_id, z_id) VALUES (@p_id, @z_id);";
            BindId(cmd);
            cmd.Parameters.Add(new NpgsqlParameter
            {
                ParameterName = "@z_id",
                DbType = DbType.Int32,
                Value = z_id,
            });
            Console.WriteLine($"Pizza::AddIngredientToThisPizza SQL: {cmd.CommandText}");
            return await ReadAllAsyncInt(await cmd.ExecuteReaderAsync());
        }

        public async Task<List<int>> RemoveIngredientFromThisPizza(int z_id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM pizza_has_zutat WHERE p_id=@p_id AND z_id=@z_id";
            BindId(cmd);
            cmd.Parameters.Add(new NpgsqlParameter
            {
                ParameterName = "@z_id",
                DbType = DbType.Int32,
                Value = z_id,
            });
            Console.WriteLine($"Pizza::RemoveIngredientFromThisPizza SQL: {cmd.CommandText}");
            return await ReadAllAsyncInt(await cmd.ExecuteReaderAsync());
        }


        private async Task<List<Pizza>> ReadAllAsync(DbDataReader reader)
        {
            var pizzas = new List<Pizza>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var pizza = new Pizza(Db)
                    {
                        p_id = reader.GetInt32(0),
                        p_name = reader.GetString(1),
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



        public async Task<int> InsertAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO pizza (p_name) VALUES (@p_name);";
            BindParams(cmd);
            await cmd.ExecuteNonQueryAsync();
            //int id_pizza = (int) cmd.LastInsertedId; // \todo Herausfinden, wie man letzte ID bei npgsql bekommt.
            //return id_pizza;
            Console.WriteLine($"Pizza::InsertAsync SQL: {cmd.CommandText}");
            return 0;
        }

        public async Task UpdateAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"UPDATE pizza SET p_name = @p_name WHERE p_id = @p_id;";
            BindParams(cmd);
            BindId(cmd);
            Console.WriteLine($"Pizza::UpdateAsync SQL: {cmd.CommandText}");
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM pizza WHERE p_id = @p_id;";
            BindId(cmd);
            Console.WriteLine($"Pizza::DeleteAsync SQL: {cmd.CommandText}");
            await cmd.ExecuteNonQueryAsync();
        }

        private void BindId(NpgsqlCommand cmd)
        {
            cmd.Parameters.Add(new NpgsqlParameter
            {
                ParameterName = "@p_id",
                DbType = DbType.Int32,
                Value = p_id,
            });
        }

        private void BindParams(NpgsqlCommand cmd)
        {
            cmd.Parameters.Add(new NpgsqlParameter
            {
                ParameterName = "@p_name",
                DbType = DbType.String,
                Value = p_name,
            });
        }
    }
}