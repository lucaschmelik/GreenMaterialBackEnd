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
                var lastInvoice = GetLastInvoice(userId, State.Created);
                if (lastInvoice != null)
                {
                    return Ok(lastInvoice.id);
                }

                var invoice = new Invoice()
                {
                    userId = userId,
                    state = (int)State.Created
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
        public ActionResult ChangeState(int userId, int currentState, int nextState)
        {
            try
            {
                var lastInvoice = GetLastInvoice(userId, (State)Enum.ToObject(typeof(State), currentState));

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
                var lastInvoice = GetLastInvoice(userId, (State)Enum.ToObject(typeof(State), state));

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
        public ActionResult<bool> HasInvoiceByStateAndUser(int userId, int state)
        {
            try
            {
                var lastInvoice = GetLastInvoice(userId, (State)Enum.ToObject(typeof(State), state));

                if (lastInvoice == null)
                {
                    return BadRequest();
                }

                return Ok(true);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}
