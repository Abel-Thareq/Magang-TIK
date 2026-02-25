using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiBMN.Models
{
    [Table("Mutasi_Aset")]
    public class MutasiAset
    {
        [Key]
        [Column("id_mutasi")]
        public int IdMutasi { get; set; }

        [Column("id_aset")]
        public int IdAset { get; set; }

        [Column("id_ruang")]
        public int IdRuang { get; set; }

        [Column("tanggal_mutasi")]
        [StringLength(100)]
        public string? TanggalMutasi { get; set; }

        // Navigation
        [ForeignKey("IdAset")]
        public AsetInventaris? AsetInventaris { get; set; }

        [ForeignKey("IdRuang")]
        public RuangGedung? RuangGedung { get; set; }
    }
}
