using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemKasir.Interfaces;
using SistemKasir.Models;

namespace SistemKasir.Controllers
{
    public class KategoriController : Controller
    {
        private readonly IKategori _kategori;

        public KategoriController(IKategori kategori)
        {
            _kategori = kategori;
        }

        // GET: Kategori
        public async Task<IActionResult> Index()
        {
            return View(await _kategori.GetAllKategori());
        }

        // GET: Kategori/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Kategori/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Kategori kategori)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _kategori.Create(kategori);

                    TempData["success"] = "Kategori berhasil di simpan";
                }
                catch (Exception ex)
                {
                    TempData["error"] = "Kategori gagal di simpan";
                }

                return RedirectToAction(nameof(Index));
            }
            return View(kategori);
        }

        // GET: Kategori/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kategori = await _kategori.GetKategoriById(id);

            if (kategori == null)
            {
                return NotFound();
            }

            return View(kategori);
        }

        // POST: Kategori/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Kategori kategori)
        {
            if (id != kategori.IdKategori)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _kategori.Edit(kategori);

                    TempData["success"] = "Kategori berhasil di update";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_kategori.IsKategoriExists(kategori.IdKategori))
                    {
                        return NotFound();
                    }
                    else
                    {
                        TempData["error"] = "Kategori gagal di update";
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(kategori);
        }

        // POST: Kategori/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            try
            {
                await _kategori.Delete(id);

                TempData["success"] = "Kategori berhasil dihapus";
            }
            catch (Exception ex)
            {
                TempData["error"] = "Kategori gagal dihapus";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
