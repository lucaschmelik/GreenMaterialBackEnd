using DB;
using Microsoft.AspNetCore.Mvc;

namespace GreenMaterialBackEnd.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ItemController : Controller
    {
        private readonly GreenMaterialContext _context;

        public ItemController(GreenMaterialContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult GetItems()
        {
            try
            {
                return Ok(_context.items.ToList());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
