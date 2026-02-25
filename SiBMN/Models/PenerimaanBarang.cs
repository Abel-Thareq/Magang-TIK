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

        // Navigation
        [ForeignKey("IdPengajuan")]
        public Pengajuan? Pengajuan { get; set; }
    }
}
