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
    public class TransaksiController : Controller
    {
        private readonly ITransaksi _transaksi;
        private readonly IProduk _produk;

        public TransaksiController(ITransaksi transaksi, IProduk produk)
        {
            _transaksi = transaksi;
            _produk = produk;
        }

        // GET: Transaksi
        public async Task<IActionResult> Index()
        {
            return View(await _transaksi.GetAllTransaksi());
        }

        // GET: Transaksi/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detailTransaksi = await _transaksi.Details(id);

            if (detailTransaksi == null)
            {
                return NotFound();
            }

            return View(detailTransaksi);
        }

        // GET: Transaksi/Create
        public IActionResult Create()
        {
            var transaksi = new Transaksi();
            transaksi.TotalHargaTransaksi = 0;
            transaksi.DetailTransaksi.Add(new DetailTransaksi() { IdDetailTransaksi = "1", Harga = 0, Jumlah = 0 });

            ViewBag.ListProduk = GetListProduk();

            return View(transaksi);
        }

        // POST: Transaksi/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Transaksi transaksi)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _transaksi.Create(transaksi);

                    TempData["success"] = "Transaksi berhasil di simpan";
                }
                catch (Exception ex)
                {
                    TempData["error"] = "Transaksi gagal di simpan";
                }

                return RedirectToAction(nameof(Index));
            }

            return View(transaksi);
        }

        // GET: Transaksi/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaksi = await _transaksi.GetTransaksiById(id);

            if (transaksi == null)
            {
                return NotFound();
            }

            ViewBag.ListProduk = GetListProduk();

            return View(transaksi);
        }

        // POST: Transaksi/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Transaksi transaksi)
        {
            if (id != transaksi.IdTransaksi)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _transaksi.Edit(id, transaksi);

                    TempData["success"] = "Transaksi berhasil di edit";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_transaksi.IsTransaksiExists(transaksi.IdTransaksi))
                    {
                        return NotFound();
                    }
                    else
                    {
                        TempData["error"] = "Transaksi gagal di edit";
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(transaksi);
        }

        // POST: Transaksi/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            try
            {
                await _transaksi.Delete(id);

                TempData["success"] = "Transaksi berhasil dihapus";
            }
            catch (Exception ex)
            {
                TempData["error"] = "Transaksi gagal dihapus";
            }

            return RedirectToAction(nameof(Index));
        }

        private List<SelectListItem> GetListProduk()
        {
            var items = _produk.GetAllProduk();

            var listKategori = items.Result.Select(ut => new SelectListItem()
            {
                Value = ut.IdProduk.ToString(),
                Text = ut.NamaProduk
            }).ToList();

            var defItem = new SelectListItem()
            {
                Value = "",
                Text = "-- Pilih Produk --"
            };

            listKategori.Insert(0, defItem);

            return listKategori;
        }
    }
}
