using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiBMN.Models
{
    [Table("Aset_Inventaris")]
    public class AsetInventaris
    {
        [Key]
        [Column("id_aset")]
        public int IdAset { get; set; }

        [Column("id_barang")]
        public int IdBarang { get; set; }

        [Column("kode_inventaris")]
        public int KodeInventaris { get; set; }

        [Column("kondisi")]
        [StringLength(100)]
        public string? Kondisi { get; set; }

        [Column("id_ruang")]
        public int IdRuang { get; set; }

        // Navigation
        [ForeignKey("IdBarang")]
        public MasterBarang? MasterBarang { get; set; }

        [ForeignKey("IdRuang")]
        public RuangGedung? RuangGedung { get; set; }

        public ICollection<MutasiAset> MutasiAsets { get; set; } = new List<MutasiAset>();
    }
}
