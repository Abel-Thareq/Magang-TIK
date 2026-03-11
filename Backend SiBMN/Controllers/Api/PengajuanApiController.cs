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

            if (roleId == 1 && unitId.HasValue)
            {
                query = query.Where(p => p.UnitId == unitId.Value);
            }
            // Roles 4 (Tim BMN) and 5 (Pimpinan BMN) see ALL pengajuan

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

        // GET: api/pengajuanapi/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var pengajuan = await _context.Pengajuans
                .Include(p => p.Unit)
                .Include(p => p.Pejabat)
                .Include(p => p.Reviewer)
                .Include(p => p.Approver)
                .FirstOrDefaultAsync(p => p.IdPengajuan == id);

            if (pengajuan == null) return NotFound();

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
                    reviewedByName = pengajuan.Reviewer?.Nama,
                    reviewedById = pengajuan.ReviewedBy,
                    approvedByName = pengajuan.Approver?.Nama
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

            pengajuan.Status = "approved";
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
                case "Review":
                    // Tim BMN clicks view → lock as reviewer
                    if (pengajuan.Status == "draft" || pengajuan.Status == "approved")
                    {
                        pengajuan.Status = "Review";
                        pengajuan.ReviewedBy = req.UserId;
                        pengajuan.ApprovedBy = null;
                    }
                    break;

                case "Reviewed":
                    // Reviewer marks review complete
                    if (pengajuan.Status == "Review" && pengajuan.ReviewedBy == req.UserId)
                    {
                        pengajuan.Status = "Reviewed";
                    }
                    else
                    {
                        return BadRequest(new { message = "Hanya reviewer yang bisa menyelesaikan review" });
                    }
                    break;

                case "Approve":
                    // Pimpinan BMN approves
                    if (req.RoleId == 5 && pengajuan.Status == "Reviewed")
                    {
                        pengajuan.Status = "Approve";
                        pengajuan.ApprovedBy = req.UserId;
                    }
                    else
                    {
                        return BadRequest(new { message = "Hanya Pimpinan BMN yang bisa menyetujui pengajuan yang sudah direview" });
                    }
                    break;

                case "Reject":
                    // Pimpinan BMN rejects → back to Draft
                    if (req.RoleId == 5 && pengajuan.Status == "Reviewed")
                    {
                        pengajuan.Status = "draft";
                        pengajuan.ReviewedBy = null;
                        pengajuan.ApprovedBy = null;
                    }
                    else
                    {
                        return BadRequest(new { message = "Hanya Pimpinan BMN yang bisa menolak pengajuan" });
                    }
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
