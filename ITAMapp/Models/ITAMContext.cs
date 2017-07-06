using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ITAMapp.Models
{
    public partial class ITAMContext : DbContext
    {
        public virtual DbSet<Categories> Categories { get; set; }
        public virtual DbSet<CustomFieldList> CustomFieldList { get; set; }
        public virtual DbSet<CustomFields> CustomFields { get; set; }
        public virtual DbSet<FieldValues> FieldValues { get; set; }
        public virtual DbSet<Identifications> Identifications { get; set; }
        public virtual DbSet<Properties> Properties { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            #warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
            optionsBuilder.UseSqlServer(@"Data Source=DANIEL\NHANSQLSERVER;Initial Catalog=ITAM;Integrated Security=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Categories>(entity =>
            {
                entity.HasKey(e => e.CategoryId)
                    .HasName("PK_Categories");

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.CategoryDescription)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.UrlName).HasMaxLength(20);
            });

            modelBuilder.Entity<CustomFieldList>(entity =>
            {
                entity.HasKey(e => e.ListId)
                    .HasName("PK_CustomFieldList");

                entity.Property(e => e.ListId).HasColumnName("ListID");

                entity.Property(e => e.FieldId).HasColumnName("FieldID");

                entity.Property(e => e.ListDescription)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<CustomFields>(entity =>
            {
                entity.HasKey(e => e.FieldId)
                    .HasName("PK_CustomFields");

                entity.Property(e => e.CollapsedBy).HasDefaultValueSql("0");

                entity.Property(e => e.FieldLabel)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.IsIdentifier)
                    .HasColumnName("isIdentifier")
                    .HasDefaultValueSql("0");
            });

            modelBuilder.Entity<FieldValues>(entity =>
            {
                entity.HasKey(e => e.ValueId)
                    .HasName("PK_FieldValues");

                entity.Property(e => e.FieldValue)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Identifications>(entity =>
            {
                entity.HasKey(e => e.IdentificationId)
                    .HasName("PK_Identifications");

                entity.Property(e => e.IdentificationId).HasColumnName("IdentificationID");

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.Identification)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Properties>(entity =>
            {
                entity.HasKey(e => e.PropertyId)
                    .HasName("PK_Properties");

                entity.Property(e => e.PropertyValue)
                    .IsRequired()
                    .HasMaxLength(100);
            });
        }
    }
}