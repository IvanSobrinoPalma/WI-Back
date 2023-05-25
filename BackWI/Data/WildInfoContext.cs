﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using BackWI.Models;
using Microsoft.EntityFrameworkCore;

namespace BackWI.Data
{
    public partial class WildInfoContext : DbContext
    {
        public WildInfoContext()
        {
        }

        public WildInfoContext(DbContextOptions<WildInfoContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AnimalTypes> AnimalTypes { get; set; }
        public virtual DbSet<Animals> Animals { get; set; }
        public virtual DbSet<Photographs> Photographs { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder){}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AnimalTypes>(entity =>
            {
                entity.HasKey(e => e.IdType)
                    .HasName("PK__AnimalTy__C3F091E08F0BDEAC");

                entity.HasIndex(e => e.NumId, "UQ__AnimalTy__FCEB8F7749D1F4A1")
                    .IsUnique();

                entity.Property(e => e.IdType)
                    .ValueGeneratedNever()
                    .HasColumnName("id_type");

                entity.Property(e => e.NameType)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("name_type");

                entity.Property(e => e.NumId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("num_id");
            });

            modelBuilder.Entity<Animals>(entity =>
            {
                entity.HasKey(e => e.IdAnimal)
                    .HasName("PK__Animals__1967CD2F00F068E0");

                entity.HasIndex(e => e.NumId, "UQ__Animals__FCEB8F77470D1ACA")
                    .IsUnique();

                entity.Property(e => e.IdAnimal)
                    .ValueGeneratedNever()
                    .HasColumnName("id_animal");

                entity.Property(e => e.DangerOfExtinction).HasColumnName("danger_of_extinction");

                entity.Property(e => e.Dangerousness).HasColumnName("dangerousness");

                entity.Property(e => e.Image)
                    .IsRequired()
                    .HasColumnType("image")
                    .HasColumnName("image");

                entity.Property(e => e.NameAnimal)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("name_animal");

                entity.Property(e => e.NumId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("num_id");

                entity.Property(e => e.ScientificName)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("scientific_name");

                entity.Property(e => e.TypeAnimal).HasColumnName("type_animal");

                entity.HasOne(d => d.TypeAnimalNavigation)
                    .WithMany(p => p.Animals)
                    .HasForeignKey(d => d.TypeAnimal)
                    .HasConstraintName("FK__Animals__type_an__2F10007B");
            });

            modelBuilder.Entity<Photographs>(entity =>
            {
                entity.HasKey(e => new { e.IdUser, e.IdAnimal })
                    .HasName("PK__Photogra__33473AE5928A1CA3");

                entity.HasIndex(e => e.NumId, "UQ__Photogra__FCEB8F77706ADBF3")
                    .IsUnique();

                entity.Property(e => e.IdUser).HasColumnName("id_user");

                entity.Property(e => e.IdAnimal).HasColumnName("id_animal");

                entity.Property(e => e.Image)
                    .IsRequired()
                    .HasColumnType("image")
                    .HasColumnName("image");

                entity.Property(e => e.NumId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("num_id");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.IdUser)
                    .HasName("PK__Users__D2D14637A0A0C579");

                entity.HasIndex(e => e.NumId, "UQ__Users__FCEB8F77FBBCD0A8")
                    .IsUnique();

                entity.Property(e => e.IdUser)
                    .ValueGeneratedNever()
                    .HasColumnName("id_user");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.FirtsSurname)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("firts_surname");

                entity.Property(e => e.NameUser)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("name_user");

                entity.Property(e => e.Nick)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nick");

                entity.Property(e => e.NumId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("num_id");

                entity.Property(e => e.Passwordd)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("passwordd");

                entity.Property(e => e.SecondSurname)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("second_surname");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}