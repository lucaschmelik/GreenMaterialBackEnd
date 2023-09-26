using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DB
{
    public class Item
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int productId { get; set; }
        public decimal cantidad { get; set; }
        public int invoiceId { get; set; }

        [ForeignKey("invoiceId")]
        public virtual Invoice brand { get; set; }
        [ForeignKey("productId")]
        public virtual Product product { get; set; }
    }
}
