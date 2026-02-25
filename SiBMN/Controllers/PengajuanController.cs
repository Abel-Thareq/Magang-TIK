using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SiBMN.Data;
using SiBMN.Models;
using SiBMN.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SiBMN.Controllers
{
    public class PengajuanController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PengajuanController(ApplicationDbContext context)
        {
            _context = context;
        }

        private bool IsLoggedIn()
        {
            return HttpContext.Session.GetInt32("UserId") != null;
        }

        // GET: Pengajuan
        public async Task<IActionResult> Index()
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Account");

            var unitId = HttpContext.Session.GetInt32("UnitId") ?? 0;
            var roleId = HttpContext.Session.GetInt32("RoleId") ?? 0;

            IQueryable<Pengajuan> query = _context.Pengajuans
                .Include(p => p.Unit)
                .Include(p => p.DetailPengajuans);

            // Admin Unit Kerja only sees their own unit's submissions
            if (roleId == 1)
            {
                query = query.Where(p => p.UnitId == unitId);
            }

            var pengajuans = await query.OrderByDescending(p => p.TanggalPengajuan).ToListAsync();
            return View(pengajuans);
        }

        // GET: Pengajuan/Create
        public async Task<IActionResult> Create()
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Account");

            await PopulateDropdowns();
            var model = new PengajuanCreateViewModel
            {
                TanggalPengajuan = DateTime.Now,
                TahunAnggaran = DateTime.Now.Year + 1
            };
            return View(model);
        }

        // POST: Pengajuan/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PengajuanCreateViewModel model)
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Account");

            if (ModelState.IsValid)
            {
                var unitId = HttpContext.Session.GetInt32("UnitId") ?? 0;

                var pengajuan = new Pengajuan
                {
                    NoSuratRektor = model.NoSuratRektor,
                    TanggalPengajuan = model.TanggalPengajuan,
                    TahunAnggaran = model.TahunAnggaran,
                    Jabatan = model.Jabatan,
                    IdPejabat = model.IdPejabat,
                    JenisPengajuan = model.JenisPengajuan ?? "Belanja Modal",
                    UnitId = unitId,
                    Status = "draft",
                    TotalHarga = 0
                };

                _context.Pengajuans.Add(pengajuan);
                await _context.SaveChangesAsync();

                return RedirectToAction("Details", new { id = pengajuan.IdPengajuan });
            }

            await PopulateDropdowns();
            return View(model);
        }

        // GET: Pengajuan/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Account");

            var pengajuan = await _context.Pengajuans.FindAsync(id);
            if (pengajuan == null) return NotFound();

            await PopulateDropdowns();

            var model = new PengajuanCreateViewModel
            {
                NoSuratRektor = pengajuan.NoSuratRektor,
                TanggalPengajuan = pengajuan.TanggalPengajuan,
                TahunAnggaran = pengajuan.TahunAnggaran,
                Jabatan = pengajuan.Jabatan,
                IdPejabat = pengajuan.IdPejabat,
                JenisPengajuan = pengajuan.JenisPengajuan
            };

            ViewBag.PengajuanId = id;
            return View(model);
        }

        // POST: Pengajuan/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PengajuanCreateViewModel model)
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Account");

            if (ModelState.IsValid)
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
                return RedirectToAction("Details", new { id });
            }

            await PopulateDropdowns();
            ViewBag.PengajuanId = id;
            return View(model);
        }

        // GET: Pengajuan/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Account");

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
                .ToListAsync();

            var viewModel = new PengajuanDetailViewModel
            {
                Pengajuan = pengajuan,
                Details = details
            };

            return View(viewModel);
        }

        // POST: Pengajuan/Submit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit(int id)
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Account");

            var pengajuan = await _context.Pengajuans.FindAsync(id);
            if (pengajuan == null) return NotFound();

            pengajuan.Status = "approved";
            await _context.SaveChangesAsync();

            TempData["Success"] = "Pengajuan berhasil diajukan!";
            return RedirectToAction("Details", new { id });
        }

        // POST: Pengajuan/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Account");

            var pengajuan = await _context.Pengajuans
                .Include(p => p.DetailPengajuans)
                .FirstOrDefaultAsync(p => p.IdPengajuan == id);

            if (pengajuan == null) return NotFound();

            _context.Pengajuans.Remove(pengajuan);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Pengajuan berhasil dihapus!";
            return RedirectToAction("Index");
        }

        private async Task PopulateDropdowns()
        {
            var pejabats = await _context.Users
                .Where(u => u.RoleId == 3)
                .Select(u => new { u.IdUser, Display = u.Nama })
                .ToListAsync();

            ViewBag.Pejabats = new SelectList(pejabats, "IdUser", "Display");
        }
    }
}
