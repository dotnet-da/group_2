using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using backend;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        public AccountController(Database db)
        {
            Db = db;
        }

        // GET api/User
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            await Db.Connection.OpenAsync();
            var query = new Account(Db);
            var result = await query.GetAllAsync();
            Console.WriteLine("Test");
            return new OkObjectResult(result);
        }

        // GET api/User/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(int id)
        {
            await Db.Connection.OpenAsync();
            var query = new Account(Db);
            var result = await query.FindOneAsync(id);
            if (result is null)
                return new NotFoundResult();
            return new OkObjectResult(result);
        }

        // POST api/User
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Account body)
        {
            await Db.Connection.OpenAsync();
            body.ac_password = BCrypt.Net.BCrypt.HashPassword(body.ac_password);
            body.Db = Db;
            await body.InsertAsync();
            return new OkObjectResult(body);
        }

        // PUT api/User/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOne(int id, [FromBody] Account body)
        {
            await Db.Connection.OpenAsync();
            var query = new Account(Db);
            body.ac_password = BCrypt.Net.BCrypt.HashPassword(body.ac_password);
            var result = await query.FindOneAsync(id);
            if (result is null)
                return new NotFoundResult();
            result.ac_username = body.ac_username;
            result.ac_password = body.ac_password;
            result.ac_type = body.ac_type;
            await result.UpdateAsync();
            return new OkObjectResult("success");
        }

        // DELETE api/User/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOne(int id)
        {
            await Db.Connection.OpenAsync();
            var query = new Account(Db);
            var result = await query.FindOneAsync(id);
            if (result is null)
                return new NotFoundResult();
            await result.DeleteAsync();
            return new OkObjectResult("success");
        }


        public Database Db { get; }
    }
}