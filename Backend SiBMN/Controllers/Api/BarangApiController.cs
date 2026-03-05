using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SiBMN.Data;

namespace SiBMN.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class BarangApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BarangApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/BarangApi/Search?term=laptop
        [HttpGet("Search")]
        public async Task<IActionResult> Search(string term)
        {
            if (string.IsNullOrWhiteSpace(term))
                return Ok(new List<object>());

            var results = await _context.MasterBarangs
                .Include(b => b.KategoriBarang)
                .Where(b => b.NamaBarang.Contains(term) || b.KategoriBarang!.NamaKategori.Contains(term))
                .Take(20)
                .Select(b => new
                {
                    id = b.IdBarang,
                    text = b.NamaBarang,
                    kategori = b.KategoriBarang!.NamaKategori,
                    satuan = b.Satuan,
                    spesifikasi = b.Spesifikasi
                })
                .ToListAsync();

            return Ok(results);
        }

        // GET: api/BarangApi/GetRuangs?gedung=Gedung A
        [HttpGet("GetRuangs")]
        public async Task<IActionResult> GetRuangs(string gedung)
        {
            var ruangs = await _context.RuangGedungs
                .Where(r => r.NamaGedung == gedung)
                .Select(r => new { r.IdRuang, r.NamaRuang })
                .ToListAsync();

            return Ok(ruangs);
        }
    }
}
