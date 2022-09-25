using SistemKasir.Models;

namespace SistemKasir.Interfaces
{
    public interface IUser
    {
        Task<IEnumerable<User>> GetAllUser();
        Task<User> GetUserById(string id);
        Task Create(User produk);
        Task Edit(User produk);
        Task Delete(string id);

        bool IsUserExists(string id);
    }
}
