using Microsoft.EntityFrameworkCore;
using SistemKasir.Data;
using SistemKasir.Interfaces;
using SistemKasir.Models;
using SistemKasir.ViewModels;

namespace SistemKasir.Services
{
    public class ProdukService : IProduk
    {
        private readonly SistemKasirContext _db;

        public ProdukService(SistemKasirContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Produk>> GetAllProduk()
        {
            return await _db.Produk.Include(d => d.Kategori).OrderBy(e => e.NamaProduk).ToListAsync();
        }

        public async Task<Produk> GetProdukById(string Id)
        {
            return await _db.Produk.Where(d => d.IdProduk == Id).FirstOrDefaultAsync();
        }

        public async Task Create(Produk kategori)
        {
            var prefix = "PRD";
            var masterIdData = _db.MasterId.Where(d => d.PrefixId == prefix)?.FirstOrDefault();
            var idProduk = GenerateIdServices.GetID(prefix, masterIdData);

            if (idProduk != null)
            {
                // Simpan Produk
                kategori.IdProduk = idProduk;
                kategori.DibuatTgl = DateTime.Now;
                kategori.DibuatOleh = "Admin";  // nanti di update dengan user yg login
                _db.Add(kategori);

                // Update counter table Master ID
                masterIdData.Counter = masterIdData.Counter + 1;
                _db.MasterId.Update(masterIdData);

                await _db.SaveChangesAsync();
            }
        }

        public async Task Edit(Produk kategori)
        {
            _db.Update(kategori);
            await _db.SaveChangesAsync();
        }

        public async Task Delete(string id)
        {
            var kategori = await _db.Produk.FindAsync(id);

            if (kategori != null)
            {
                _db.Remove(kategori);
            }

            _ = _db.SaveChangesAsync();
        }

        public bool IsProdukExists(string id)
        {
            return (_db.Produk?.Any(e => e.IdProduk == id)).GetValueOrDefault();
        }
    }
}
