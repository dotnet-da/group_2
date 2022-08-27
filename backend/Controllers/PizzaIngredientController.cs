using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [BasicAuthorization]
    public class PizzaIngredientController : ControllerBase
    {
        public PizzaIngredientController(Database db)
        {
            Db = db;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetIngredientsForPizza(int id)
        {
            await Db.Connection.OpenAsync();
            var query = new PizzaIngredientModel(Db);
            var result = await query.GetAllIngredientIdsForThisPizza(id);
            if (result is null)
                return new NotFoundResult();

            return new OkObjectResult(result);
        }

        // POST api/Book
        [HttpPost]
        public async Task<IActionResult> AddIngredientToPizza([FromBody] PizzaIngredientModel body)
        {
            string roleAttemptingAccess = RoleGetter.GetRoleFromClaimsPrincipal(this.User).Result;
            if (!roleAttemptingAccess.Equals("admin"))
            {
                return StatusCode(403);
            }
            await Db.Connection.OpenAsync();
            var query = new PizzaIngredientModel(Db);
            query.z_id = body.z_id;
            query.p_id = body.p_id;

            try
            {
                await query.AddThisCombination(); //\todo Errorhandling (Foreign Keys verletzt)
                return new OkObjectResult("success");
            }
            catch(Npgsql.PostgresException pe)
            {
                if (pe.MessageText.Contains("insert or update on table \"pizza_has_zutat\" violates foreign key constraint \"myct1\"")) {                 
                    return new BadRequestObjectResult("Pizza doesn't exist");
                }
                else if (pe.MessageText.Contains("insert or update on table \"pizza_has_zutat\" violates foreign key constraint \"myct2\""))
                {
                    return new BadRequestObjectResult("Ingredient doesn't exist");
                }
            }
            return new BadRequestObjectResult("Other error");
        }

        [HttpDelete("{z_id}/{p_id}")]
        public async Task<IActionResult> RemoveIngredientFromPizza(int z_id, int p_id)
        {
            string roleAttemptingAccess = RoleGetter.GetRoleFromClaimsPrincipal(this.User).Result;
            if (!roleAttemptingAccess.Equals("admin"))
            {
                return StatusCode(403);
            }
            await Db.Connection.OpenAsync();
            var query = new PizzaIngredientModel(Db);
            query.z_id = z_id;
            query.p_id = p_id;

            var object_to_be_deleted = query.FindThisCombination();
            await Db.Connection.CloseAsync();  //\todo Prüfen, warum das hier nötig ist.
            if (object_to_be_deleted is null)
                return new NotFoundResult();
            else {
                await Db.Connection.OpenAsync();
                await query.RemoveThisCombination();
                return new OkObjectResult("success"); 
            }
        }

        public Database Db { get; }
    }
}