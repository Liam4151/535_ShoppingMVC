using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SamsWarehouseWebApp.Models.Data
{
    public class AppUser
    {
        [Key]
        public int UserId { get; set; }
        [Column("PasswordHash")]
        [StringLength(100)]
        [Required]
        [DataType(DataType.Password)]
        public string PasswordHash { get; set; }
        [EmailAddress]
        [StringLength(50)]
        public string Email { get; set; }
        public virtual ICollection<List> Lists { get; set; }
    }
}
