using SistemKasir.Models;
using SistemKasir.ViewModels;

namespace SistemKasir.Interfaces
{
    public interface ITransaksi
    {
        Task<IEnumerable<Transaksi>> GetAllTransaksi();
        Task<Transaksi> GetTransaksiById(string id);
        Task<TransaksiViewModel> Details(string id);
        Task Create(Transaksi produk);
        Task Edit(string id, Transaksi produk);
        Task Delete(string id);

        bool IsTransaksiExists(string id);
    }
}
