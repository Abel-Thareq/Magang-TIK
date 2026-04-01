using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SiBMN.Data;
using SiBMN.Models;

namespace SiBMN.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class JadwalApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public JadwalApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetEvents([FromQuery] int userId, [FromQuery] string? bulan)
        {
            var query = _context.JadwalEvents
                .Where(j => j.UserId == userId && j.DeletedAt == null);

            if (!string.IsNullOrEmpty(bulan))
            {
                query = query.Where(j => j.Bulan == bulan);
            }

            var events = await query
                .OrderBy(j => j.Waktu)
                .Select(j => new
                {
                    j.Id,
                    j.Bulan,
                    j.Waktu,
                    j.Keterangan
                })
                .ToListAsync();

            return Ok(events);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEvent([FromBody] JadwalEventDto dto)
        {
            var jadwal = new JadwalEvent
            {
                UserId = dto.UserId,
                Bulan = dto.Bulan,
                Waktu = dto.Waktu,
                Keterangan = dto.Keterangan
            };

            _context.JadwalEvents.Add(jadwal);
            await _context.SaveChangesAsync();

            return Ok(new { jadwal.Id, jadwal.Bulan, jadwal.Waktu, jadwal.Keterangan });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var jadwal = await _context.JadwalEvents.FindAsync(id);
            if (jadwal == null) return NotFound();

            jadwal.DeletedAt = DateTime.Now;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Jadwal dihapus" });
        }
    }

    public class JadwalEventDto
    {
        public int UserId { get; set; }
        public string Bulan { get; set; } = string.Empty;
        public string Waktu { get; set; } = string.Empty;
        public string Keterangan { get; set; } = string.Empty;
    }
}
