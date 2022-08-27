using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using backend;

namespace MyApi.Controllers
{
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        public LoginController(Database db)
        {
            Db = db;
        }


        // POST api/login
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Account body)
        {
            Console.WriteLine($"Username: {body.ac_username}, Password: {body.ac_password}, Type: {body.ac_type}");
            await Db.Connection.OpenAsync();
            var query = new Login(Db);
            var passwordFromDatabase = await query.GetPassword(body.ac_username);

            if (passwordFromDatabase is null || !BCrypt.Net.BCrypt.Verify(body.ac_password, passwordFromDatabase))
            {
                // authentication failed
                return new OkObjectResult(false);
            }
            else
            {
                // authentication successful
                await Db.Connection.CloseAsync();
                await Db.Connection.OpenAsync();
                return new OkObjectResult(await query.GetUserType(body.ac_username));
            }

        }



        public Database Db { get; }
    }
}