using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiBMN.Models
{
    [Table("Ruang_Gedung")]
    public class RuangGedung
    {
        [Key]
        [Column("id_ruang")]
        public int IdRuang { get; set; }

        [Column("nama_gedung")]
        [StringLength(200)]
        public string NamaGedung { get; set; } = string.Empty;

        [Required]
        [Column("nama_ruang")]
        [StringLength(200)]
        public string NamaRuang { get; set; } = string.Empty;

        [Column("id_unit")]
        public int IdUnit { get; set; }

        // Navigation
        [ForeignKey("IdUnit")]
        public Unit? Unit { get; set; }

        public ICollection<DetailPengajuan> DetailPengajuans { get; set; } = new List<DetailPengajuan>();
        public ICollection<AsetInventaris> AsetInventaris { get; set; } = new List<AsetInventaris>();
        public ICollection<MutasiAset> MutasiAsets { get; set; } = new List<MutasiAset>();
    }
}
