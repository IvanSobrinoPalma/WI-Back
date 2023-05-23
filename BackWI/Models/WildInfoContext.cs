using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BackWI.Models;

public partial class WildInfoContext : DbContext
{
    public WildInfoContext()
    {
    }

    public WildInfoContext(DbContextOptions<WildInfoContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Animal> Animals { get; set; }

    public virtual DbSet<AnimalsType> AnimalsTypes { get; set; }

    public virtual DbSet<Photograph> Photographs { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder){}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Animal>(entity =>
        {
            entity.HasKey(e => e.IdAnimal);

            entity.Property(e => e.IdAnimal)
                .ValueGeneratedNever()
                .HasColumnName("id_animal");
            entity.Property(e => e.DangerOfExtinction).HasColumnName("danger_of_extinction");
            entity.Property(e => e.Dangerousness).HasColumnName("dangerousness");
            entity.Property(e => e.IdNum).HasColumnName("id_num");
            entity.Property(e => e.Image)
                .HasColumnType("image")
                .HasColumnName("image");
            entity.Property(e => e.NameAnimal)
                .HasMaxLength(150)
                .HasColumnName("name_animal");
            entity.Property(e => e.ScientificName)
                .HasMaxLength(250)
                .HasColumnName("scientific_name");
            entity.Property(e => e.Type).HasColumnName("type");

            entity.HasOne(d => d.TypeNavigation).WithMany(p => p.Animals)
                .HasForeignKey(d => d.Type)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Animals_AnimalsType");
        });

        modelBuilder.Entity<AnimalsType>(entity =>
        {
            entity.HasKey(e => e.IdType);

            entity.ToTable("AnimalsType");

            entity.Property(e => e.IdType)
                .ValueGeneratedNever()
                .HasColumnName("id_type");
            entity.Property(e => e.NameType)
                .HasMaxLength(150)
                .HasColumnName("name_type");
            entity.Property(e => e.NumId)
                .ValueGeneratedOnAdd()
                .HasColumnName("num_id");
        });

        modelBuilder.Entity<Photograph>(entity =>
        {
            entity.HasNoKey();

            entity.HasIndex(e => e.IdAnimal, "PK2_Photographs");

            entity.Property(e => e.IdAnimal).HasColumnName("id_animal");
            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.Image)
                .HasColumnType("image")
                .HasColumnName("image");
            entity.Property(e => e.NumId).HasColumnName("num_id");

            entity.HasOne(d => d.IdAnimalNavigation).WithMany()
                .HasForeignKey(d => d.IdAnimal)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Photographs_Animals");

            entity.HasOne(d => d.IdUserNavigation).WithMany()
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Photographs_Users");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IdUser);

            entity.Property(e => e.IdUser)
                .ValueGeneratedNever()
                .HasColumnName("id_user");
            entity.Property(e => e.Email)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.FirtsSurname)
                .HasMaxLength(100)
                .HasColumnName("firts_surname");
            entity.Property(e => e.IdNum)
                .ValueGeneratedOnAdd()
                .HasColumnName("id_num");
            entity.Property(e => e.NameUser)
                .HasMaxLength(50)
                .HasColumnName("name_user");
            entity.Property(e => e.Nick)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nick");
            entity.Property(e => e.Password)
                .HasMaxLength(150)
                .HasColumnName("password");
            entity.Property(e => e.SecondSurname)
                .HasMaxLength(100)
                .HasColumnName("second_surname");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
