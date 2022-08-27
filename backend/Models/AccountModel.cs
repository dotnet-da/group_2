using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using backend;
using Npgsql;

namespace backend
{
    public class Account
    {
        public int ac_id { get; set; }
        public string ac_username { get; set; }
        public string ac_password { get; set; }
        public string ac_type { get; set; }

        internal Database Db { get; set; }

        public Account()
        {
        }

        internal Account(Database db)
        {
            Db = db;
        }

        public async Task<Account> FindOneAsync(int id_user)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT * FROM stjucloo.accounts WHERE ac_id= @ac_id";
            cmd.Parameters.Add(new NpgsqlParameter
            {
                ParameterName = "@ac_id",
                DbType = DbType.Int32,
                Value = id_user,
            });

            Console.WriteLine($"Account::FindOneAsync SQL: {cmd.CommandText}");

            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }

        public async Task<List<Account>> GetAllAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT * FROM stjucloo.accounts;";
            Console.WriteLine($"Account::GetAllAsync SQL: {cmd.CommandText}");
            return await ReadAllAsync(await cmd.ExecuteReaderAsync());
        }

        public async Task DeleteAllAsync()
        {
            using var txn = await Db.Connection.BeginTransactionAsync();
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `accounts`";
            Console.WriteLine($"Account::DeleteAllAsync SQL: {cmd.CommandText}");
            await cmd.ExecuteNonQueryAsync();
            await txn.CommitAsync();
        }

        private async Task<List<Account>> ReadAllAsync(DbDataReader reader)
        {
            var accounts = new List<Account>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var account = new Account(Db)
                    {
                        ac_id = reader.GetInt32(0),
                        ac_username = reader.GetString(1),
                        ac_password = reader.GetString(2),
                        ac_type = reader.GetString(3),
                    };
                    accounts.Add(account);
                }
            }
            return accounts;
        }


        public async Task<int> InsertAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO stjucloo.accounts (ac_username, ac_password, ac_type) VALUES (@ac_username, @ac_password, @ac_type);";
            BindParams(cmd);
            Console.WriteLine($"Account::InsertAsync SQL: {cmd.CommandText}");
            await cmd.ExecuteNonQueryAsync();
            return 0;
        }

        public async Task UpdateAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"UPDATE stjucloo.accounts SET ac_username = @ac_username, ac_password = @ac_password, ac_type = @ac_type WHERE ac_id = @ac_id;";
            BindParams(cmd);
            BindId(cmd);
            Console.WriteLine($"Account::UpdateAsync SQL: {cmd.CommandText}");
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM stjucloo.accounts WHERE ac_id = @ac_id;";
            BindId(cmd);
            Console.WriteLine($"Account::DeleteAsync SQL: {cmd.CommandText}");
            await cmd.ExecuteNonQueryAsync();
        }

        private void BindId(NpgsqlCommand cmd)
        {
            cmd.Parameters.Add(new NpgsqlParameter
            {
                ParameterName = "@ac_id",
                DbType = DbType.Int32,
                Value = ac_id,
            });
        }

        private void BindParams(NpgsqlCommand cmd)
        {
            cmd.Parameters.Add(new NpgsqlParameter
            {
                ParameterName = "@ac_username",
                DbType = DbType.String,
                Value = ac_username,
            });
            cmd.Parameters.Add(new NpgsqlParameter
            {
                ParameterName = "@ac_password",
                DbType = DbType.String,
                Value = ac_password,
            });
            cmd.Parameters.Add(new NpgsqlParameter
            {
                ParameterName = "@ac_type",
                DbType = DbType.String,
                Value = ac_type,
            });
        }
    }
}
