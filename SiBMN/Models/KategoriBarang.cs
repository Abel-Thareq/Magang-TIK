using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiBMN.Models
{
    [Table("Kategori_Barang")]
    public class KategoriBarang
    {
        [Key]
        [Column("id_kategori")]
        public int IdKategori { get; set; }

        [Required]
        [Column("nama_kategori")]
        [StringLength(200)]
        public string NamaKategori { get; set; } = string.Empty;

        // Navigation
        public ICollection<MasterBarang> MasterBarangs { get; set; } = new List<MasterBarang>();
    }
}
