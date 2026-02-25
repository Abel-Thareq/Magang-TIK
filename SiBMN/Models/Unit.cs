using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiBMN.Models
{
    [Table("Units")]
    public class Unit
    {
        [Key]
        [Column("id_unit")]
        public int IdUnit { get; set; }

        [Required]
        [Column("nama_unit")]
        [StringLength(200)]
        public string NamaUnit { get; set; } = string.Empty;

        // Navigation
        public ICollection<User> Users { get; set; } = new List<User>();
        public ICollection<Pengajuan> Pengajuans { get; set; } = new List<Pengajuan>();
        public ICollection<StokPersediaan> StokPersediaans { get; set; } = new List<StokPersediaan>();
        public ICollection<RuangGedung> RuangGedungs { get; set; } = new List<RuangGedung>();
    }
}
