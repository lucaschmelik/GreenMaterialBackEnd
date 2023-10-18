using DB;
using GreenMaterialBackEnd.Enumerables;
using Microsoft.AspNetCore.Mvc;

namespace GreenMaterialBackEnd.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly GreenMaterialContext _context;

        public InvoiceController(GreenMaterialContext context) { _context = context; }

        [HttpGet]
        public ActionResult GetInvoices()
        {
            try
            {
                return Ok(_context.invoices.ToList());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("ByUser")]
        public ActionResult GetInvoicesByUser(int userId)
        {
            try
            {
                return Ok(_context.invoices.Where(x => x.user.id == userId).ToList());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public ActionResult<Invoice> PostInvoice(int userId)
        {
            try
            {
                var currentCreateInvoice = _context.invoices.FirstOrDefault(
                    x => x.isCurrent &&
                    x.userId == userId && (
                    x.state == (int)State.Created ||
                    x.state == (int)State.Confirmed ||
                    x.state == (int)State.NotPayed
                    ));

                if (currentCreateInvoice != null)
                {
                    return Ok(currentCreateInvoice.id);
                }

                var invoice = new Invoice()
                {
                    userId = userId,
                    state = (int)State.Created,
                    isCurrent = true
                };

                _context.invoices.Add(invoice);

                _context.SaveChanges();

                return Ok(invoice.id);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        private Invoice? GetLastInvoice(int userId, State state)
        {
            return _context.invoices
                    .Where(x => x.userId == userId && x.state == (int)state)
                    .OrderByDescending(x => x.id)
                    .FirstOrDefault();
        }

        [HttpPost("AddItem")]
        public ActionResult AddItem(int invoiceId, int productId, int cantidad)
        {
            try
            {
                var item = new Item()
                {
                    cantidad = cantidad,
                    productId = productId,
                    invoiceId = invoiceId
                };

                _context.items.Add(item);

                _context.SaveChanges();

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("ChangeState")]
        public ActionResult ChangeState(int userId, int nextState)
        {
            try
            {
                var lastInvoice = _context.invoices.FirstOrDefault(
                    x => x.isCurrent &&
                    x.userId == userId);

                if (lastInvoice == null)
                {
                    return Ok();
                }

                lastInvoice.state = nextState;

                _context.SaveChanges();

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("GetTotalAmountByStateAndUser")]
        public ActionResult<decimal> GetTotalAmountByStateAndUser(int userId, int state)
        {
            try
            {
                var lastInvoice = _context.invoices.FirstOrDefault(
                    x => x.isCurrent &&
                    x.userId == userId &&
                    x.state == state);

                if (lastInvoice == null)
                {
                    return BadRequest();
                }

                var totalSum = _context.items
                    .Where(x => x.invoiceId == lastInvoice.id)
                    .Join(_context.products, item => item.productId, product => product.id, (item, product) => new
                    {
                        item.productId,
                        item.cantidad,
                        precio = product.price
                    })
                    .Sum(item => item.cantidad * item.precio);

                return Ok(totalSum);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("HasInvoiceByStateAndUser")]
        public ActionResult<bool> HasInvoiceByStateAndUser(int userId)
        {
            try
            {
                var lastInvoice = _context.invoices.FirstOrDefault(
                    x => x.isCurrent && 
                         x.userId == userId &&
                         x.state == (int)State.Confirmed);

                return Ok(lastInvoice != null);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}
