﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace U3A1.Models.Entities;

public partial class NeatContext : DbContext
{
    public NeatContext()
    {
    }

    public NeatContext(DbContextOptions<NeatContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Clasificacion> Clasificacion { get; set; }

    public virtual DbSet<Menu> Menu { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseMySql("server=localhost;user=root;password=root;database=Neat", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.31-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("latin1_swedish_ci")
            .HasCharSet("latin1");

        modelBuilder.Entity<Clasificacion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("clasificacion");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Nombre).HasMaxLength(45);
        });

        modelBuilder.Entity<Menu>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("menu");

            entity.HasIndex(e => e.IdClasificacion, "fkclas_idx");

            entity.Property(e => e.Descripción).HasColumnType("text");
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Precio).HasColumnType("double(8,2)");
            entity.Property(e => e.PrecioPromocion).HasColumnType("double(8,2)");

            entity.HasOne(d => d.IdClasificacionNavigation).WithMany(p => p.Menu)
                .HasForeignKey(d => d.IdClasificacion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fkclas");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
