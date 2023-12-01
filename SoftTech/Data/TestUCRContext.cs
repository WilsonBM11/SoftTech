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

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Payroll> Payrolls { get; set; }

    public virtual DbSet<Salary> Salaries { get; set; }

    public virtual DbSet<Vacation> Vacations { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=163.178.173.130;Database=Aplicada_WF;User ID=basesdedatos; Password=rpbases.2022;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__Client__3213E83F15C94E58");

            entity.ToTable("Client");

            entity.Property(e => e.id)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.address).IsUnicode(false);
            entity.Property(e => e.email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.phone_number)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.profile_picture).IsUnicode(false);
            entity.Property(e => e.userName)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK_Dept");

            entity.ToTable("Department");

            entity.Property(e => e.id)
                .HasMaxLength(36)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK_Emp");

            entity.ToTable("Employee");

            entity.Property(e => e.id)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.birthdate).HasColumnType("date");
            entity.Property(e => e.contract_date).HasColumnType("date");
            entity.Property(e => e.id_depto)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.id_user).HasMaxLength(450);
            entity.Property(e => e.identification)
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.HasOne(d => d.id_deptoNavigation).WithMany(p => p.Employees)
                .HasForeignKey(d => d.id_depto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Employee");
        });

        modelBuilder.Entity<Payroll>(entity =>
        {
            entity.ToTable("Payroll");

            entity.Property(e => e.id)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.date_payroll).HasColumnType("date");
        });

        modelBuilder.Entity<Salary>(entity =>
        {
            entity.ToTable("Salary");

            entity.Property(e => e.id)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.id_employee)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.id_payroll)
                .HasMaxLength(36)
                .IsUnicode(false);

            entity.HasOne(d => d.id_employeeNavigation).WithMany(p => p.Salaries)
                .HasForeignKey(d => d.id_employee)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalaryEmployee");

            entity.HasOne(d => d.id_payrollNavigation).WithMany(p => p.Salaries)
                .HasForeignKey(d => d.id_payroll)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalaryPayroll");
        });

        modelBuilder.Entity<Vacation>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK_Vac");

            entity.Property(e => e.id)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.date_vac).HasColumnType("date");
            entity.Property(e => e.id_emp)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.status_vac)
                .HasMaxLength(8)
                .IsUnicode(false);

            entity.HasOne(d => d.id_empNavigation).WithMany(p => p.Vacations)
                .HasForeignKey(d => d.id_emp)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Vacations");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
