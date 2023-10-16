using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DB
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string description { get; set; }
        public string subdescription { get; set; }
        public string detail { get; set; }
        public decimal price { get; set; }
        public string color { get;set; }
        public int stock { get;set; }

        [JsonIgnore]
        public ICollection<Item> items { get; set; }
    }
}
