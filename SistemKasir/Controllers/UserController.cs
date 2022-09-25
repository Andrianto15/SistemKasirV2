using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemKasir.Interfaces;
using SistemKasir.Models;

namespace SistemKasir.Controllers
{
    public class UserController : Controller
    {
        private readonly IUser _user;

        public UserController(IUser user)
        {
            _user = user;
        }

        // GET: User
        public async Task<IActionResult> Index()
        {
              return View(await _user.GetAllUser());
        }

        // GET: User/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(User user)
        {
            if (user.Password == null)
            {
                ModelState.AddModelError("Password", "Password harus diisi.");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    await _user.Create(user);

                    TempData["success"] = "User berhasil di simpan";
                }
                catch (Exception ex)
                {
                    TempData["error"] = "User gagal di simpan";
                }

                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: User/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _user.GetUserById(id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, User user)
        {
            if (id != user.IdUser)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _user.Edit(user);

                    TempData["success"] = "User berhasil di update";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_user.IsUserExists(user.IdUser))
                    {
                        return NotFound();
                    }
                    else
                    {
                        TempData["error"] = "User gagal di update";
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            try
            {
                await _user.Delete(id);

                TempData["success"] = "User berhasil dihapus";
            }
            catch (Exception ex)
            {
                TempData["error"] = "User gagal dihapus";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
