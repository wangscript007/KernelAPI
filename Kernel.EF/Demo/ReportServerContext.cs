using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Kernel.EF.Demo
{
    public partial class ReportServerContext : DbContext
    {
        public ReportServerContext()
        {
        }

        public ReportServerContext(DbContextOptions<ReportServerContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=127.0.0.1;Initial Catalog=ReportServer2;User Id=sa;Password=sa;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Roles>(entity =>
            {
                entity.HasKey(e => e.RoleId)
                    .IsClustered(false);

                entity.HasIndex(e => e.RoleName)
                    .HasName("IX_Roles")
                    .IsUnique()
                    .IsClustered();

                entity.Property(e => e.RoleId)
                    .HasColumnName("RoleID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Description).HasMaxLength(512);

                entity.Property(e => e.RoleName)
                    .IsRequired()
                    .HasMaxLength(260);

                entity.Property(e => e.TaskMask)
                    .IsRequired()
                    .HasMaxLength(32);
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .IsClustered(false);

                entity.HasIndex(e => new { e.Sid, e.UserName, e.AuthType })
                    .HasName("IX_Users")
                    .IsUnique()
                    .IsClustered();

                entity.Property(e => e.UserId)
                    .HasColumnName("UserID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Sid).HasMaxLength(85);

                entity.Property(e => e.UserName).HasMaxLength(260);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
