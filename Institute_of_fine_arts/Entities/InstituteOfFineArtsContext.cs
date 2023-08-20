using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Microsoft.EntityFrameworkCore;

namespace Institute_of_fine_arts.Entities;

public partial class InstituteOfFineArtsContext : DbContext
{
    public static string connectionString;
    public InstituteOfFineArtsContext()
    {
    }

    public InstituteOfFineArtsContext(DbContextOptions<InstituteOfFineArtsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Art> Arts { get; set; }

    public virtual DbSet<Competition> Competitions { get; set; }

    public virtual DbSet<Evaluate> Evaluates { get; set; }

    public virtual DbSet<Exibition> Exibitions { get; set; }

    public virtual DbSet<Prize> Prizes { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer(connectionString);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Art>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__arts__3213E83FAB2D4645");

            entity.ToTable("arts");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CompetitionId).HasColumnName("competition_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Description)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.Favorite).HasColumnName("favorite");
            entity.Property(e => e.IsExibition)
                .HasDefaultValueSql("((0))")
                .HasColumnName("is_exibition");
            entity.Property(e => e.IsSell)
                .HasDefaultValueSql("((0))")
                .HasColumnName("is_sell");
            entity.Property(e => e.IsSold)
                .HasDefaultValueSql("((0))")
                .HasColumnName("is_sold");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("price");
            entity.Property(e => e.PrizeId).HasColumnName("prize_id");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Competition).WithMany(p => p.Arts)
                .HasForeignKey(d => d.CompetitionId)
                .HasConstraintName("FK__arts__competitio__33D4B598");

            entity.HasOne(d => d.Prize).WithMany(p => p.Arts)
                .HasForeignKey(d => d.PrizeId)
                .HasConstraintName("FK__arts__prize_id__34C8D9D1");

            entity.HasOne(d => d.User).WithMany(p => p.Arts)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__arts__user_id__32E0915F");
        });

        modelBuilder.Entity<Competition>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__competit__3213E83FF96DDD47");

            entity.ToTable("competitions");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Description)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.EndDate)
                .HasColumnType("date")
                .HasColumnName("end_date");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Slug)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("slug");
            entity.Property(e => e.StartDate)
                .HasColumnType("date")
                .HasColumnName("start_date");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("status");
            entity.Property(e => e.Theme)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("theme");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserCreate).HasColumnName("user_create");

            entity.HasOne(d => d.UserCreateNavigation).WithMany(p => p.Competitions)
                .HasForeignKey(d => d.UserCreate)
                .HasConstraintName("FK__competiti__user___398D8EEE");
        });

        modelBuilder.Entity<Evaluate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__evaluate__3213E83FC311AEA7");

            entity.ToTable("evaluates");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Color).HasColumnName("color");
            entity.Property(e => e.CompetitionId).HasColumnName("competition_id");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Creative).HasColumnName("creative");
            entity.Property(e => e.Feedback)
                .IsUnicode(false)
                .HasColumnName("feedback");
            entity.Property(e => e.Layout).HasColumnName("layout");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("status");
            entity.Property(e => e.Total)
                .HasColumnType("decimal(4, 3)")
                .HasColumnName("total");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Competition).WithMany(p => p.Evaluates)
                .HasForeignKey(d => d.CompetitionId)
                .HasConstraintName("FK__evaluates__compe__37A5467C");

            entity.HasOne(d => d.User).WithMany(p => p.Evaluates)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__evaluates__user___38996AB5");
        });

        modelBuilder.Entity<Exibition>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__exibitio__3213E83F1E9465A8");

            entity.ToTable("exibitions");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Description)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.EndDate)
                .HasColumnType("date")
                .HasColumnName("end_date");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Slug)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("slug");
            entity.Property(e => e.StartDate)
                .HasColumnType("date")
                .HasColumnName("start_date");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("status");
            entity.Property(e => e.Theme)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("theme");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Exibitions)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__exibition__user___3A81B327");
        });

        modelBuilder.Entity<Prize>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__prizes__3213E83FCAD648E4");

            entity.ToTable("prizes");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ConpetitionId).HasColumnName("conpetition_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Detail)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("detail");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserCreate).HasColumnName("user_create");

            entity.HasOne(d => d.Conpetition).WithMany(p => p.Prizes)
                .HasForeignKey(d => d.ConpetitionId)
                .HasConstraintName("FK__prizes__conpetit__286302EC");

            entity.HasOne(d => d.UserCreateNavigation).WithMany(p => p.Prizes)
                .HasForeignKey(d => d.UserCreate)
                .HasConstraintName("FK__prizes__user_cre__3B75D760");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__roles__3213E83F084FC37A");

            entity.ToTable("roles");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("date")
                .HasColumnName("created_at");
            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("date")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__users__3213E83F06BC5FE8");

            entity.ToTable("users");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("address");
            entity.Property(e => e.Birthday)
                .HasColumnType("date")
                .HasColumnName("birthday");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.JoinDate)
                .HasColumnType("date")
                .HasColumnName("join_date");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("status");
            entity.Property(e => e.Tel)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("tel");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__users__role_id__2D27B809");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
