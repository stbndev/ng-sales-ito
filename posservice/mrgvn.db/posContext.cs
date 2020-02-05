namespace mrgvn.db
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class posContext : DbContext
    {
        public posContext()
            : base("name=posContext")
        {
        }

        public virtual DbSet<CSTATU> CSTATUS { get; set; }
        public virtual DbSet<LOSTITEMDETAIL> LOSTITEMDETAILS { get; set; }
        public virtual DbSet<LOSTITEM> LOSTITEMS { get; set; }
        public virtual DbSet<PRODUCTENTRy> PRODUCTENTRIES { get; set; }
        public virtual DbSet<PRODUCTENTRYDETAIL> PRODUCTENTRYDETAILS { get; set; }
        public virtual DbSet<PRODUCT> PRODUCTS { get; set; }
        public virtual DbSet<SALEDETAIL> SALEDETAILS { get; set; }
        public virtual DbSet<SALE> SALES { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CSTATU>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<CSTATU>()
                .Property(e => e.maker)
                .IsUnicode(false);

            modelBuilder.Entity<CSTATU>()
                .HasMany(e => e.LOSTITEMS)
                .WithRequired(e => e.CSTATU)
                .HasForeignKey(e => e.idcstatus)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CSTATU>()
                .HasMany(e => e.PRODUCTENTRIES)
                .WithRequired(e => e.CSTATU)
                .HasForeignKey(e => e.idcstatus)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CSTATU>()
                .HasMany(e => e.PRODUCTS)
                .WithRequired(e => e.CSTATU)
                .HasForeignKey(e => e.idcstatus)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<LOSTITEMDETAIL>()
                .Property(e => e.unitary_cost)
                .HasPrecision(10, 2);

            modelBuilder.Entity<LOSTITEM>()
                .Property(e => e.total)
                .HasPrecision(10, 2);

            modelBuilder.Entity<LOSTITEM>()
                .Property(e => e.maker)
                .IsUnicode(false);

            modelBuilder.Entity<LOSTITEM>()
                .HasMany(e => e.LOSTITEMDETAILS)
                .WithRequired(e => e.LOSTITEM)
                .HasForeignKey(e => e.idlostitems)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PRODUCTENTRy>()
                .Property(e => e.total)
                .HasPrecision(10, 2);

            modelBuilder.Entity<PRODUCTENTRy>()
                .HasMany(e => e.PRODUCTENTRYDETAILS)
                .WithRequired(e => e.PRODUCTENTRy)
                .HasForeignKey(e => e.idproductentries)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PRODUCTENTRYDETAIL>()
                .Property(e => e.unitary_cost)
                .HasPrecision(10, 2);

            modelBuilder.Entity<PRODUCT>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<PRODUCT>()
                .Property(e => e.barcode)
                .IsUnicode(false);

            modelBuilder.Entity<PRODUCT>()
                .Property(e => e.unitary_price)
                .HasPrecision(10, 2);

            modelBuilder.Entity<PRODUCT>()
                .Property(e => e.unitary_cost)
                .HasPrecision(10, 2);

            modelBuilder.Entity<PRODUCT>()
                .HasMany(e => e.LOSTITEMDETAILS)
                .WithRequired(e => e.PRODUCT)
                .HasForeignKey(e => e.idproducts)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PRODUCT>()
                .HasMany(e => e.PRODUCTENTRYDETAILS)
                .WithRequired(e => e.PRODUCT)
                .HasForeignKey(e => e.idproducts)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PRODUCT>()
                .HasMany(e => e.SALEDETAILS)
                .WithRequired(e => e.PRODUCT)
                .HasForeignKey(e => e.idproducts)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SALEDETAIL>()
                .Property(e => e.unitary_cost)
                .HasPrecision(10, 2);

            modelBuilder.Entity<SALEDETAIL>()
                .Property(e => e.unitary_price)
                .HasPrecision(10, 2);

            modelBuilder.Entity<SALE>()
                .Property(e => e.total)
                .HasPrecision(10, 2);

            modelBuilder.Entity<SALE>()
                .Property(e => e.maker)
                .IsUnicode(false);

            modelBuilder.Entity<SALE>()
                .HasMany(e => e.SALEDETAILS)
                .WithRequired(e => e.SALE)
                .HasForeignKey(e => e.idsales)
                .WillCascadeOnDelete(false);
        }
    }
}
