using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiBMN.Models
{
    [Table("Stok_Persediaan")]
    public class StokPersediaan
    {
        [Key]
        [Column("id_stok")]
        public int IdStok { get; set; }

        [Column("id_barang")]
        public int IdBarang { get; set; }

        [Column("id_unit")]
        public int IdUnit { get; set; }

        [Column("jumlah_stok")]
        public int JumlahStok { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        [Column("deleted_at")]
        public DateTime? DeletedAt { get; set; }

        [ForeignKey("IdBarang")]
        public MasterBarang? MasterBarang { get; set; }

        [ForeignKey("IdUnit")]
        public Unit? Unit { get; set; }
    }
}
