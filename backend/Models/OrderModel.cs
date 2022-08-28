using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using Npgsql;

namespace backend
{
    public class Order
    {
        public int backer_ac_id { get; set; } // Baker Account ID
        public int be_id { get; set; } //Order ID
        public int ac_id { get; set; } //Account ID of the one who ordered it

        public string p_name { get; set; }
        public string p_status { get; set; }

        public int p_id { get; set; }

        public int be_backerid { get; set; } = -1; //\todo redundant? (backer_ac_id ist schon da?)
        public int be_pizzaid { get; set; } = -1; //\todo redundant? (p_id ist schon da?)

        public bool be_ready { get; set; } 
        internal Database Db { get; set; }

        public Order()
        {
        }

        internal Order(Database db)
        {
            Db = db;
        }

        public async Task<Order> FindOneAsync(int id_order)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT backer_ac_id, ac_id, be_status, p.p_name
                                FROM stjucloo.bestellungen
                                join stjucloo.pizza p on bestellungen.p_id = p.p_id 
                                WHERE be_id = @be_id;";
            cmd.Parameters.Add(new NpgsqlParameter
            {
                ParameterName = "@be_id",
                DbType = DbType.Int32,
                Value = id_order,
            });
            Console.WriteLine($"Order::FindOneAsync SQL: {cmd.CommandText}");
            var result = await ReadOneAsync(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }

        public async Task<List<Order>> GetAllAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT backer_ac_id, be_id, ac_id, be_status, p.p_name 
                                FROM stjucloo.bestellungen
                                join stjucloo.pizza p on p.p_id = bestellungen.p_id;";
            Console.WriteLine($"Order::GetAllAsync SQL: {cmd.CommandText}");
            return await ReadAllAsync(await cmd.ExecuteReaderAsync());
        }

        private async Task<List<Order>> ReadAllAsync(DbDataReader reader)
        {
            var orders = new List<Order>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var order = new Order(Db)
                    {
                        backer_ac_id = reader.GetInt32(0),
                        be_id = reader.GetInt32(1),
                        ac_id = reader.GetInt32(2),
                        p_status = reader.GetString(3),
                        p_name = reader.GetString(4)
                    };
                    orders.Add(order);
                }
            }
            return orders;
        }

        private async Task<List<Order>> ReadOneAsync(DbDataReader reader)
        {
            var orders = new List<Order>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var order = new Order(Db)
                    {
                        backer_ac_id = reader.GetInt32(0),
                        ac_id = reader.GetInt32(1),
                        p_status = reader.GetString(2),
                        p_name = reader.GetString(3)
                    };
                    orders.Add(order);
                }
            }
            return orders;
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

        private async Task<int> ReadOneAsyncInt(DbDataReader reader)
        {
            using (reader)
            {
                return reader.GetInt32(0);
            }
        }



        public async Task<int> InsertAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO stjucloo.bestellungen (backer_ac_id, ac_id, p_id, be_status) VALUES (10 @ac_id, @p_id, 'Order given' );"; //\todo prüfen
            BindParams(cmd);
            Console.WriteLine($"Order::InsertAsync SQL: {cmd.CommandText}");
            //int id_pizza = (int) cmd.LastInsertedId; // \todo Herausfinden, wie man letzte ID bei npgsql bekommt.
            //return id_pizza;
            await cmd.ExecuteNonQueryAsync();
            
            return 1;
        }

        public async Task UpdateAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"UPDATE stjucloo.bestellungen SET backer_ac_id = @backer_ac_id, ac_id=@ac_id, p_id=@p_id, be_ready=@be_ready, be_backerid=-1  WHERE be_id = @be_id;";
            BindParams(cmd);
            BindId(cmd);
            Console.WriteLine($"Order::UpdateAsync SQL: {cmd.CommandText}");
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM stjucloo.bestellungen WHERE be_id = @be_id;";
            BindId(cmd);
            Console.WriteLine($"Order::DeleteAsync SQL: {cmd.CommandText}");
            await cmd.ExecuteNonQueryAsync();
        }

        private void BindId(NpgsqlCommand cmd)
        {
            cmd.Parameters.Add(new NpgsqlParameter
            {
                ParameterName = "@be_id",
                DbType = DbType.Int32,
                Value = be_id,
            });
        }

        private void BindParams(NpgsqlCommand cmd)
        {
            cmd.Parameters.Add(new NpgsqlParameter
            {
                ParameterName = "@backer_ac_id",
                DbType = DbType.Int32,
                Value = backer_ac_id,
            });
            cmd.Parameters.Add(new NpgsqlParameter
            {
                ParameterName = "@ac_id",
                DbType = DbType.Int32,
                Value = ac_id,
            });
            cmd.Parameters.Add(new NpgsqlParameter
            {
                ParameterName = "@p_id",
                DbType = DbType.Int32,
                Value = p_id,
            });
            cmd.Parameters.Add(new NpgsqlParameter
            {
                ParameterName = "@be_backerid",
                DbType = DbType.Int32,
                Value = be_backerid,
            });
            cmd.Parameters.Add(new NpgsqlParameter
            {
                ParameterName = "@be_pizzaid",
                DbType = DbType.Int32,
                Value = be_pizzaid,
            });
            cmd.Parameters.Add(new NpgsqlParameter
            {
                ParameterName = "@be_ready",
                DbType = DbType.Boolean,
                Value = be_ready,
            });
        }
    }
}