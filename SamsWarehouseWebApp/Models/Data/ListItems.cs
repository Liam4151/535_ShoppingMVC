using System.ComponentModel.DataAnnotations;

namespace SamsWarehouseWebApp.Models.Data
{
    public class ListItems
    {
        [Key]
        [Required]
        public int ListItemsId { get; set; }
        [Required]
        public int ListId { get; set; }
        [Required]
        public int ItemId { get; set; }
        [Required]
        [Range(1,99)]
        public int Quantity { get; set; }
        public Item Item { get; set; }
        public List List { get; set; }
    }
}
