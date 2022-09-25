using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemKasir.Data;
using SistemKasir.Interfaces;
using SistemKasir.Models;
using SistemKasir.Services;
using SistemKasir.ViewModels;

namespace SistemKasir.Controllers
{
    public class ProdukController : Controller
    {
        private readonly IProduk _produk;
        private readonly IKategori _kategori;

        public ProdukController(IProduk produk, IKategori kategori)
        {
            _produk = produk;
            _kategori = kategori;
        }

        // GET: Produk
        public async Task<IActionResult> Index()
        {
            return View(await _produk.GetAllProduk());
        }

        // GET: Produk/Create
        public IActionResult Create()
        {
            ViewBag.ListKategori = GetListKategori();

            return View();
        }

        // POST: Produk/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Produk produk)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _produk.Create(produk);

                    TempData["success"] = "Produk berhasil di simpan";
                }
                catch (Exception ex)
                {
                    TempData["error"] = "Produk gagal di simpan";
                }

                return RedirectToAction(nameof(Index));
            }

            ViewBag.ListKategori = GetListKategori();

            return View(produk);
        }

        // GET: Produk/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produk = await _produk.GetProdukById(id);

            if (produk == null)
            {
                return NotFound();
            }

            ViewBag.ListKategori = GetListKategori();

            return View(produk);
        }

        // POST: Produk/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Produk produk)
        {
            if (id != produk.IdProduk)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _produk.Edit(produk);

                    TempData["success"] = "Produk berhasil di update";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_produk.IsProdukExists(produk.IdProduk))
                    {
                        return NotFound();
                    }
                    else
                    {
                        TempData["error"] = "Produk gagal di update";
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.ListKategori = GetListKategori();

            return View(produk);
        }

        // POST: Produk/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            try
            {
                await _produk.Delete(id);

                TempData["success"] = "Produk berhasil dihapus";
            }
            catch (Exception ex)
            {
                TempData["error"] = "Produk gagal dihapus";
            }

            return RedirectToAction(nameof(Index));
        }

        private List<SelectListItem> GetListKategori()
        {
            var items = _kategori.GetAllKategori();

            var listKategori = items.Result.Select(ut => new SelectListItem()
            {
                Value = ut.IdKategori.ToString(),
                Text = ut.Deskripsi
            }).ToList();

            var defItem = new SelectListItem()
            {
                Value = "",
                Text = "-- Pilih Kategori --"
            };

            listKategori.Insert(0, defItem);

            return listKategori;
        }
    }
}
