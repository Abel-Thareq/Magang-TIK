using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SiBMN.Data;

namespace SiBMN.Controllers
{
    public class BarangModalController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BarangModalController(ApplicationDbContext context)
        {
            _context = context;
        }

        private bool IsAdminUnitKerja()
        {
            var roleId = HttpContext.Session.GetInt32("RoleId");
            return roleId == 1;
        }

        public async Task<IActionResult> Index(string? filterGolongan, string? filterBidang, string? filterKelompok, string? filterSubKelompok)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
                return RedirectToAction("Login", "Account");

            if (!IsAdminUnitKerja())
            {
                TempData["Error"] = "Anda tidak memiliki akses ke halaman ini.";
                return RedirectToAction("Index", "Dashboard");
            }

            // Only show items at "Kode Barang" level (KodeBarangValue != "000")
            var query = _context.KodeBarangs
                .Where(k => k.KodeBarangValue != "000")
                .AsQueryable();

            if (!string.IsNullOrEmpty(filterGolongan))
                query = query.Where(k => k.KodeGolongan == filterGolongan);
            if (!string.IsNullOrEmpty(filterBidang))
                query = query.Where(k => k.KodeBidang == filterBidang);
            if (!string.IsNullOrEmpty(filterKelompok))
                query = query.Where(k => k.KodeKelompok == filterKelompok);
            if (!string.IsNullOrEmpty(filterSubKelompok))
                query = query.Where(k => k.KodeSubKelompok == filterSubKelompok);

            var data = await query
                .OrderBy(k => k.KodeGolongan)
                .ThenBy(k => k.KodeBidang)
                .ThenBy(k => k.KodeKelompok)
                .ThenBy(k => k.KodeSubKelompok)
                .ThenBy(k => k.KodeBarangValue)
                .ToListAsync();

            // Pass filter values to view
            ViewBag.FilterGolongan = filterGolongan;
            ViewBag.FilterBidang = filterBidang;
            ViewBag.FilterKelompok = filterKelompok;
            ViewBag.FilterSubKelompok = filterSubKelompok;

            return View(data);
        }
    }
}
