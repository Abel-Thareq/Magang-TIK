using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiBMN.Models
{
    [Table("Roles")]
    public class Role
    {
        [Key]
        [Column("id_role")]
        public int IdRole { get; set; }

        [Required]
        [Column("nama_role")]
        [StringLength(50)]
        public string NamaRole { get; set; } = string.Empty;

        // Navigation
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
