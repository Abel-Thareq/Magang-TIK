using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SiBMN.Data;
using SiBMN.Models;
using SiBMN.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SiBMN.Controllers
{
    public class DetailPengajuanController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DetailPengajuanController(ApplicationDbContext context)
        {
            _context = context;
        }

        private bool IsLoggedIn()
        {
            return HttpContext.Session.GetInt32("UserId") != null;
        }

        // GET: DetailPengajuan/Create?pengajuanId=1
        public async Task<IActionResult> Create(int pengajuanId)
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Account");

            var pengajuan = await _context.Pengajuans.FindAsync(pengajuanId);
            if (pengajuan == null) return NotFound();

            await PopulateDropdowns(pengajuan.UnitId);

            var model = new DetailPengajuanCreateViewModel
            {
                IdPengajuan = pengajuanId
            };

            return View(model);
        }

        // POST: DetailPengajuan/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DetailPengajuanCreateViewModel model)
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Account");

            if (ModelState.IsValid)
            {
                // Get next priority number
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

                // Update total harga in pengajuan
                await UpdateTotalHarga(model.IdPengajuan);

                TempData["Success"] = "Barang berhasil ditambahkan!";
                return RedirectToAction("Details", "Pengajuan", new { id = model.IdPengajuan });
            }

            var pengajuan = await _context.Pengajuans.FindAsync(model.IdPengajuan);
            await PopulateDropdowns(pengajuan?.UnitId ?? 0);
            return View(model);
        }

        // GET: DetailPengajuan/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Account");

            var detail = await _context.DetailPengajuans
                .Include(d => d.Pengajuan)
                .FirstOrDefaultAsync(d => d.IdDetPengajuan == id);

            if (detail == null) return NotFound();

            await PopulateDropdowns(detail.Pengajuan?.UnitId ?? 0);

            var model = new DetailPengajuanCreateViewModel
            {
                IdPengajuan = detail.IdPengajuan,
                IdBarang = detail.IdBarang,
                Spesifikasi = detail.Spesifikasi,
                JumlahDiminta = detail.JumlahDiminta,
                HargaSatuan = detail.HargaSatuan,
                IdRuang = detail.IdRuang,
                FungsiBarang = detail.FungsiBarang,
                AsalBarang = detail.AsalBarang,
                AlasanImport = detail.AlasanImport,
                LinkSurvey = detail.LinkSurvey,
                LinkGambar = detail.LinkGambar
            };

            ViewBag.DetailId = id;
            return View(model);
        }

        // POST: DetailPengajuan/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DetailPengajuanCreateViewModel model)
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Account");

            if (ModelState.IsValid)
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

                TempData["Success"] = "Data barang berhasil diperbarui!";
                return RedirectToAction("Details", "Pengajuan", new { id = detail.IdPengajuan });
            }

            var pengajuan = await _context.Pengajuans.FindAsync(model.IdPengajuan);
            await PopulateDropdowns(pengajuan?.UnitId ?? 0);
            ViewBag.DetailId = id;
            return View(model);
        }

        // POST: DetailPengajuan/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Account");

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

            TempData["Success"] = "Barang berhasil dihapus!";
            return RedirectToAction("Details", "Pengajuan", new { id = pengajuanId });
        }

        // POST: DetailPengajuan/MoveUp/5
        [HttpPost]
        public async Task<IActionResult> MoveUp(int id)
        {
            if (!IsLoggedIn()) return Json(new { success = false });

            var detail = await _context.DetailPengajuans.FindAsync(id);
            if (detail == null) return Json(new { success = false });

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

            return RedirectToAction("Details", "Pengajuan", new { id = detail.IdPengajuan });
        }

        // POST: DetailPengajuan/MoveDown/5
        [HttpPost]
        public async Task<IActionResult> MoveDown(int id)
        {
            if (!IsLoggedIn()) return Json(new { success = false });

            var detail = await _context.DetailPengajuans.FindAsync(id);
            if (detail == null) return Json(new { success = false });

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

            return RedirectToAction("Details", "Pengajuan", new { id = detail.IdPengajuan });
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

        private async Task PopulateDropdowns(int unitId)
        {
            // Use KodeBarang (leaf-level items only) instead of MasterBarang
            var kodeBarangs = await _context.KodeBarangs
                .Where(k => k.KodeBarangValue != "000") // Only leaf-level items
                .OrderBy(k => k.KodeGolongan)
                .ThenBy(k => k.KodeBidang)
                .ThenBy(k => k.KodeKelompok)
                .ThenBy(k => k.KodeSubKelompok)
                .ThenBy(k => k.KodeBarangValue)
                .Select(k => new { 
                    k.Id, 
                    Display = k.KodeGolongan + "." + k.KodeBidang + "." + k.KodeKelompok + "." + k.KodeSubKelompok + "." + k.KodeBarangValue + " - " + k.UraianBarang 
                })
                .ToListAsync();

            ViewBag.Barangs = new SelectList(kodeBarangs, "Id", "Display");

            // Get gedung list (distinct)
            var gedungs = await _context.RuangGedungs
                .Select(r => r.NamaGedung)
                .Distinct()
                .OrderBy(g => g)
                .ToListAsync();
            ViewBag.Gedungs = gedungs;

            var ruangs = await _context.RuangGedungs
                .OrderBy(r => r.NamaGedung)
                .ThenBy(r => r.NamaRuang)
                .Select(r => new { r.IdRuang, Display = r.NamaGedung + " - " + r.NamaRuang, r.NamaGedung })
                .ToListAsync();

            ViewBag.Ruangs = new SelectList(ruangs, "IdRuang", "Display");
            ViewBag.RuangData = System.Text.Json.JsonSerializer.Serialize(ruangs);
        }
    }
}
