using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SoftTech.Models;

namespace SoftTech.Data;

public partial class TestUCRContext : DbContext
{
    public TestUCRContext()
    {
    }

    public TestUCRContext(DbContextOptions<TestUCRContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Client> Client { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=163.178.173.130;Database=Aplicada_WF;User ID=basesdedatos; Password=rpbases.2022;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__Client__3213E83F5BF826C4");

            entity.ToTable("Client");

            entity.Property(e => e.id).ValueGeneratedNever();
            entity.Property(e => e.client_name)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.dir)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.profile_picture).IsUnicode(false);
            entity.Property(e => e.tel)
                .HasMaxLength(15)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
