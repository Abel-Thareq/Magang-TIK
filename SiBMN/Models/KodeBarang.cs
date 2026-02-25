using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiBMN.Models
{
    [Table("Kode_Barang")]
    public class KodeBarang
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Kode Golongan wajib diisi")]
        [Column("kode_golongan")]
        [StringLength(1, MinimumLength = 1, ErrorMessage = "Kode Golongan harus 1 digit")]
        [RegularExpression(@"^\d{1}$", ErrorMessage = "Kode Golongan harus 1 digit angka")]
        public string KodeGolongan { get; set; } = string.Empty;

        [Required]
        [Column("kode_bidang")]
        [StringLength(2)]
        [RegularExpression(@"^\d{2}$", ErrorMessage = "Kode Bidang harus 2 digit angka")]
        public string KodeBidang { get; set; } = "00";

        [Required]
        [Column("kode_kelompok")]
        [StringLength(2)]
        [RegularExpression(@"^\d{2}$", ErrorMessage = "Kode Kelompok harus 2 digit angka")]
        public string KodeKelompok { get; set; } = "00";

        [Required]
        [Column("kode_sub_kelompok")]
        [StringLength(2)]
        [RegularExpression(@"^\d{2}$", ErrorMessage = "Kode Sub Kelompok harus 2 digit angka")]
        public string KodeSubKelompok { get; set; } = "00";

        [Required]
        [Column("kode_barang_value")]
        [StringLength(3)]
        [RegularExpression(@"^\d{3}$", ErrorMessage = "Kode Barang harus 3 digit angka")]
        public string KodeBarangValue { get; set; } = "000";

        [Required(ErrorMessage = "Uraian Barang wajib diisi")]
        [Column("uraian_barang")]
        [StringLength(500)]
        public string UraianBarang { get; set; } = string.Empty;

        [NotMapped]
        public string KodeBarangLengkap =>
            $"{KodeGolongan}.{KodeBidang}.{KodeKelompok}.{KodeSubKelompok}.{KodeBarangValue}";

        [NotMapped]
        public string Level
        {
            get
            {
                if (KodeBarangValue != "000") return "Kode Barang";
                if (KodeSubKelompok != "00") return "Sub Kelompok";
                if (KodeKelompok != "00") return "Kelompok";
                if (KodeBidang != "00") return "Bidang";
                return "Golongan";
            }
        }
    }
}
