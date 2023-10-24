using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DB
{
    public class Payment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string name { get; set; }
        public string number { get; set; }
        public string mmyy { get; set; }
        public string cvc { get; set; }
        public int paymentType { get; set; }
        public int invoiceId { get; set; }
    }
}
