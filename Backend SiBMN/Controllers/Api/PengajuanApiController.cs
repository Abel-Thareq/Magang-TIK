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
                .Include(p => p.DetailPengajuans);

            if (roleId == 1 && unitId.HasValue)
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
                    p.Status
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
                    d.IdRuang
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
                    pejabatName = pengajuan.Pejabat?.Nama
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
