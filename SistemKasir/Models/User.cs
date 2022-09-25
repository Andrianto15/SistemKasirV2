using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SistemKasir.Models
{
    public partial class User
    {
        public User()
        {
            Transaksi = new HashSet<Transaksi>();
        }

        [Key]
        public string? IdUser { get; set; }

        [Required(ErrorMessage = "Nama User harus diisi.")]
        [Display(Name = "Nama User")]
        public string? NamaUser { get; set; }

        [Required(ErrorMessage = "Username harus diisi.")]
        public string? Username { get; set; }

        public string? Password { get; set; }

        public string Role { get; set; } = null!;

        public DateTime? DibuatTgl { get; set; }

        public string? DibuatOleh { get; set; }

        public bool? Status { get; set; }

        public virtual ICollection<Transaksi> Transaksi { get; set; }
    }
}
