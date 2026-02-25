using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiBMN.Models
{
    [Table("Detail_Pengajuan")]
    public class DetailPengajuan
    {
        [Key]
        [Column("id_detPengajuan")]
        public int IdDetPengajuan { get; set; }

        [Column("id_pengajuan")]
        public int IdPengajuan { get; set; }

        [Column("id_barang")]
        public int IdBarang { get; set; }

        [Column("no_prioritas")]
        public int NoPrioritas { get; set; }

        [Column("id_ruang")]
        public int IdRuang { get; set; }

        [Column("jumlah_diminta")]
        public int JumlahDiminta { get; set; }

        [Column("harga_satuan")]
        public decimal HargaSatuan { get; set; }

        [Column("jumlah_harga")]
        public decimal JumlahHarga { get; set; }

        [Column("fungsi_barang")]
        public string? FungsiBarang { get; set; }

        [Column("asal_barang")]
        [StringLength(20)]
        public string AsalBarang { get; set; } = "PDN"; // "Import" or "PDN"

        [Column("alasan_import")]
        public string? AlasanImport { get; set; }

        [Column("link_gambar")]
        [StringLength(500)]
        public string? LinkGambar { get; set; }

        [Column("jumlah_disetujui")]
        public int JumlahDisetujui { get; set; }

        [Column("link_survey")]
        [StringLength(500)]
        public string? LinkSurvey { get; set; }

        [Column("gambar_ekatalog")]
        [StringLength(500)]
        public string? GambarEkatalog { get; set; }

        [Column("spesifikasi")]
        public string? Spesifikasi { get; set; }

        // Navigation
        [ForeignKey("IdPengajuan")]
        public Pengajuan? Pengajuan { get; set; }

        [ForeignKey("IdBarang")]
        public KodeBarang? KodeBarang { get; set; }

        [ForeignKey("IdRuang")]
        public RuangGedung? RuangGedung { get; set; }
    }
}
