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

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        [Column("deleted_at")]
        public DateTime? DeletedAt { get; set; }

        // === Per-stage approval tracking ===

        // Stage 1: Operator submits
        [Column("submitted_at")]
        public DateTime? SubmittedAt { get; set; }

        // Stage 2: Pimpinan Unit Kerja
        [Column("pimpinan_unit_approved_by")]
        public int? PimpinanUnitApprovedBy { get; set; }
        [Column("pimpinan_unit_approved_at")]
        public DateTime? PimpinanUnitApprovedAt { get; set; }

        // Stage 3: WR BPKU
        [Column("wr_bpku_approved_by")]
        public int? WrBpkuApprovedBy { get; set; }
        [Column("wr_bpku_approved_at")]
        public DateTime? WrBpkuApprovedAt { get; set; }

        // Stage 4: Kabiro BPKU
        [Column("kabiro_bpku_approved_by")]
        public int? KabiroBpkuApprovedBy { get; set; }
        [Column("kabiro_bpku_approved_at")]
        public DateTime? KabiroBpkuApprovedAt { get; set; }

        // Stage 5 & 6: Tim BMN review + Pimpinan BMN approve
        [Column("reviewed_by")]
        public int? ReviewedBy { get; set; }
        [Column("reviewed_at")]
        public DateTime? ReviewedAt { get; set; }
        [Column("approved_by")]
        public int? ApprovedBy { get; set; }
        [Column("approved_at")]
        public DateTime? ApprovedAt { get; set; }

        // Stage 7: Kabag Umum
        [Column("kabag_umum_approved_by")]
        public int? KabagUmumApprovedBy { get; set; }
        [Column("kabag_umum_approved_at")]
        public DateTime? KabagUmumApprovedAt { get; set; }

        // Navigation
        [ForeignKey("UnitId")]
        public Unit? Unit { get; set; }

        [ForeignKey("IdPejabat")]
        public User? Pejabat { get; set; }

        [ForeignKey("ReviewedBy")]
        public User? Reviewer { get; set; }

        [ForeignKey("ApprovedBy")]
        public User? Approver { get; set; }

        [ForeignKey("PimpinanUnitApprovedBy")]
        public User? PimpinanUnitApprover { get; set; }

        [ForeignKey("WrBpkuApprovedBy")]
        public User? WrBpkuApprover { get; set; }

        [ForeignKey("KabiroBpkuApprovedBy")]
        public User? KabiroBpkuApprover { get; set; }

        [ForeignKey("KabagUmumApprovedBy")]
        public User? KabagUmumApprover { get; set; }

        public ICollection<DetailPengajuan> DetailPengajuans { get; set; } = new List<DetailPengajuan>();
        public ICollection<PenerimaanBarang> PenerimaanBarangs { get; set; } = new List<PenerimaanBarang>();
    }
}
