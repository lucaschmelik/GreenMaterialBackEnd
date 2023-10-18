using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace DB
{
    public class Invoice
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int userId { get; set; }
        public int state { get; set; }
        public bool isCurrent { get; set; }

        [ForeignKey("userId")]
        [JsonIgnore]
        public virtual User user { get; set; }
        [JsonIgnore]
        public virtual Collection<Item> items { get; set; }
    }
}
