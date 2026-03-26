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

        // GET: api/pengajuanapi?unitId=1&roleId=1
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int? unitId, [FromQuery] int? roleId)
        {
            IQueryable<Pengajuan> query = _context.Pengajuans
                .Include(p => p.Unit)
                .Include(p => p.DetailPengajuans)
                .Include(p => p.Reviewer)
                .Include(p => p.Approver);

            // Role 1 (Admin Unit) and Role 6 (Pimpinan Unit) see their own unit only
            if ((roleId == 1 || roleId == 6) && unitId.HasValue)
            {
                query = query.Where(p => p.UnitId == unitId.Value);
            }
            // Roles 4,5,7,8,9 see ALL pengajuan

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

        // GET: api/pengajuanapi/5?roleId=4
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

            // Only Tim BMN (4) and Pimpinan BMN (5) can see BMN reviewer/approver names
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
                    // Per-stage tracking info
                    submittedAt = pengajuan.SubmittedAt,
                    pimpinanUnitApprovedByName = pengajuan.PimpinanUnitApprover?.Nama,
                    pimpinanUnitApprovedAt = pengajuan.PimpinanUnitApprovedAt,
                    wrBpkuApprovedByName = pengajuan.WrBpkuApprover?.Nama,
                    wrBpkuApprovedAt = pengajuan.WrBpkuApprovedAt,
                    kabiroBpkuApprovedByName = pengajuan.KabiroBpkuApprover?.Nama,
                    kabiroBpkuApprovedAt = pengajuan.KabiroBpkuApprovedAt,
                    // BMN review info — restricted to Tim BMN & Pimpinan BMN
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

        // POST: api/pengajuanapi
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

        // PUT: api/pengajuanapi/5
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

        // POST: api/pengajuanapi/5/submit
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

        // PATCH: api/pengajuanapi/5/status
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateStatusRequest req)
        {
            var pengajuan = await _context.Pengajuans.FindAsync(id);
            if (pengajuan == null) return NotFound();

            switch (req.Status)
            {
                // === Pimpinan Unit Kerja (Role 6) ===
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

                // === WR BPKU (Role 7) ===
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

                // === Kabiro BPKU (Role 8) ===
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

                // === Tim BMN (Role 4) — Review ===
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

                // === Pimpinan BMN (Role 5) — Approve/Reject ===
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

                // === Kabag Umum (Role 9) ===
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

        // DELETE: api/pengajuanapi/5
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

        // GET: api/pengajuanapi/pejabats
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
