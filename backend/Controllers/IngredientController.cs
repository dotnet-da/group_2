using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    public class IngredientController : ControllerBase
    {
        public IngredientController(Database db)
        {
            Db = db;
        }

        // GET api/Book
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            Console.WriteLine("test1");
            await Db.Connection.OpenAsync();
            var query = new Ingredient(Db);
            var result = await query.GetAllAsync();
            return new OkObjectResult(result);
        }

        // GET api/Book/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(int id)
        {
            await Db.Connection.OpenAsync();
            var query = new Ingredient(Db);
            var result = await query.FindOneAsync(id);
            if (result is null)
                return new NotFoundResult();
            return new OkObjectResult(result);
        }

        // POST api/Book
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Ingredient body)
        {
            await Db.Connection.OpenAsync();
            body.Db = Db;
            int result = await body.InsertAsync();
            Console.WriteLine("inserted id=" + result);
            return new OkObjectResult(result);
        }

        // PUT api/Book/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOne(int id, [FromBody] Ingredient body)
        {
            await Db.Connection.OpenAsync();
            var query = new Ingredient(Db);
            var result = await query.FindOneAsync(id);
            if (result is null)
                return new NotFoundResult();
            result.zu_name = body.zu_name;
            result.zu_amount = body.zu_amount;
            await result.UpdateAsync();
            return new OkObjectResult(result);
        }

        // DELETE api/Book/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOne(int id)
        {
            await Db.Connection.OpenAsync();
            var query = new Ingredient(Db);
            var result = await query.FindOneAsync(id);
            if (result is null)
                return new NotFoundResult();
            await result.DeleteAsync();
            return new OkObjectResult("success");
        }

        public Database Db { get; }
    }
}