using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiBMN.Models
{
    [Table("Penerimaan_Barang")]
    public class PenerimaanBarang
    {
        [Key]
        [Column("id_penerimaan")]
        public int IdPenerimaan { get; set; }

        [Column("id_pengajuan")]
        public int IdPengajuan { get; set; }

        [Column("tanggal_terima")]
        [DataType(DataType.Date)]
        public DateTime TanggalTerima { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        [Column("deleted_at")]
        public DateTime? DeletedAt { get; set; }

        // Navigation
        [ForeignKey("IdPengajuan")]
        public Pengajuan? Pengajuan { get; set; }
    }
}
