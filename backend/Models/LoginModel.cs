using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using backend;
using Npgsql;

namespace backend
{
    public class Login
    {
        public int id_user { get; set; }
        public string username { get; set; }
        public string password { get; set; }

        public string ac_type { get; set; }

        internal Database Db { get; set; }

        public Login()
        {
        }

        internal Login(Database db)
        {
            Db = db;
        }


        public async Task<string> GetPassword(string username)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT ac_password  FROM stjucloo.accounts WHERE ac_username = @ac_username";
            cmd.Parameters.Add(new NpgsqlParameter
            {
                ParameterName = "@ac_username",
                DbType = DbType.String,
                Value = username,
            });
            Console.WriteLine($"Login::GetPassword SQL: {cmd.CommandText}");
            var result = await ReadPassword(await cmd.ExecuteReaderAsync());
            //return lengt(result) > 0 ? result : null;
            return result;
        }

        public async Task<string> GetUserType(string username)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT ac_type  FROM stjucloo.accounts WHERE ac_username = @ac_username";
            cmd.Parameters.Add(new NpgsqlParameter
            {
                ParameterName = "@ac_username",
                DbType = DbType.String,
                Value = username,
            });
            Console.WriteLine($"Login::GetUserType SQL: {cmd.CommandText}");
            var result = await ReadPassword(await cmd.ExecuteReaderAsync()); // Funktioniert, weil nur 1 Wert geladen wird (wie in GetPassword)
            //return lengt(result) > 0 ? result : null;
            return result;
        }

        private async Task<string> ReadPassword(DbDataReader reader)
        {
            var loginUser = new Login();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var user = new Login(Db)
                    {
                        password = reader.GetString(0)
                    };
                    loginUser = user;
                }
            }

            return loginUser.password;
        }


    }
}