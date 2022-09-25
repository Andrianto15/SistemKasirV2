using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemKasir.Models
{
    public partial class DetailTransaksi
    {
        [Key]
        public string? IdDetailTransaksi { get; set; }

        [ForeignKey("Transaksi")]
        public string? IdTransaksi { get; set; }

        [ForeignKey("Produk")]
        [Required(ErrorMessage = "Produk harus diisi")]
        public string IdProduk { get; set; } = null!;

        [Required]
        [Range(1, 100, ErrorMessage = "Jumlah harus lebih dari 1 dan kurang dari 100")]
        public int? Jumlah { get; set; }

        [Required(ErrorMessage = "Harga harus diisi")]
        [Display(Name = "Harga Satuan")]
        public decimal? Harga { get; set; }

        public decimal? TotalHarga
        {
            get
            {
                return Jumlah * Harga;
            }

            set
            {
            }
        }

        [NotMapped]
        public bool IsDeleted { get; set; } = false;

        [NotMapped]
        public decimal Total { get; set; }

        public virtual Produk? Produk { get; set; }

        public virtual Transaksi? Transaksi { get; set; }
    }
}
