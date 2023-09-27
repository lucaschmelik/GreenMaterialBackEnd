using DB;
using Microsoft.AspNetCore.Mvc;

namespace GreenMaterialBackEnd.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly GreenMaterialContext _context;

        public ProductController(GreenMaterialContext context) { _context = context; }

        [HttpGet]
        public ActionResult GetProducts()
        {
            try
            {
                return Ok(_context.products.ToList());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
