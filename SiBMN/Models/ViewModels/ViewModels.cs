using System.ComponentModel.DataAnnotations;

namespace SiBMN.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email wajib diisi")]
        [EmailAddress(ErrorMessage = "Format email tidak valid")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password wajib diisi")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }

    public class PengajuanCreateViewModel
    {
        [Required(ErrorMessage = "Nomor surat wajib diisi")]
        [Display(Name = "Nomor Surat Pengajuan")]
        public string NomorSuratText { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tanggal pengajuan wajib diisi")]
        [DataType(DataType.Date)]
        [Display(Name = "Tanggal Pengajuan")]
        public DateTime TanggalPengajuan { get; set; } = DateTime.Now;

        [Display(Name = "Tahun Anggaran")]
        public int? TahunAnggaran { get; set; }

        [Display(Name = "No. Surat Rektor")]
        public string? NoSuratRektor { get; set; }

        [Display(Name = "Jabatan Penandatangan")]
        public string? Jabatan { get; set; }

        [Display(Name = "Pejabat Penandatangan")]
        public int? IdPejabat { get; set; }

        [Display(Name = "Jenis Pengajuan")]
        public string? JenisPengajuan { get; set; }
    }

    public class DetailPengajuanCreateViewModel
    {
        public int IdPengajuan { get; set; }

        [Required(ErrorMessage = "Barang wajib dipilih")]
        [Display(Name = "Nama Barang")]
        public int IdBarang { get; set; }

        [Display(Name = "Spesifikasi")]
        public string? Spesifikasi { get; set; }

        [Required(ErrorMessage = "Volume wajib diisi")]
        [Range(1, int.MaxValue, ErrorMessage = "Volume harus lebih dari 0")]
        [Display(Name = "Volume")]
        public int JumlahDiminta { get; set; }

        [Required(ErrorMessage = "Harga satuan wajib diisi")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Harga harus lebih dari 0")]
        [Display(Name = "Harga Satuan (Rp)")]
        public decimal HargaSatuan { get; set; }

        [Required(ErrorMessage = "Lokasi ruang wajib dipilih")]
        [Display(Name = "Ruang Penempatan")]
        public int IdRuang { get; set; }

        [Display(Name = "Fungsi Barang")]
        public string? FungsiBarang { get; set; }

        [Required(ErrorMessage = "Asal barang wajib dipilih")]
        [Display(Name = "Asal Barang")]
        public string AsalBarang { get; set; } = "PDN";

        [Display(Name = "Alasan Pemilihan Produk Import")]
        public string? AlasanImport { get; set; }

        [Display(Name = "Link Survey (e-katalog)")]
        public string? LinkSurvey { get; set; }

        [Display(Name = "Link Gambar")]
        public string? LinkGambar { get; set; }
    }

    public class PengajuanDetailViewModel
    {
        public Pengajuan Pengajuan { get; set; } = null!;
        public List<DetailPengajuan> Details { get; set; } = new();
    }

    public class KodeBarangCreateViewModel
    {
        [Required(ErrorMessage = "Level wajib dipilih")]
        public string Level { get; set; } = "Golongan";

        [Required(ErrorMessage = "Kode Golongan wajib diisi")]
        [RegularExpression(@"^\d{1}$", ErrorMessage = "Kode Golongan harus 1 digit angka")]
        [Display(Name = "Kode Golongan")]
        public string KodeGolongan { get; set; } = string.Empty;

        [RegularExpression(@"^\d{2}$", ErrorMessage = "Kode Bidang harus 2 digit angka")]
        [Display(Name = "Kode Bidang")]
        public string? KodeBidang { get; set; }

        [RegularExpression(@"^\d{2}$", ErrorMessage = "Kode Kelompok harus 2 digit angka")]
        [Display(Name = "Kode Kelompok")]
        public string? KodeKelompok { get; set; }

        [RegularExpression(@"^\d{2}$", ErrorMessage = "Kode Sub Kelompok harus 2 digit angka")]
        [Display(Name = "Kode Sub Kelompok")]
        public string? KodeSubKelompok { get; set; }

        [RegularExpression(@"^\d{3}$", ErrorMessage = "Kode Barang harus 3 digit angka")]
        [Display(Name = "Kode Barang")]
        public string? KodeBarangValue { get; set; }

        [Required(ErrorMessage = "Uraian Barang wajib diisi")]
        [Display(Name = "Uraian Barang")]
        public string UraianBarang { get; set; } = string.Empty;
    }
}
