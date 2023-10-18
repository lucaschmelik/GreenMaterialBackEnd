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

                if (lastInvoice == null)
                {
                    return Ok();
                }

                var cartProducts = _context.items
                    .Where(x => x.invoiceId == lastInvoice.id)
                    .Join(
                        _context.products,
                        item => item.productId,
                        producto => producto.id,
                        (item, producto) => new
                        {
                            product = producto,
                            amount = item.cantidad,
                            itemId = item.id
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

        [HttpDelete("DeleteItem")]
        public ActionResult DeleteItem(int itemId)
        {
            try
            {
                var item = _context.items.FirstOrDefault(item => item.id == itemId);

                if (item == null) return BadRequest("No se encontró el item a eliminar.");

                var invoice = _context.invoices.FirstOrDefault(x => x.id == item.invoiceId);

                _context.items.Remove(item);

                if (invoice != null && _context.items.Count(x=>x.invoiceId == invoice.id) == 1)
                {
                    _context.invoices.Remove(invoice);
                }

                _context.SaveChanges();

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
