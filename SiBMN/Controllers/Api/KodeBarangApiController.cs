using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SiBMN.Data;

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

        // GET: api/KodeBarangApi/Golongan
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

        // GET: api/KodeBarangApi/Bidang?kodeGolongan=3
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

        // GET: api/KodeBarangApi/Kelompok?kodeGolongan=3&kodeBidang=01
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

        // GET: api/KodeBarangApi/SubKelompok?kodeGolongan=3&kodeBidang=01&kodeKelompok=01
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

        // GET: api/KodeBarangApi/GetById/5
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
