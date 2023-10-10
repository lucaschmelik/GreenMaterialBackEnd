using DB;
using GreenMaterialBackEnd.Enumerables;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

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
        public async Task<ActionResult> PostInvoice(int userId)
        {
            try
            {
                var invoice = new Invoice()
                {
                    userId = userId,
                    state = (int)State.Created                  
                };

                _context.invoices.Add(invoice);
                await _context.SaveChangesAsync();
                return Ok(invoice);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult> CreatedInvoice([FromBody] IList<Item> items)
        {
            try
            {
                if (!items.Any())
                {
                    throw new Exception("No hay items");
                }

                var invoiceFound = await _context.invoices.FirstOrDefaultAsync(x=>x.id == items.First().invoiceId);

                if (invoiceFound == null)
                {
                    throw new Exception("Factura no encontrada");
                }

                items.ToList().ForEach(x => _context.items.Add(x));

                await _context.SaveChangesAsync();

                return Ok(items);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
