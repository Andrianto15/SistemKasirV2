using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using SistemKasir.Models;

namespace SistemKasir.Data
{
    public partial class SistemKasirContext : DbContext
    {
        public SistemKasirContext()
        {
        }

        public SistemKasirContext(DbContextOptions<SistemKasirContext> options)
            : base(options)
        {
        }

        public virtual DbSet<DetailTransaksi> DetailTransaksi { get; set; } = null!;
        public virtual DbSet<Kategori> Kategori { get; set; } = null!;
        public virtual DbSet<MasterId> MasterId { get; set; } = null!;
        public virtual DbSet<Produk> Produk { get; set; } = null!;
        public virtual DbSet<Transaksi> Transaksi { get; set; } = null!;
        public virtual DbSet<User> User { get; set; } = null!;
    }
}
