using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemKasir.Models
{
    public partial class Produk
    {
        public Produk()
        {
            DetailTransaksi = new HashSet<DetailTransaksi>();
        }

        [Key]
        public string? IdProduk { get; set; }

        [ForeignKey("Kategori")]
        [Display(Name = "Kategori")]
        [Required(ErrorMessage = "Kategori harus diisi.")]
        public string IdKategori { get; set; } = null!;

        [Display(Name = "Nama Produk")]
        [Required(ErrorMessage = "Nama Produk harus diisi.")]
        [StringLength(50, ErrorMessage = "Nama Produk tidak boleh lebih dari 50 karakter")]
        public string? NamaProduk { get; set; }

        [Display(Name = "Deskripsi Produk")]
        [StringLength(100, ErrorMessage = "Deskripsi Produk tidak boleh lebih dari 100 karakter")]
        public string? DeskripsiProduk { get; set; }

        public string? Satuan { get; set; }

        [Required(ErrorMessage = "Harga harus diisi.")]
        //[DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? Harga { get; set; }

        [Required(ErrorMessage = "Stok harus diisi.")]
        public int? Stok { get; set; }

        public DateTime? DibuatTgl { get; set; }

        public string? DibuatOleh { get; set; }

        public bool? Status { get; set; }

        public virtual Kategori? Kategori { get; set; }

        public virtual ICollection<DetailTransaksi> DetailTransaksi { get; set; }
    }
}
