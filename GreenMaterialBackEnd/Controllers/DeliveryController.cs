using DB;
using GreenMaterialBackEnd.Enumerables;
using Microsoft.AspNetCore.Mvc;

namespace GreenMaterialBackEnd.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DeliveryController : ControllerBase
    {
        private readonly GreenMaterialContext _context;

        public DeliveryController(GreenMaterialContext context) { _context = context; }

        [HttpPost]
        public ActionResult<Delivery> PostDelivery(int userId, Delivery delivery)
        {
            try
            {
                if (delivery == null)
                {
                    return BadRequest("El delivery enviado tiene errores");
                }

                var currentInvoice = _context.invoices.FirstOrDefault(
                    x => x.isCurrent &&
                    x.userId == userId &&
                    x.state == (int)StateEnum.Confirmed);

                if (currentInvoice == null)
                {
                    return BadRequest("El usuario no tiene una factura en curso");
                }

                currentInvoice.state = (int)StateEnum.NotPayed;

                var newDelivery = new Delivery()
                {
                    invoiceId = currentInvoice.id,
                    deliveryTypeId = delivery.deliveryTypeId,
                    name = delivery.name,
                    adress = delivery.adress,
                    phoneNumber = delivery.phoneNumber,
                    notes = delivery.notes,
                    price = delivery.price
                };

                if (delivery.deliveryTypeId != (int)DeliveryTypeEnum.Envío &&
                    delivery.deliveryTypeId != (int)DeliveryTypeEnum.Retiro ||
                    (delivery.deliveryTypeId == (int)DeliveryTypeEnum.Envío && (
                    delivery.name == string.Empty ||
                    delivery.adress == string.Empty ||
                    delivery.phoneNumber == string.Empty)))
                {
                    return BadRequest("Existe un error al momento de elegir el tipo de envío.");
                }

                _context.deliveries.Add(newDelivery);

                _context.SaveChanges();

                return Ok(newDelivery.id);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}
