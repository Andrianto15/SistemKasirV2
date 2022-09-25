using Microsoft.EntityFrameworkCore;
using SistemKasir.Data;
using SistemKasir.Interfaces;
using SistemKasir.Models;
using SistemKasir.ViewModels;

namespace SistemKasir.Services
{
    public class TransaksiService : ITransaksi
    {
        private readonly SistemKasirContext _db;

        public TransaksiService(SistemKasirContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Transaksi>> GetAllTransaksi()
        {
            return await _db.Transaksi.Include(d => d.User).OrderByDescending(e => e.IdTransaksi).ToListAsync();
        }

        public async Task<Transaksi> GetTransaksiById(string Id)
        {
            return await _db.Transaksi.Include(d => d.DetailTransaksi).Where(e => e.IdTransaksi == Id).FirstOrDefaultAsync();
        }

        public async Task<TransaksiViewModel> Details(string id)
        {
            var detailTransaksi = await _db.Transaksi
                .Include(d => d.DetailTransaksi)
                .Join(_db.User, t => t.IdUser, u => u.IdUser, (t, u) => new
                {
                    idTransaksi = t.IdTransaksi,
                    namaUser = u.NamaUser,
                    tanggalTransaksi = t.TglTransaksi,
                    totalTransaksi = t.TotalHargaTransaksi,
                })
                .Where(w => w.idTransaksi == id)
                .Select(s => new TransaksiViewModel()
                {
                    IdTransaksi = s.idTransaksi,
                    NamaUser = s.namaUser,
                    TglTransaksi = s.tanggalTransaksi,
                    TotalHargaTransaksi = s.totalTransaksi,
                }).FirstOrDefaultAsync();

            var detailProdukTransaksi = _db.DetailTransaksi
                .Join(_db.Produk, dt => dt.IdProduk, p => p.IdProduk, (det, pro) => new { det, pro })
                .Join(_db.Kategori, p => p.pro.IdKategori, k => k.IdKategori, (r, kat) => new { r, kat })
                .Where(d => d.r.det.IdTransaksi == id)
                .Select(s => new DetailTransaksiViewModel()
                {
                    IdDetailTransaksi = s.r.det.IdDetailTransaksi,
                    IdTransaksi = s.r.det.IdTransaksi,
                    IdKategori = s.r.pro.IdKategori,
                    IdProduk = s.r.det.IdProduk,
                    NamaKategori = s.kat.Deskripsi,
                    NamaProduk = s.r.pro.NamaProduk,
                    Jumlah = s.r.det.Jumlah,
                    Harga = s.r.det.Harga,
                    TotalHarga = s.r.det.TotalHarga
                }).ToList();

            detailTransaksi?.DetailTransaksi.AddRange(detailProdukTransaksi);

            return detailTransaksi;
        }

        public async Task Create(Transaksi transaksi)
        {
            transaksi.DetailTransaksi.RemoveAll(d => d.Jumlah == 0);
            transaksi.DetailTransaksi.RemoveAll(d => d.IsDeleted == true);

            // ID Transaksi
            var prefixTransaksi = "TRN";
            var masterIdDataTransaksi = _db.MasterId.Where(d => d.PrefixId == prefixTransaksi)?.FirstOrDefault();
            var idTransaksi = GenerateIdServices.GetID(prefixTransaksi, masterIdDataTransaksi);

            // ID Detail Transaksi
            var prefixDetailTransaksi = "DTR";
            var masterIdDataDetailTransaksi = _db.MasterId.Where(d => d.PrefixId == prefixDetailTransaksi)?.FirstOrDefault();
            var lastIdDetailTransaksiCount = masterIdDataDetailTransaksi?.Counter;
            var idDetailTransaksi = String.Empty;

            if (lastIdDetailTransaksiCount != null)
            {
                foreach (var item in transaksi.DetailTransaksi)
                {
                    idDetailTransaksi = prefixDetailTransaksi + String.Format("{0:D7}", lastIdDetailTransaksiCount);
                    lastIdDetailTransaksiCount++;

                    item.IdDetailTransaksi = idDetailTransaksi;
                    item.IdTransaksi = idTransaksi;
                }
            }

            // Simpan ke table Transaksi
            transaksi.IdTransaksi = idTransaksi;
            transaksi.IdUser = "USR0000001";    // nanti diganti dengan user yg login
            transaksi.TglTransaksi = DateTime.Now;
            _db.Add(transaksi);

            // Update counter table Master ID - Transaksi
            masterIdDataTransaksi.Counter = masterIdDataTransaksi.Counter + 1;
            _db.MasterId.Update(masterIdDataTransaksi);

            // Update counter table Master ID - Detail Transaksi
            masterIdDataDetailTransaksi.Counter = lastIdDetailTransaksiCount;
            _db.MasterId.Update(masterIdDataDetailTransaksi);

            await _db.SaveChangesAsync();
        }

        public async Task Edit(string id, Transaksi transaksi)
        {
            // Delete existing Detail Transaksi
            var existingDetailTransaksi = _db.DetailTransaksi.Where(d => d.IdTransaksi == id).ToList();
            _db.DetailTransaksi.RemoveRange(existingDetailTransaksi);
            await _db.SaveChangesAsync();

            transaksi.TglUpdateTransaksi = DateTime.Now;
            transaksi.DetailTransaksi.RemoveAll(d => d.Jumlah == 0);
            transaksi.DetailTransaksi.RemoveAll(d => d.IsDeleted == true);

            // ID Detail Transaksi
            var prefixDetailTransaksi = "DTR";
            var masterIdDataDetailTransaksi = _db.MasterId.Where(d => d.PrefixId == prefixDetailTransaksi)?.FirstOrDefault();
            var lastIdDetailTransaksiCount = masterIdDataDetailTransaksi?.Counter;
            var idDetailTransaksi = String.Empty;

            if (lastIdDetailTransaksiCount != null && transaksi.DetailTransaksi.Count > 0)
            {
                foreach (var item in transaksi.DetailTransaksi)
                {
                    if (item.IdDetailTransaksi == null && item.IdTransaksi == null)
                    {
                        idDetailTransaksi = prefixDetailTransaksi + String.Format("{0:D7}", lastIdDetailTransaksiCount);
                        lastIdDetailTransaksiCount++;

                        item.IdDetailTransaksi = idDetailTransaksi;
                        item.IdTransaksi = id;
                    }
                }
            }

            // Insert New Detail Transaksi
            _db.Transaksi.Update(transaksi);
            _db.Entry(transaksi).Property(d => d.TglTransaksi).IsModified = false;

            _db.DetailTransaksi.AddRange(transaksi.DetailTransaksi);

            // Update counter table Master ID - Detail Transaksi
            masterIdDataDetailTransaksi.Counter = lastIdDetailTransaksiCount;
            _db.MasterId.Update(masterIdDataDetailTransaksi);

            await _db.SaveChangesAsync();
        }

        public async Task Delete(string id)
        {
            // Delete Detail Transaksi
            var detailTransaksi = _db.DetailTransaksi.Where(d => d.IdTransaksi == id).ToList();
            _db.DetailTransaksi.RemoveRange(detailTransaksi);

            // Delete Transaksi
            var transaksi = await _db.Transaksi.Where(d => d.IdTransaksi == id).FirstOrDefaultAsync();

            if (transaksi != null)
            {
                _db.Transaksi.Remove(transaksi);
            }

            _ = _db.SaveChangesAsync();
        }

        public bool IsTransaksiExists(string id)
        {
            return (_db.Transaksi?.Any(e => e.IdTransaksi == id)).GetValueOrDefault();
        }
    }
}
