using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SiBMN.Data;
using SiBMN.Models;

namespace SiBMN.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class DetailPengajuanApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DetailPengajuanApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        public class DetailRequest
        {
            public int IdPengajuan { get; set; }
            public int IdBarang { get; set; }
            public string? Spesifikasi { get; set; }
            public int JumlahDiminta { get; set; }
            public decimal HargaSatuan { get; set; }
            public int IdRuang { get; set; }
            public string? FungsiBarang { get; set; }
            public string AsalBarang { get; set; } = "PDN";
            public string? AlasanImport { get; set; }
            public string? LinkSurvey { get; set; }
            public string? LinkGambar { get; set; }
        }

        // POST: api/detailpengajuanapi
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DetailRequest model)
        {
            var maxPriority = await _context.DetailPengajuans
                .Where(d => d.IdPengajuan == model.IdPengajuan)
                .MaxAsync(d => (int?)d.NoPrioritas) ?? 0;

            var detail = new DetailPengajuan
            {
                IdPengajuan = model.IdPengajuan,
                IdBarang = model.IdBarang,
                Spesifikasi = model.Spesifikasi,
                NoPrioritas = maxPriority + 1,
                IdRuang = model.IdRuang,
                JumlahDiminta = model.JumlahDiminta,
                HargaSatuan = model.HargaSatuan,
                JumlahHarga = model.JumlahDiminta * model.HargaSatuan,
                FungsiBarang = model.FungsiBarang,
                AsalBarang = model.AsalBarang,
                AlasanImport = model.AsalBarang == "Import" ? model.AlasanImport : null,
                LinkSurvey = model.LinkSurvey,
                LinkGambar = model.LinkGambar,
                JumlahDisetujui = 0
            };

            _context.DetailPengajuans.Add(detail);
            await _context.SaveChangesAsync();

            await UpdateTotalHarga(model.IdPengajuan);

            return Ok(new { message = "Barang berhasil ditambahkan!", id = detail.IdDetPengajuan });
        }

        // PUT: api/detailpengajuanapi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] DetailRequest model)
        {
            var detail = await _context.DetailPengajuans.FindAsync(id);
            if (detail == null) return NotFound();

            detail.IdBarang = model.IdBarang;
            detail.Spesifikasi = model.Spesifikasi;
            detail.JumlahDiminta = model.JumlahDiminta;
            detail.HargaSatuan = model.HargaSatuan;
            detail.JumlahHarga = model.JumlahDiminta * model.HargaSatuan;
            detail.IdRuang = model.IdRuang;
            detail.FungsiBarang = model.FungsiBarang;
            detail.AsalBarang = model.AsalBarang;
            detail.AlasanImport = model.AsalBarang == "Import" ? model.AlasanImport : null;
            detail.LinkSurvey = model.LinkSurvey;
            detail.LinkGambar = model.LinkGambar;

            await _context.SaveChangesAsync();
            await UpdateTotalHarga(detail.IdPengajuan);

            return Ok(new { message = "Data barang berhasil diperbarui!" });
        }

        // GET: api/detailpengajuanapi/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var detail = await _context.DetailPengajuans
                .Include(d => d.KodeBarang)
                .Include(d => d.RuangGedung)
                .Include(d => d.Pengajuan)
                .FirstOrDefaultAsync(d => d.IdDetPengajuan == id);

            if (detail == null) return NotFound();

            return Ok(new
            {
                detail.IdDetPengajuan,
                detail.IdPengajuan,
                detail.IdBarang,
                detail.Spesifikasi,
                detail.JumlahDiminta,
                detail.HargaSatuan,
                detail.JumlahHarga,
                detail.IdRuang,
                detail.FungsiBarang,
                detail.AsalBarang,
                detail.AlasanImport,
                detail.LinkSurvey,
                detail.LinkGambar,
                gedungNama = detail.RuangGedung?.NamaGedung,
                unitId = detail.Pengajuan?.UnitId
            });
        }

        // DELETE: api/detailpengajuanapi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var detail = await _context.DetailPengajuans.FindAsync(id);
            if (detail == null) return NotFound();

            var pengajuanId = detail.IdPengajuan;
            _context.DetailPengajuans.Remove(detail);
            await _context.SaveChangesAsync();

            // Re-order priorities
            var remaining = await _context.DetailPengajuans
                .Where(d => d.IdPengajuan == pengajuanId)
                .OrderBy(d => d.NoPrioritas)
                .ToListAsync();

            for (int i = 0; i < remaining.Count; i++)
            {
                remaining[i].NoPrioritas = i + 1;
            }
            await _context.SaveChangesAsync();
            await UpdateTotalHarga(pengajuanId);

            return Ok(new { message = "Barang berhasil dihapus!" });
        }

        // POST: api/detailpengajuanapi/5/moveup
        [HttpPost("{id}/moveup")]
        public async Task<IActionResult> MoveUp(int id)
        {
            var detail = await _context.DetailPengajuans.FindAsync(id);
            if (detail == null) return NotFound();

            if (detail.NoPrioritas > 1)
            {
                var swapWith = await _context.DetailPengajuans
                    .FirstOrDefaultAsync(d => d.IdPengajuan == detail.IdPengajuan && d.NoPrioritas == detail.NoPrioritas - 1);

                if (swapWith != null)
                {
                    swapWith.NoPrioritas++;
                    detail.NoPrioritas--;
                    await _context.SaveChangesAsync();
                }
            }

            return Ok(new { message = "Prioritas berhasil dinaikkan" });
        }

        // POST: api/detailpengajuanapi/5/movedown
        [HttpPost("{id}/movedown")]
        public async Task<IActionResult> MoveDown(int id)
        {
            var detail = await _context.DetailPengajuans.FindAsync(id);
            if (detail == null) return NotFound();

            var maxPriority = await _context.DetailPengajuans
                .Where(d => d.IdPengajuan == detail.IdPengajuan)
                .MaxAsync(d => d.NoPrioritas);

            if (detail.NoPrioritas < maxPriority)
            {
                var swapWith = await _context.DetailPengajuans
                    .FirstOrDefaultAsync(d => d.IdPengajuan == detail.IdPengajuan && d.NoPrioritas == detail.NoPrioritas + 1);

                if (swapWith != null)
                {
                    swapWith.NoPrioritas--;
                    detail.NoPrioritas++;
                    await _context.SaveChangesAsync();
                }
            }

            return Ok(new { message = "Prioritas berhasil diturunkan" });
        }

        // GET: api/detailpengajuanapi/dropdowns
        [HttpGet("dropdowns")]
        public async Task<IActionResult> GetDropdowns()
        {
            var gedungs = await _context.RuangGedungs
                .Select(r => r.NamaGedung)
                .Distinct()
                .OrderBy(g => g)
                .ToListAsync();

            return Ok(new { gedungs });
        }

        // GET: api/detailpengajuanapi/kodebarangs?golongan=3&bidang=01&kelompok=01&subKelompok=01&search=laptop
        [HttpGet("kodebarangs")]
        public async Task<IActionResult> GetKodeBarangs(
            [FromQuery] string? golongan,
            [FromQuery] string? bidang,
            [FromQuery] string? kelompok,
            [FromQuery] string? subKelompok,
            [FromQuery] string? search)
        {
            var query = _context.KodeBarangs
                .Where(k => k.KodeBarangValue != "000"); // Only leaf level (actual items)

            if (!string.IsNullOrEmpty(golongan))
                query = query.Where(k => k.KodeGolongan == golongan);
            if (!string.IsNullOrEmpty(bidang))
                query = query.Where(k => k.KodeBidang == bidang);
            if (!string.IsNullOrEmpty(kelompok))
                query = query.Where(k => k.KodeKelompok == kelompok);
            if (!string.IsNullOrEmpty(subKelompok))
                query = query.Where(k => k.KodeSubKelompok == subKelompok);
            if (!string.IsNullOrEmpty(search))
                query = query.Where(k => k.UraianBarang.Contains(search));

            var data = await query
                .OrderBy(k => k.KodeGolongan)
                .ThenBy(k => k.KodeBidang)
                .ThenBy(k => k.KodeKelompok)
                .ThenBy(k => k.KodeSubKelompok)
                .ThenBy(k => k.KodeBarangValue)
                .Select(k => new
                {
                    id = k.Id,
                    kode = k.KodeGolongan + "." + k.KodeBidang + "." + k.KodeKelompok + "." + k.KodeSubKelompok + "." + k.KodeBarangValue,
                    uraian = k.UraianBarang,
                    display = k.KodeGolongan + "." + k.KodeBidang + "." + k.KodeKelompok + "." + k.KodeSubKelompok + "." + k.KodeBarangValue + " - " + k.UraianBarang
                })
                .ToListAsync();

            return Ok(data);
        }

        private async Task UpdateTotalHarga(int pengajuanId)
        {
            var total = await _context.DetailPengajuans
                .Where(d => d.IdPengajuan == pengajuanId)
                .SumAsync(d => d.JumlahHarga);

            var pengajuan = await _context.Pengajuans.FindAsync(pengajuanId);
            if (pengajuan != null)
            {
                pengajuan.TotalHarga = total;
                await _context.SaveChangesAsync();
            }
        }
    }
}
