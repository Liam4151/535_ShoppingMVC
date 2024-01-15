using System.ComponentModel.DataAnnotations;

namespace SamsWarehouseWebApp.Models.Data
{
    public class List
    {
        [Key]
        [Required]
        public int ListId { get; set; }
        [Required]
        [StringLength(100, MinimumLength =1)]
        public string ListName { get; set; }
        [Required]
        public DateTime Created { get; set; } = DateTime.Now;
        [Required]
        public int UserId { get; set; }
        public AppUser User { get; set; }
        public virtual ICollection<ListItems> ListItems { get; set; }
    }
}
