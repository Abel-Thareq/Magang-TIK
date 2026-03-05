using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiBMN.Models
{
    [Table("Master_Barang")]
    public class MasterBarang
    {
        [Key]
        [Column("id_barang")]
        public int IdBarang { get; set; }

        [Column("id_kategori")]
        public int IdKategori { get; set; }

        [Required]
        [Column("nama_barang")]
        [StringLength(300)]
        public string NamaBarang { get; set; } = string.Empty;

        [Column("spesifikasi")]
        public string? Spesifikasi { get; set; }

        [Required]
        [Column("satuan")]
        [StringLength(100)]
        public string Satuan { get; set; } = string.Empty;

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        [Column("deleted_at")]
        public DateTime? DeletedAt { get; set; }

        // Navigation
        [ForeignKey("IdKategori")]
        public KategoriBarang? KategoriBarang { get; set; }

        public ICollection<StokPersediaan> StokPersediaans { get; set; } = new List<StokPersediaan>();
        public ICollection<AsetInventaris> AsetInventaris { get; set; } = new List<AsetInventaris>();
    }
}
