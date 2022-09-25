using SistemKasir.Models;

namespace SistemKasir.Interfaces
{
    public interface IKategori
    {
        Task<IEnumerable<Kategori>> GetAllKategori();
        Task<Kategori> GetKategoriById(string id);
        Task Create(Kategori kategori);
        Task Edit(Kategori kategori);
        Task Delete(string id);

        bool IsKategoriExists(string id);
    }
}
