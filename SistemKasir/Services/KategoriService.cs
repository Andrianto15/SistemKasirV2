using Microsoft.EntityFrameworkCore;
using SistemKasir.Data;
using SistemKasir.Interfaces;
using SistemKasir.Models;

namespace SistemKasir.Services
{
    public class KategoriService : IKategori
    {
        private readonly SistemKasirContext _db;

        public KategoriService(SistemKasirContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Kategori>> GetAllKategori()
        {
            return await _db.Kategori.OrderBy(d => d.Deskripsi).ToListAsync();
        }

        public async Task<Kategori> GetKategoriById(string Id)
        {
            return await _db.Kategori.Where(d => d.IdKategori == Id).FirstOrDefaultAsync();
        }

        public async Task Create(Kategori kategori)
        {
            var prefix = "KAT";
            var masterIdData = _db.MasterId.Where(d => d.PrefixId == prefix)?.FirstOrDefault();
            var idKategori = GenerateIdServices.GetID(prefix, masterIdData);

            if (idKategori != null)
            {
                // Simpan Kategori
                kategori.IdKategori = idKategori;
                _db.Add(kategori);

                // Update counter table Master ID
                masterIdData.Counter = masterIdData.Counter + 1;
                _db.MasterId.Update(masterIdData);

                await _db.SaveChangesAsync();
            }
        }

        public async Task Edit(Kategori kategori)
        {
            _db.Update(kategori);
            await _db.SaveChangesAsync();
        }

        public async Task Delete(string id)
        {
            var kategori = await _db.Kategori.FindAsync(id);

            if (kategori != null)
            {
                _db.Remove(kategori);
            }

            _ = _db.SaveChangesAsync();
        }

        public bool IsKategoriExists(string id)
        {
            return (_db.Kategori?.Any(e => e.IdKategori == id)).GetValueOrDefault();
        }
    }
}
