using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [BasicAuthorization]
    public class PizzaController : ControllerBase
    {
        public PizzaController(Database db)
        {
            Db = db;
        }

        // GET api/Book
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {

            Console.WriteLine("test1");
            await Db.Connection.OpenAsync();
            var query = new Pizza(Db);
            var result = await query.GetAllAsync();
            return new OkObjectResult(result);
        }

        // GET api/Book/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(int id)
        {

            await Db.Connection.OpenAsync();
            var query = new Pizza(Db);
            var result = await query.FindOneAsync(id);
            if (result is null)
                return new NotFoundResult();
            return new OkObjectResult(result);
        }

        // POST api/Book
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Pizza body)
        {
            string roleAttemptingAccess = RoleGetter.GetRoleFromClaimsPrincipal(this.User).Result;
            if (!roleAttemptingAccess.Equals("admin"))
            {
                return StatusCode(403);
            }
            await Db.Connection.OpenAsync();
            body.Db = Db;
            int result = await body.InsertAsync();
            Console.WriteLine("inserted id=" + result);
            return new OkObjectResult(result);
        }

        // PUT api/Book/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOne(int id, [FromBody] Pizza body)
        {
            string roleAttemptingAccess = RoleGetter.GetRoleFromClaimsPrincipal(this.User).Result;
            if (!roleAttemptingAccess.Equals("admin"))
            {
                return StatusCode(403);
            }
            await Db.Connection.OpenAsync();
            var query = new Pizza(Db);
            var result = await query.FindOneAsync(id);
            if (result is null)
                return new NotFoundResult();

            result.p_name = body.p_name;
            await result.UpdateAsync();
            return new OkObjectResult(result);
        }

        // DELETE api/Book/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOne(int id)
        {
            string roleAttemptingAccess = RoleGetter.GetRoleFromClaimsPrincipal(this.User).Result;
            if (!roleAttemptingAccess.Equals("admin"))
            {
                return StatusCode(403);
            }
            await Db.Connection.OpenAsync();
            var query = new Pizza(Db);
            var result = await query.FindOneAsync(id);
            if (result is null)
                return new NotFoundResult();
            await result.DeleteAsync();
            return new OkObjectResult("success");
        }

        public Database Db { get; }
    }
}