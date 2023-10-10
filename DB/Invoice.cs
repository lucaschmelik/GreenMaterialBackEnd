using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.ObjectModel;

namespace DB
{
    public class Invoice
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int userId { get; set; }
        public int state { get; set; }

        [ForeignKey("userId")]
        public virtual User user { get; set; } 

        public virtual Collection<Item> items { get; set; }
    }
}
