using DB;
using GreenMaterialBackEnd.Enumerables;
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

        [HttpGet("ByCurrentShippingCart")]
        public ActionResult GetProductsByCurrentShippingCart(int userId)
        {
            try
            {
                var lastInvoice = _context.invoices
                    .Where(x => x.userId == userId && x.state == (int)State.Created)
                    .OrderByDescending(x => x.id)
                    .FirstOrDefault();

                if (lastInvoice == null) { throw new Exception("El usuario no tiene facturas generadas"); }

                var cartProducts = _context.items
                    .Where(x => x.invoiceId == lastInvoice.id)
                    .Join(
                        _context.products,
                        item => item.productId,
                        producto => producto.id,
                        (item, producto) => new
                        {
                            Producto = producto,
                            Cantidad = item.cantidad
                        }
                    )
                    .ToList();

                return Ok(cartProducts.Select(x=> new
                {
                    lastInvoiceId = lastInvoice.id,
                    products = cartProducts
                }));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
