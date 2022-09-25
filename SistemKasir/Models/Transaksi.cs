using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemKasir.Models
{
    public partial class Transaksi
    {
        [Key]
        [Display(Name = "ID Transaksi")]
        public string? IdTransaksi { get; set; }

        [ForeignKey("User")]
        public string? IdUser { get; set; }

        [Display(Name = "Total Transaksi")]
        public decimal? TotalHargaTransaksi { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Tanggal Transaksi")]
        public DateTime? TglTransaksi { get; set; }

        public DateTime? TglUpdateTransaksi { get; set; }

        public virtual User? User { get; set; }

        public virtual List<DetailTransaksi> DetailTransaksi { get; set; } = new List<DetailTransaksi>();
    }
}
