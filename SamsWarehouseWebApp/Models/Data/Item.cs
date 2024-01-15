using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SamsWarehouseWebApp.Models.Data
{
    public class Item
    {
        [Key]
        [Required]
        public int ItemId { get; set; }
        [StringLength(50)]
        [Required]
        public string ItemName { get; set; }
        [StringLength(50)]
        [Required]
        public string Unit { get; set; }
        [Required]
        public double UnitPrice {get; set;}
        public virtual ICollection<ListItems> ListItems { get; set; }

    }    
}
