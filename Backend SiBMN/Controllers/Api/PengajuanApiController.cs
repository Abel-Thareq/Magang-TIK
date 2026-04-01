using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SiBMN.Data;
using SiBMN.Models;

namespace SiBMN.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class PengajuanApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PengajuanApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int? unitId, [FromQuery] int? roleId)
        {
            IQueryable<Pengajuan> query = _context.Pengajuans
                .Include(p => p.Unit)
                .Include(p => p.DetailPengajuans)
                .Include(p => p.Reviewer)
                .Include(p => p.Approver);

            if ((roleId == 1 || roleId == 6) && unitId.HasValue)
            {
                query = query.Where(p => p.UnitId == unitId.Value);
            }

            var pengajuans = await query.OrderByDescending(p => p.TanggalPengajuan)
                .Select(p => new
                {
                    p.IdPengajuan,
                    p.TanggalPengajuan,
                    unitName = p.Unit != null ? p.Unit.NamaUnit : "",
                    p.JenisPengajuan,
                    detailCount = p.DetailPengajuans.Count,
                    p.TotalHarga,
                    p.Status,
                    reviewedByName = p.Reviewer != null ? p.Reviewer.Nama : null,
                    reviewedById = p.ReviewedBy,
                    approvedByName = p.Approver != null ? p.Approver.Nama : null
                })
                .ToListAsync();

            return Ok(pengajuans);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, [FromQuery] int? roleId)
        {
            var pengajuan = await _context.Pengajuans
                .Include(p => p.Unit)
                .Include(p => p.Pejabat)
                .Include(p => p.Reviewer)
                .Include(p => p.Approver)
                .Include(p => p.PimpinanUnitApprover)
                .Include(p => p.WrBpkuApprover)
                .Include(p => p.KabiroBpkuApprover)
                .Include(p => p.KabagUmumApprover)
                .FirstOrDefaultAsync(p => p.IdPengajuan == id);

            if (pengajuan == null) return NotFound();

            bool canSeeBmnReviewInfo = roleId == 4 || roleId == 5;

            var details = await _context.DetailPengajuans
                .Include(d => d.KodeBarang)
                .Include(d => d.RuangGedung)
                .Where(d => d.IdPengajuan == id)
                .OrderBy(d => d.NoPrioritas)
                .Select(d => new
                {
                    d.IdDetPengajuan,
                    d.NoPrioritas,
                    barangNama = d.KodeBarang != null ? d.KodeBarang.UraianBarang : "",
                    barangKode = d.KodeBarang != null
                        ? d.KodeBarang.KodeGolongan + "." + d.KodeBarang.KodeBidang + "." + d.KodeBarang.KodeKelompok + "." + d.KodeBarang.KodeSubKelompok + "." + d.KodeBarang.KodeBarangValue
                        : "",
                    d.Spesifikasi,
                    d.JumlahDiminta,
                    d.HargaSatuan,
                    d.JumlahHarga,
                    gedungNama = d.RuangGedung != null ? d.RuangGedung.NamaGedung : "",
                    ruangNama = d.RuangGedung != null ? d.RuangGedung.NamaRuang : "",
                    d.AsalBarang,
                    d.AlasanImport,
                    d.FungsiBarang,
                    d.LinkSurvey,
                    d.LinkGambar,
                    d.IdBarang,
                    d.IdRuang,
                    d.IsExcluded
                })
                .ToListAsync();

            return Ok(new
            {
                pengajuan = new
                {
                    pengajuan.IdPengajuan,
                    pengajuan.NomorSurat,
                    pengajuan.NoSuratRektor,
                    pengajuan.TglSuratRektor,
                    pengajuan.TanggalPengajuan,
                    pengajuan.TahunAnggaran,
                    pengajuan.Jabatan,
                    pengajuan.JenisPengajuan,
                    pengajuan.Status,
                    pengajuan.TotalHarga,
                    pengajuan.UnitId,
                    pengajuan.IdPejabat,
                    unitName = pengajuan.Unit?.NamaUnit,
                    pejabatName = pengajuan.Pejabat?.Nama,
                    submittedAt = pengajuan.SubmittedAt,
                    pimpinanUnitApprovedByName = pengajuan.PimpinanUnitApprover?.Nama,
                    pimpinanUnitApprovedAt = pengajuan.PimpinanUnitApprovedAt,
                    wrBpkuApprovedByName = pengajuan.WrBpkuApprover?.Nama,
                    wrBpkuApprovedAt = pengajuan.WrBpkuApprovedAt,
                    kabiroBpkuApprovedByName = pengajuan.KabiroBpkuApprover?.Nama,
                    kabiroBpkuApprovedAt = pengajuan.KabiroBpkuApprovedAt,
                    reviewedByName = canSeeBmnReviewInfo ? pengajuan.Reviewer?.Nama : null,
                    reviewedById = pengajuan.ReviewedBy,
                    reviewedAt = canSeeBmnReviewInfo ? pengajuan.ReviewedAt : null,
                    approvedByName = canSeeBmnReviewInfo ? pengajuan.Approver?.Nama : null,
                    approvedAt = canSeeBmnReviewInfo ? pengajuan.ApprovedAt : null,
                    kabagUmumApprovedByName = pengajuan.KabagUmumApprover?.Nama,
                    kabagUmumApprovedAt = pengajuan.KabagUmumApprovedAt
                },
                details
            });
        }

        public class PengajuanRequest
        {
            public string? NoSuratRektor { get; set; }
            public DateTime TanggalPengajuan { get; set; }
            public int? TahunAnggaran { get; set; }
            public string? Jabatan { get; set; }
            public int? IdPejabat { get; set; }
            public string? JenisPengajuan { get; set; }
            public int UnitId { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PengajuanRequest model)
        {
            var pengajuan = new Pengajuan
            {
                NoSuratRektor = model.NoSuratRektor,
                TanggalPengajuan = model.TanggalPengajuan,
                TahunAnggaran = model.TahunAnggaran,
                Jabatan = model.Jabatan,
                IdPejabat = model.IdPejabat,
                JenisPengajuan = model.JenisPengajuan ?? "Belanja Modal",
                UnitId = model.UnitId,
                Status = "draft",
                TotalHarga = 0
            };

            _context.Pengajuans.Add(pengajuan);
            await _context.SaveChangesAsync();

            return Ok(new { id = pengajuan.IdPengajuan, message = "Pengajuan berhasil dibuat" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PengajuanRequest model)
        {
            var pengajuan = await _context.Pengajuans.FindAsync(id);
            if (pengajuan == null) return NotFound();

            pengajuan.NoSuratRektor = model.NoSuratRektor;
            pengajuan.TanggalPengajuan = model.TanggalPengajuan;
            pengajuan.TahunAnggaran = model.TahunAnggaran;
            pengajuan.Jabatan = model.Jabatan;
            pengajuan.IdPejabat = model.IdPejabat;
            pengajuan.JenisPengajuan = model.JenisPengajuan;

            await _context.SaveChangesAsync();
            return Ok(new { message = "Pengajuan berhasil diperbarui" });
        }

        [HttpPost("{id}/submit")]
        public async Task<IActionResult> Submit(int id)
        {
            var pengajuan = await _context.Pengajuans.FindAsync(id);
            if (pengajuan == null) return NotFound();

            pengajuan.Status = "Menunggu Pimpinan Unit";
            pengajuan.SubmittedAt = DateTime.Now;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Pengajuan berhasil diajukan!" });
        }

        public class UpdateStatusRequest
        {
            public string Status { get; set; } = string.Empty;
            public int UserId { get; set; }
            public int RoleId { get; set; }
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateStatusRequest req)
        {
            var pengajuan = await _context.Pengajuans.FindAsync(id);
            if (pengajuan == null) return NotFound();

            switch (req.Status)
            {
                case "ApprovePimpinanUnit":
                    if (req.RoleId == 6 && pengajuan.Status == "Menunggu Pimpinan Unit")
                    {
                        pengajuan.Status = "Menunggu WR BPKU";
                        pengajuan.PimpinanUnitApprovedBy = req.UserId;
                        pengajuan.PimpinanUnitApprovedAt = DateTime.Now;
                    }
                    else return BadRequest(new { message = "Hanya Pimpinan Unit Kerja yang bisa menyetujui pada tahap ini" });
                    break;

                case "RejectPimpinanUnit":
                    if (req.RoleId == 6 && pengajuan.Status == "Menunggu Pimpinan Unit")
                    {
                        pengajuan.Status = "draft";
                        pengajuan.SubmittedAt = null;
                        pengajuan.PimpinanUnitApprovedBy = null;
                        pengajuan.PimpinanUnitApprovedAt = null;
                    }
                    else return BadRequest(new { message = "Hanya Pimpinan Unit Kerja yang bisa menolak pada tahap ini" });
                    break;

                case "ApproveWrBpku":
                    if (req.RoleId == 7 && pengajuan.Status == "Menunggu WR BPKU")
                    {
                        pengajuan.Status = "Menunggu Kabiro BPKU";
                        pengajuan.WrBpkuApprovedBy = req.UserId;
                        pengajuan.WrBpkuApprovedAt = DateTime.Now;
                    }
                    else return BadRequest(new { message = "Hanya WR BPKU yang bisa menyetujui pada tahap ini" });
                    break;

                case "RejectWrBpku":
                    if (req.RoleId == 7 && pengajuan.Status == "Menunggu WR BPKU")
                    {
                        pengajuan.Status = "Menunggu Pimpinan Unit";
                        pengajuan.WrBpkuApprovedBy = null;
                        pengajuan.WrBpkuApprovedAt = null;
                    }
                    else return BadRequest(new { message = "Hanya WR BPKU yang bisa menolak pada tahap ini" });
                    break;

                case "ApproveKabiroBpku":
                    if (req.RoleId == 8 && pengajuan.Status == "Menunggu Kabiro BPKU")
                    {
                        pengajuan.Status = "Menunggu Tim BMN";
                        pengajuan.KabiroBpkuApprovedBy = req.UserId;
                        pengajuan.KabiroBpkuApprovedAt = DateTime.Now;
                    }
                    else return BadRequest(new { message = "Hanya Kabiro BPKU yang bisa menyetujui pada tahap ini" });
                    break;

                case "RejectKabiroBpku":
                    if (req.RoleId == 8 && pengajuan.Status == "Menunggu Kabiro BPKU")
                    {
                        pengajuan.Status = "Menunggu WR BPKU";
                        pengajuan.KabiroBpkuApprovedBy = null;
                        pengajuan.KabiroBpkuApprovedAt = null;
                    }
                    else return BadRequest(new { message = "Hanya Kabiro BPKU yang bisa menolak pada tahap ini" });
                    break;

                case "Review":
                    if (pengajuan.Status == "Menunggu Tim BMN")
                    {
                        pengajuan.Status = "Review";
                        pengajuan.ReviewedBy = req.UserId;
                        pengajuan.ReviewedAt = DateTime.Now;
                        pengajuan.ApprovedBy = null;
                        pengajuan.ApprovedAt = null;
                    }
                    break;

                case "Reviewed":
                    if (pengajuan.Status == "Review" && pengajuan.ReviewedBy == req.UserId)
                    {
                        pengajuan.Status = "Reviewed";
                    }
                    else return BadRequest(new { message = "Hanya reviewer yang bisa menyelesaikan review" });
                    break;

                case "Approve":
                    if (req.RoleId == 5 && pengajuan.Status == "Reviewed")
                    {
                        pengajuan.Status = "Menunggu Kabag Umum";
                        pengajuan.ApprovedBy = req.UserId;
                        pengajuan.ApprovedAt = DateTime.Now;
                    }
                    else return BadRequest(new { message = "Hanya Pimpinan BMN yang bisa menyetujui pengajuan yang sudah direview" });
                    break;

                case "Reject":
                    if (req.RoleId == 5 && pengajuan.Status == "Reviewed")
                    {
                        pengajuan.Status = "Menunggu Tim BMN";
                        pengajuan.ReviewedBy = null;
                        pengajuan.ReviewedAt = null;
                        pengajuan.ApprovedBy = null;
                        pengajuan.ApprovedAt = null;
                    }
                    else return BadRequest(new { message = "Hanya Pimpinan BMN yang bisa menolak pengajuan" });
                    break;

                case "ApproveKabagUmum":
                    if (req.RoleId == 9 && pengajuan.Status == "Menunggu Kabag Umum")
                    {
                        pengajuan.Status = "Selesai";
                        pengajuan.KabagUmumApprovedBy = req.UserId;
                        pengajuan.KabagUmumApprovedAt = DateTime.Now;
                    }
                    else return BadRequest(new { message = "Hanya Kabag Umum yang bisa menyetujui pada tahap ini" });
                    break;

                case "RejectKabagUmum":
                    if (req.RoleId == 9 && pengajuan.Status == "Menunggu Kabag Umum")
                    {
                        pengajuan.Status = "Reviewed";
                        pengajuan.KabagUmumApprovedBy = null;
                        pengajuan.KabagUmumApprovedAt = null;
                    }
                    else return BadRequest(new { message = "Hanya Kabag Umum yang bisa menolak pada tahap ini" });
                    break;

                default:
                    return BadRequest(new { message = "Status tidak valid" });
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "Status berhasil diperbarui", status = pengajuan.Status });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var pengajuan = await _context.Pengajuans
                .Include(p => p.DetailPengajuans)
                .FirstOrDefaultAsync(p => p.IdPengajuan == id);

            if (pengajuan == null) return NotFound();

            _context.Pengajuans.Remove(pengajuan);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Pengajuan berhasil dihapus!" });
        }

        [HttpGet("pejabats")]
        public async Task<IActionResult> GetPejabats()
        {
            var pejabats = await _context.Users
                .Where(u => u.RoleId == 3)
                .Select(u => new { u.IdUser, u.Nama })
                .ToListAsync();

            return Ok(pejabats);
        }
    }
}
