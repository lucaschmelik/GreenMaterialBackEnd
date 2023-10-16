using DB;
using GreenMaterialBackEnd.Enumerables;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Transactions;

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
        public ActionResult PostInvoice(int userId, int productId, int cantidad)
        {
            using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            try
            {
                var invoice = new Invoice()
                {
                    userId = userId,
                    state = (int)State.Created
                };

                _context.invoices.Add(invoice);
                _context.SaveChanges();

                AddItem(productId, cantidad, invoice.id);

                transactionScope.Complete();

                return Ok();
            }
            catch (Exception e)
            {
                transactionScope.Dispose();
                return BadRequest(e.Message);
            }
        }

        private void AddItem(int productId, int cantidad, int id)
        {
            try
            {
                var item = new Item()
                {
                    cantidad = cantidad,
                    productId = productId,
                    invoiceId = id
                };

                _context.items.Add(item);

                _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
