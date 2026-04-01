using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SiBMN.Data;
using SiBMN.Models;

namespace SiBMN.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class KodeBarangApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public KodeBarangApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? filterGolongan,
            [FromQuery] string? filterBidang,
            [FromQuery] string? filterKelompok,
            [FromQuery] string? filterSubKelompok,
            [FromQuery] string? filterLevel)
        {
            var query = _context.KodeBarangs.AsQueryable();

            if (!string.IsNullOrEmpty(filterGolongan))
                query = query.Where(k => k.KodeGolongan == filterGolongan);
            if (!string.IsNullOrEmpty(filterBidang))
                query = query.Where(k => k.KodeBidang == filterBidang);
            if (!string.IsNullOrEmpty(filterKelompok))
                query = query.Where(k => k.KodeKelompok == filterKelompok);
            if (!string.IsNullOrEmpty(filterSubKelompok))
                query = query.Where(k => k.KodeSubKelompok == filterSubKelompok);
            if (!string.IsNullOrEmpty(filterLevel))
            {
                query = filterLevel switch
                {
                    "Golongan" => query.Where(k => k.KodeBidang == "00" && k.KodeKelompok == "00" && k.KodeSubKelompok == "00" && k.KodeBarangValue == "000"),
                    "Bidang" => query.Where(k => k.KodeBidang != "00" && k.KodeKelompok == "00" && k.KodeSubKelompok == "00" && k.KodeBarangValue == "000"),
                    "Kelompok" => query.Where(k => k.KodeKelompok != "00" && k.KodeSubKelompok == "00" && k.KodeBarangValue == "000"),
                    "SubKelompok" => query.Where(k => k.KodeSubKelompok != "00" && k.KodeBarangValue == "000"),
                    "KodeBarang" => query.Where(k => k.KodeBarangValue != "000"),
                    _ => query
                };
            }

            var data = await query
                .OrderBy(k => k.KodeGolongan)
                .ThenBy(k => k.KodeBidang)
                .ThenBy(k => k.KodeKelompok)
                .ThenBy(k => k.KodeSubKelompok)
                .ThenBy(k => k.KodeBarangValue)
                .Select(k => new
                {
                    k.Id,
                    k.KodeGolongan,
                    k.KodeBidang,
                    k.KodeKelompok,
                    k.KodeSubKelompok,
                    k.KodeBarangValue,
                    k.UraianBarang,
                    kodeBarangLengkap = k.KodeGolongan + "." + k.KodeBidang + "." + k.KodeKelompok + "." + k.KodeSubKelompok + "." + k.KodeBarangValue,
                    level = k.KodeBarangValue != "000" ? "Kode Barang"
                        : k.KodeSubKelompok != "00" ? "Sub Kelompok"
                        : k.KodeKelompok != "00" ? "Kelompok"
                        : k.KodeBidang != "00" ? "Bidang"
                        : "Golongan"
                })
                .ToListAsync();

            return Ok(data);
        }

        public class KodeBarangRequest
        {
            public string Level { get; set; } = "Golongan";
            public string KodeGolongan { get; set; } = string.Empty;
            public string? KodeBidang { get; set; }
            public string? KodeKelompok { get; set; }
            public string? KodeSubKelompok { get; set; }
            public string? KodeBarangValue { get; set; }
            public string UraianBarang { get; set; } = string.Empty;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] KodeBarangRequest model)
        {
            var kodeBarang = new KodeBarang
            {
                KodeGolongan = model.KodeGolongan,
                KodeBidang = model.Level == "Golongan" ? "00" : (model.KodeBidang ?? "00"),
                KodeKelompok = (model.Level == "Golongan" || model.Level == "Bidang") ? "00" : (model.KodeKelompok ?? "00"),
                KodeSubKelompok = (model.Level == "Golongan" || model.Level == "Bidang" || model.Level == "Kelompok") ? "00" : (model.KodeSubKelompok ?? "00"),
                KodeBarangValue = model.Level != "KodeBarang" ? "000" : (model.KodeBarangValue ?? "000"),
                UraianBarang = model.UraianBarang
            };

            var exists = await _context.KodeBarangs.AnyAsync(k =>
                k.KodeGolongan == kodeBarang.KodeGolongan &&
                k.KodeBidang == kodeBarang.KodeBidang &&
                k.KodeKelompok == kodeBarang.KodeKelompok &&
                k.KodeSubKelompok == kodeBarang.KodeSubKelompok &&
                k.KodeBarangValue == kodeBarang.KodeBarangValue);

            if (exists)
                return BadRequest(new { message = $"Kode barang {kodeBarang.KodeBarangLengkap} sudah ada dalam database." });

            _context.KodeBarangs.Add(kodeBarang);
            await _context.SaveChangesAsync();

            return Ok(new { message = $"Kode barang {kodeBarang.KodeBarangLengkap} berhasil ditambahkan.", id = kodeBarang.Id });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] dynamic body)
        {
            var kodeBarang = await _context.KodeBarangs.FindAsync(id);
            if (kodeBarang == null)
                return NotFound(new { message = "Data kode barang tidak ditemukan." });

            string uraianBarang = body.GetProperty("uraianBarang").GetString();
            if (string.IsNullOrWhiteSpace(uraianBarang))
                return BadRequest(new { message = "Uraian barang tidak boleh kosong." });

            kodeBarang.UraianBarang = uraianBarang;
            await _context.SaveChangesAsync();

            return Ok(new { message = $"Kode barang {kodeBarang.KodeBarangLengkap} berhasil diperbarui." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var kodeBarang = await _context.KodeBarangs.FindAsync(id);
            if (kodeBarang == null)
                return NotFound(new { message = "Data kode barang tidak ditemukan." });

            _context.KodeBarangs.Remove(kodeBarang);
            await _context.SaveChangesAsync();

            return Ok(new { message = $"Kode barang {kodeBarang.KodeBarangLengkap} berhasil dihapus." });
        }

        [HttpGet("Golongan")]
        public async Task<IActionResult> GetGolongan()
        {
            var data = await _context.KodeBarangs
                .Where(k => k.KodeBidang == "00" && k.KodeKelompok == "00" && k.KodeSubKelompok == "00" && k.KodeBarangValue == "000")
                .Select(k => new { kode = k.KodeGolongan, uraian = k.UraianBarang })
                .Distinct()
                .OrderBy(k => k.kode)
                .ToListAsync();

            return Ok(data);
        }

        [HttpGet("Bidang")]
        public async Task<IActionResult> GetBidang(string kodeGolongan)
        {
            var data = await _context.KodeBarangs
                .Where(k => k.KodeGolongan == kodeGolongan && k.KodeBidang != "00" && k.KodeKelompok == "00" && k.KodeSubKelompok == "00" && k.KodeBarangValue == "000")
                .Select(k => new { kode = k.KodeBidang, uraian = k.UraianBarang })
                .OrderBy(k => k.kode)
                .ToListAsync();

            return Ok(data);
        }

        [HttpGet("Kelompok")]
        public async Task<IActionResult> GetKelompok(string kodeGolongan, string kodeBidang)
        {
            var data = await _context.KodeBarangs
                .Where(k => k.KodeGolongan == kodeGolongan && k.KodeBidang == kodeBidang && k.KodeKelompok != "00" && k.KodeSubKelompok == "00" && k.KodeBarangValue == "000")
                .Select(k => new { kode = k.KodeKelompok, uraian = k.UraianBarang })
                .OrderBy(k => k.kode)
                .ToListAsync();

            return Ok(data);
        }

        [HttpGet("SubKelompok")]
        public async Task<IActionResult> GetSubKelompok(string kodeGolongan, string kodeBidang, string kodeKelompok)
        {
            var data = await _context.KodeBarangs
                .Where(k => k.KodeGolongan == kodeGolongan && k.KodeBidang == kodeBidang && k.KodeKelompok == kodeKelompok && k.KodeSubKelompok != "00" && k.KodeBarangValue == "000")
                .Select(k => new { kode = k.KodeSubKelompok, uraian = k.UraianBarang })
                .OrderBy(k => k.kode)
                .ToListAsync();

            return Ok(data);
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _context.KodeBarangs.FindAsync(id);
            if (item == null) return NotFound();

            return Ok(new
            {
                id = item.Id,
                kodeGolongan = item.KodeGolongan,
                kodeBidang = item.KodeBidang,
                kodeKelompok = item.KodeKelompok,
                kodeSubKelompok = item.KodeSubKelompok,
                kodeBarangValue = item.KodeBarangValue,
                uraianBarang = item.UraianBarang,
                kodeBarangLengkap = item.KodeBarangLengkap,
                level = item.Level
            });
        }
    }
}
