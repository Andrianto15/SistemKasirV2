using SistemKasir.Models;
using SistemKasir.ViewModels;

namespace SistemKasir.Interfaces
{
    public interface IProduk
    {
        //Task<IEnumerable<ProdukViewModel>> GetAllProduk();
        Task<IEnumerable<Produk>> GetAllProduk();
        Task<Produk> GetProdukById(string id);
        Task Create(Produk produk);
        Task Edit(Produk produk);
        Task Delete(string id);

        bool IsProdukExists(string id);
    }
}
