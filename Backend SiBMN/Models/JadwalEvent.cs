using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiBMN.Models
{
    [Table("JadwalEvents")]
    public class JadwalEvent
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("user_id")]
        public int UserId { get; set; }

        [Required]
        [Column("bulan")]
        [StringLength(7)]
        public string Bulan { get; set; } = string.Empty; // Format: "2026-03"

        [Required]
        [Column("waktu")]
        [StringLength(10)]
        public string Waktu { get; set; } = string.Empty; // Format: "14:00"

        [Required]
        [Column("keterangan")]
        [StringLength(500)]
        public string Keterangan { get; set; } = string.Empty;

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        [Column("deleted_at")]
        public DateTime? DeletedAt { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; }
    }
}
