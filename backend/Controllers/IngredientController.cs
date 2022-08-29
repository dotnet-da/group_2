using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [BasicAuthorization]
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
            string roleAttemptingAccess = RoleGetter.GetRoleFromClaimsPrincipal(this.User).Result;
            if (!roleAttemptingAccess.Equals("backer"))
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
        public async Task<IActionResult> PutOne(int id, [FromBody] Ingredient body)
        {
            string roleAttemptingAccess = RoleGetter.GetRoleFromClaimsPrincipal(this.User).Result;
            if (!roleAttemptingAccess.Equals("backer"))
            {
                return StatusCode(403);
            }
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
        // PUT api/Book/5
        [HttpPut("reduceIngredient/")]
        public async Task<IActionResult> ReduceIngredient([FromBody] IngredientNameValueModel ingredientNameValueModel)
        {
            string roleAttemptingAccess = RoleGetter.GetRoleFromClaimsPrincipal(this.User).Result;
            if (!roleAttemptingAccess.Equals("backer"))
            {
                return StatusCode(403);
            }
            await Db.Connection.OpenAsync();
            var query = new Ingredient(Db);
            var result = await query.GetAllAsync();
            
            Ingredient wantedIngredient = null;
            foreach (Ingredient i in result)
            {
                string i_zutat_name = i.zu_name;
                string wanted_zutat_name = ingredientNameValueModel.zutat_name;

                if (i_zutat_name.Equals(wanted_zutat_name))
                {
                    wantedIngredient = i;
                    break;
                }
            }
            if (wantedIngredient is null)
                return new NotFoundResult();

            wantedIngredient.zu_amount = (short)(wantedIngredient.zu_amount - ingredientNameValueModel.verringern_um);
            await wantedIngredient.UpdateAsync();
            await Db.Connection.CloseAsync();
            return new OkObjectResult(wantedIngredient);
        }

        // DELETE api/Book/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOne(int id)
        {
            string roleAttemptingAccess = RoleGetter.GetRoleFromClaimsPrincipal(this.User).Result;
            if (!roleAttemptingAccess.Equals("backer"))
            {
                return StatusCode(403);
            }
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
