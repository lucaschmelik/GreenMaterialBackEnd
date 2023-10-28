using DB;
using GreenMaterialBackEnd.Enumerables;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GreenMaterialBackEnd.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly GreenMaterialContext _context;

        public PaymentController(GreenMaterialContext context) { _context = context; }

        [HttpGet]
        public ActionResult<object> GetResumeByUserId(int userId)
        {
            try
            {
                var lastInvoice = _context.invoices.FirstOrDefault(
                    x => x.isCurrent &&
                    x.userId == userId &&
                    x.state == (int)StateEnum.Pago);

                if (lastInvoice == null)
                {
                    return BadRequest("No se encontró la factura a partir del usuario.");
                }

                var delivery = _context.deliveries.FirstOrDefault(x => x.invoiceId == lastInvoice.id);

                if (delivery == null)
                {
                    return BadRequest("El usuario no tiene una factura con envio.");
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

                return Ok(new
                {
                    deliveryPrice = delivery.price,
                    itemsPrice = totalSum
                });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public ActionResult PostPayment(int userId, Payment payment)
        {
            try
            {
                if (payment == null)
                {
                    return BadRequest("El pago tiene errores");
                }

                var currentInvoice = _context.invoices.FirstOrDefault(
                    x => x.isCurrent &&
                    x.userId == userId &&
                    x.state == (int)StateEnum.Pago);

                if (currentInvoice == null)
                {
                    return BadRequest("El usuario no tiene una factura en curso");
                }

                currentInvoice.state = (int)StateEnum.Pagado;
                currentInvoice.isCurrent = false;

                var newPayment = new Payment()
                {
                    invoiceId = currentInvoice.id,
                    name = payment.name,
                    mmyy = payment.mmyy,
                    cvc = payment.cvc,
                    number = payment.number,
                    paymentType = payment.paymentType
                };

                if (newPayment.paymentType != (int)PaymentTypeEnum.Efectivo &&
                    newPayment.paymentType != (int)PaymentTypeEnum.MercadoPago &&
                    newPayment.paymentType != (int)PaymentTypeEnum.Tarjeta ||
                    (newPayment.paymentType == (int)PaymentTypeEnum.Tarjeta && (
                    newPayment.name == string.Empty ||
                    newPayment.mmyy == string.Empty ||
                    newPayment.cvc == string.Empty ||
                    newPayment.number == string.Empty)))
                {
                    return BadRequest("Existe un error al momento de elegir el tipo de pago.");
                }

                _context.payments.Add(newPayment);

                _context.SaveChanges();

                return Ok(newPayment.id);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
