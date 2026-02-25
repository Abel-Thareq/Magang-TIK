using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiBMN.Models
{
    [Table("Pengajuan")]
    public class Pengajuan
    {
        [Key]
        [Column("id_pengajuan")]
        public int IdPengajuan { get; set; }

        [Column("nomor_surat")]
        public int? NomorSurat { get; set; }

        [Column("no_surat_rektor")]
        [StringLength(200)]
        public string? NoSuratRektor { get; set; }

        [Column("tgl_surat_rektor")]
        [DataType(DataType.Date)]
        public DateTime? TglSuratRektor { get; set; }

        [Column("id_pejabat")]
        public int? IdPejabat { get; set; }

        [Column("jabatan")]
        [StringLength(200)]
        public string? Jabatan { get; set; }

        [Column("total_harga")]
        public decimal TotalHarga { get; set; }

        [Column("unit_id")]
        public int UnitId { get; set; }

        [Column("tanggal_pengajuan")]
        [DataType(DataType.Date)]
        public DateTime TanggalPengajuan { get; set; }

        [Column("jenis_pengajuan")]
        [StringLength(200)]
        public string? JenisPengajuan { get; set; }

        [Column("status")]
        [StringLength(50)]
        public string Status { get; set; } = "draft";

        [Column("tahun_anggaran")]
        public int? TahunAnggaran { get; set; }

        // Navigation
        [ForeignKey("UnitId")]
        public Unit? Unit { get; set; }

        [ForeignKey("IdPejabat")]
        public User? Pejabat { get; set; }

        public ICollection<DetailPengajuan> DetailPengajuans { get; set; } = new List<DetailPengajuan>();
        public ICollection<PenerimaanBarang> PenerimaanBarangs { get; set; } = new List<PenerimaanBarang>();
    }
}
