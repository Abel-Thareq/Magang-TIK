using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiBMN.Models
{
    [Table("Users")]
    public class User
    {
        [Key]
        [Column("id_user")]
        public int IdUser { get; set; }

        [Required]
        [Column("nama")]
        [StringLength(200)]
        public string Nama { get; set; } = string.Empty;

        [Required]
        [Column("email")]
        [StringLength(200)]
        public string Email { get; set; } = string.Empty;

        [Column("password")]
        [StringLength(200)]
        public string Password { get; set; } = string.Empty;

        [Column("role_id")]
        public int RoleId { get; set; }

        [Column("unit_id")]
        public int UnitId { get; set; }

        // Navigation
        [ForeignKey("RoleId")]
        public Role? Role { get; set; }

        [ForeignKey("UnitId")]
        public Unit? Unit { get; set; }
    }
}
