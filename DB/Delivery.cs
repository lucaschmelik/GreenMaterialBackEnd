using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DB
{
    public class Delivery
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int deliveryTypeId { get; set; }
        public int invoiceId { get; set; }
        public string name { get; set; }
        public string adress { get; set; }
        public string phoneNumber { get; set; }
        public string notes { get; set; }
        public decimal price { get; set; }

    }
}
