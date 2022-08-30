using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using Npgsql;

namespace backend
{
    public class Ingredient
    {
        public int zu_id { get; set; }
        public string zu_name { get; set; }

        public short zu_amount { get; set; } // \todo Datentyp prüfen (smallint in SQL)
        internal Database Db { get; set; }

        public Ingredient()
        {
        }

        internal Ingredient(Database db)
        {
            Db = db;
        }

        public async Task<Ingredient> FindOneAsync(int id_ingredient)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT * FROM stjucloo.zutaten WHERE zu_id = @id_ingredient";
            cmd.Parameters.Add(new NpgsqlParameter
            {
                ParameterName = "@id_ingredient",
                DbType = DbType.Int32,
                Value = id_ingredient,
            });
            Console.WriteLine($"Ingredient::FindOneAsync SQL: {cmd.CommandText}");
            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }

        public async Task<List<Ingredient>> GetAllAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT * FROM stjucloo.zutaten;";
            Console.WriteLine($"Ingredient::GetAllAsync SQL: {cmd.CommandText}");
            return await ReadAllAsync(await cmd.ExecuteReaderAsync());
        }


        private async Task<List<Ingredient>> ReadAllAsync(DbDataReader reader)
        {
            var ingredients = new List<Ingredient>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var ingredient = new Ingredient(Db)
                    {
                        zu_id = reader.GetInt32(0),
                        zu_name = reader.GetString(1),
                        zu_amount = reader.GetInt16(2)
                    };
                    ingredients.Add(ingredient);
                }
            }
            return ingredients;
        }
    

        public async Task<int> InsertAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO stjucloo.zutaten (zu_name, zu_amount) VALUES (@zu_name, @zu_amount) RETURNING zu_id;";
            BindParams(cmd);
            Console.WriteLine($"Ingredient::InsertAsync SQL: {cmd.CommandText}");
            //int id_pizza = (int) cmd.LastInsertedId; // \todo Herausfinden, wie man letzte ID bei npgsql bekommt.
            //return id_pizza;
            var id = cmd.ExecuteScalarAsync().Result;
            return (int)id;
        }

        public async Task UpdateAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"UPDATE stjucloo.zutaten SET zu_name = @zu_name, zu_amount = @zu_amount WHERE zu_id = @zu_id;";
            BindParams(cmd);
            BindId(cmd);
            Console.WriteLine($"Ingredient::UpdateAsync SQL: {cmd.CommandText}");
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM stjucloo.zutaten WHERE zu_id = @zu_id;";
            BindId(cmd);
            Console.WriteLine($"Ingredient::DeleteAsync SQL: {cmd.CommandText}");
            await cmd.ExecuteNonQueryAsync();
        }

        private void BindId(NpgsqlCommand cmd)
        {
            cmd.Parameters.Add(new NpgsqlParameter
            {
                ParameterName = "@zu_id",
                DbType = DbType.Int32,
                Value = zu_id,
            });
        }

        private void BindParams(NpgsqlCommand cmd)
        {
            cmd.Parameters.Add(new NpgsqlParameter
            {
                ParameterName = "@zu_name",
                DbType = DbType.String,
                Value = zu_name,
            });
            cmd.Parameters.Add(new NpgsqlParameter
            {
                ParameterName = "@zu_amount",
                DbType = DbType.Int16,
                Value = zu_amount,
            });
        }
    }
}
