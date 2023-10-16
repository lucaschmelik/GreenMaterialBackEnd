using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

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
        [JsonIgnore]
        public virtual Invoice invoice { get; set; }
        [ForeignKey("productId")]
        [JsonIgnore]
        public virtual Product product { get; set; }
    }
}
