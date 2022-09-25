using Microsoft.EntityFrameworkCore;
using SistemKasir.Data;
using SistemKasir.Interfaces;
using SistemKasir.Models;
using BC = BCrypt.Net.BCrypt;

namespace SistemKasir.Services
{
    public class UserService : IUser
    {
        private readonly SistemKasirContext _db;

        public UserService(SistemKasirContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<User>> GetAllUser()
        {
            return await _db.User.OrderBy(d => d.NamaUser).ToListAsync();
        }

        public async Task<User> GetUserById(string Id)
        {
            return await _db.User.Where(d => d.IdUser == Id).FirstOrDefaultAsync();
        }

        public async Task Create(User user)
        {
            var prefix = "USR";
            var masterIdData = _db.MasterId.Where(d => d.PrefixId == prefix)?.FirstOrDefault();
            var idUser = GenerateIdServices.GetID(prefix, masterIdData);

            if (idUser != null)
            {
                // hash password
                var passwordHash = BC.HashPassword(user.Password);

                // Simpan User
                user.IdUser = idUser;
                user.Password = passwordHash;
                user.DibuatTgl = DateTime.Now;
                user.DibuatOleh = "Admin";  // nanti di update dengan user yg login
                _db.Add(user);

                // Update counter table Master ID
                masterIdData.Counter = masterIdData.Counter + 1;
                _db.MasterId.Update(masterIdData);

                await _db.SaveChangesAsync();
            }
        }

        public async Task Edit(User user)
        {
            if (user.Password == null)
            {
                _db.Update(user);
                _db.Entry(user).Property(d => d.Password).IsModified = false;
            }
            else
            {
                _db.Update(user);
            }

            await _db.SaveChangesAsync();
        }

        public async Task Delete(string id)
        {
            var user = await _db.User.FindAsync(id);

            if (user != null)
            {
                _db.Remove(user);
            }

            _ = _db.SaveChangesAsync();
        }

        public bool IsUserExists(string id)
        {
            return (_db.User?.Any(e => e.IdUser == id)).GetValueOrDefault();
        }

        public bool Authenticate(Login model)
        {
            var user = _db.User.SingleOrDefault(x => x.Username == model.Username);

            // check account found and verify password
            return user != null && BC.Verify(model.Password, user.Password);
        }
    }
}
