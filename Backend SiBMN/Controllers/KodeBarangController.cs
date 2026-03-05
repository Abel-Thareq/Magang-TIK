using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SiBMN.Data;
using SiBMN.Models;
using SiBMN.Models.ViewModels;

namespace SiBMN.Controllers
{
    public class KodeBarangController : Controller
    {
        private readonly ApplicationDbContext _context;

        public KodeBarangController(ApplicationDbContext context)
        {
            _context = context;
        }

        private bool IsTimKerjaBMN()
        {
            var roleId = HttpContext.Session.GetInt32("RoleId");
            return roleId == 4;
        }

        private IActionResult GuardAccess()
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
                return RedirectToAction("Login", "Account");

            if (!IsTimKerjaBMN())
            {
                TempData["Error"] = "Anda tidak memiliki akses ke halaman ini.";
                return RedirectToAction("Index", "Dashboard");
            }

            return null!;
        }

        public async Task<IActionResult> Index(string? filterGolongan, string? filterBidang, string? filterKelompok, string? filterSubKelompok, string? filterLevel)
        {
            var guard = GuardAccess();
            if (guard != null) return guard;

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
                .ToListAsync();

            // Pass filter values to view
            ViewBag.FilterGolongan = filterGolongan;
            ViewBag.FilterBidang = filterBidang;
            ViewBag.FilterKelompok = filterKelompok;
            ViewBag.FilterSubKelompok = filterSubKelompok;
            ViewBag.FilterLevel = filterLevel;

            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(KodeBarangCreateViewModel model)
        {
            var guard = GuardAccess();
            if (guard != null) return guard;

            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Data yang dimasukkan tidak valid. Silakan periksa kembali.";
                return RedirectToAction("Index");
            }

            var kodeBarang = new KodeBarang
            {
                KodeGolongan = model.KodeGolongan,
                KodeBidang = model.Level == "Golongan" ? "00" : (model.KodeBidang ?? "00"),
                KodeKelompok = (model.Level == "Golongan" || model.Level == "Bidang") ? "00" : (model.KodeKelompok ?? "00"),
                KodeSubKelompok = (model.Level == "Golongan" || model.Level == "Bidang" || model.Level == "Kelompok") ? "00" : (model.KodeSubKelompok ?? "00"),
                KodeBarangValue = model.Level != "KodeBarang" ? "000" : (model.KodeBarangValue ?? "000"),
                UraianBarang = model.UraianBarang
            };

            // Check for duplicate
            var exists = await _context.KodeBarangs.AnyAsync(k =>
                k.KodeGolongan == kodeBarang.KodeGolongan &&
                k.KodeBidang == kodeBarang.KodeBidang &&
                k.KodeKelompok == kodeBarang.KodeKelompok &&
                k.KodeSubKelompok == kodeBarang.KodeSubKelompok &&
                k.KodeBarangValue == kodeBarang.KodeBarangValue);

            if (exists)
            {
                TempData["Error"] = $"Kode barang {kodeBarang.KodeBarangLengkap} sudah ada dalam database.";
                return RedirectToAction("Index");
            }

            _context.KodeBarangs.Add(kodeBarang);
            await _context.SaveChangesAsync();

            TempData["Success"] = $"Kode barang {kodeBarang.KodeBarangLengkap} berhasil ditambahkan.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string uraianBarang)
        {
            var guard = GuardAccess();
            if (guard != null) return guard;

            var kodeBarang = await _context.KodeBarangs.FindAsync(id);
            if (kodeBarang == null)
            {
                TempData["Error"] = "Data kode barang tidak ditemukan.";
                return RedirectToAction("Index");
            }

            if (string.IsNullOrWhiteSpace(uraianBarang))
            {
                TempData["Error"] = "Uraian barang tidak boleh kosong.";
                return RedirectToAction("Index");
            }

            kodeBarang.UraianBarang = uraianBarang;
            await _context.SaveChangesAsync();

            TempData["Success"] = $"Kode barang {kodeBarang.KodeBarangLengkap} berhasil diperbarui.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var guard = GuardAccess();
            if (guard != null) return guard;

            var kodeBarang = await _context.KodeBarangs.FindAsync(id);
            if (kodeBarang == null)
            {
                TempData["Error"] = "Data kode barang tidak ditemukan.";
                return RedirectToAction("Index");
            }

            _context.KodeBarangs.Remove(kodeBarang);
            await _context.SaveChangesAsync();

            TempData["Success"] = $"Kode barang {kodeBarang.KodeBarangLengkap} berhasil dihapus.";
            return RedirectToAction("Index");
        }
    }
}
