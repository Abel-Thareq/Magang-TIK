using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SiBMN.Data;

namespace SiBMN.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DashboardApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetStats([FromQuery] int? unitId, [FromQuery] int? roleId)
        {
            var query = _context.Pengajuans.AsQueryable();

            if (roleId == 1 && unitId.HasValue)
            {
                query = query.Where(p => p.UnitId == unitId.Value);
            }

            var totalPengajuan = await query.CountAsync();
            var draftCount = await query.CountAsync(p => p.Status == "draft");
            var approvedCount = await query.CountAsync(p => p.Status == "approved");

            var pengajuanIds = await query.Select(p => p.IdPengajuan).ToListAsync();
            var totalBarang = await _context.DetailPengajuans
                .CountAsync(d => pengajuanIds.Contains(d.IdPengajuan));

            var recentPengajuan = await query
                .Include(p => p.Unit)
                .OrderByDescending(p => p.TanggalPengajuan)
                .Take(5)
                .Select(p => new
                {
                    p.IdPengajuan,
                    p.TanggalPengajuan,
                    unitName = p.Unit != null ? p.Unit.NamaUnit : "-",
                    p.JenisPengajuan,
                    p.Status
                })
                .ToListAsync();

            var golongans = await _context.KodeBarangs
                .Where(k => k.KodeBidang == "00" && k.KodeKelompok == "00" && k.KodeSubKelompok == "00" && k.KodeBarangValue == "000")
                .Select(g => new { g.KodeGolongan, g.UraianBarang })
                .ToListAsync();

            var totalAset = await _context.KodeBarangs
                .Where(k => k.KodeBarangValue != "000")
                .CountAsync();

            var asetPerGolongan = new List<object>();
            foreach (var g in golongans)
            {
                var count = await _context.KodeBarangs
                    .CountAsync(k => k.KodeGolongan == g.KodeGolongan && k.KodeBarangValue != "000");
                if (count > 0)
                {
                    asetPerGolongan.Add(new { kode = g.KodeGolongan, uraian = g.UraianBarang, count });
                }
            }

            return Ok(new
            {
                totalPengajuan,
                draftCount,
                approvedCount,
                totalBarang,
                totalAset,
                asetPerGolongan,
                recentPengajuan
            });
        }
    }
}
