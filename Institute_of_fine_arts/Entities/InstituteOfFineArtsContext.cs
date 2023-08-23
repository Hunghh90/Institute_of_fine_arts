using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Institute_of_fine_arts.Entities;

public partial class InstituteOfFineArtsContext : DbContext
{
    public static String connectionString;
    public InstituteOfFineArtsContext()
    {
    }

    public InstituteOfFineArtsContext(DbContextOptions<InstituteOfFineArtsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Art> Arts { get; set; }

    public virtual DbSet<Competition> Competitions { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Evaluate> Evaluates { get; set; }

    public virtual DbSet<Exibition> Exibitions { get; set; }

    public virtual DbSet<ExibitionArt> ExibitionArts { get; set; }

    public virtual DbSet<Judge> Judges { get; set; }

    public virtual DbSet<Manager> Managers { get; set; }

    public virtual DbSet<Prize> Prizes { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer(connectionString);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("SQL_Latin1_General_CP1_CI_AS");

        modelBuilder.Entity<Art>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__arts__3213E83F493618D6");

            entity.ToTable("arts");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CompetitionId).HasColumnName("competition_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("date")
                .HasColumnName("created_at");
            entity.Property(e => e.Description)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.IsExibition)
                .HasDefaultValueSql("((0))")
                .HasColumnName("is_exibition");
            entity.Property(e => e.IsSell)
                .HasDefaultValueSql("((0))")
                .HasColumnName("is_sell");
            entity.Property(e => e.IsSold)
                .HasDefaultValueSql("((0))")
                .HasColumnName("is_sold");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.OwnerId).HasColumnName("owner_id");
            entity.Property(e => e.Path)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("path");
            entity.Property(e => e.Price)
                .HasDefaultValueSql("((0))")
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("price");
            entity.Property(e => e.PrizeId).HasColumnName("prize_id");
            entity.Property(e => e.Slug)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("slug");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("date")
                .HasColumnName("updated_at");
            entity.Property(e => e.Url)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("url");

            entity.HasOne(d => d.Competition).WithMany(p => p.Arts)
                .HasForeignKey(d => d.CompetitionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__arts__competitio__7D439ABD");

            entity.HasOne(d => d.Owner).WithMany(p => p.Arts)
                .HasForeignKey(d => d.OwnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__arts__owner_id__7C4F7684");

            entity.HasOne(d => d.Prize).WithMany(p => p.Arts)
                .HasForeignKey(d => d.PrizeId)
                .HasConstraintName("FK__arts__prize_id__7E37BEF6");
        });

        modelBuilder.Entity<Competition>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__competit__3213E83F0252A309");

            entity.ToTable("competitions");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("date")
                .HasColumnName("created_at");
            entity.Property(e => e.Description)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.EndDate)
                .HasColumnType("date")
                .HasColumnName("end_date");
            entity.Property(e => e.Name)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Slug)
                .IsUnicode(false)
                .HasColumnName("slug");
            entity.Property(e => e.StartDate)
                .HasColumnType("date")
                .HasColumnName("start_date");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("('Pending')")
                .HasColumnName("status");
            entity.Property(e => e.Theme)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("theme");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("date")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserActive).HasColumnName("user_active");
            entity.Property(e => e.UserCreate).HasColumnName("user_create");

            entity.HasOne(d => d.UserActiveNavigation).WithMany(p => p.CompetitionUserActiveNavigations)
                .HasForeignKey(d => d.UserActive)
                .HasConstraintName("FK__competiti__user___6FE99F9F");

            entity.HasOne(d => d.UserCreateNavigation).WithMany(p => p.CompetitionUserCreateNavigations)
                .HasForeignKey(d => d.UserCreate)
                .HasConstraintName("FK__competiti__user___6EF57B66");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__customer__3213E83F1F8001F8");

            entity.ToTable("customers");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("address");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Telephone)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("telephone");
        });

        modelBuilder.Entity<Evaluate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__evaluate__3213E83FA20B9D03");

            entity.ToTable("evaluates");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ArtsId).HasColumnName("arts_id");
            entity.Property(e => e.Color).HasColumnName("color");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("date")
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
            entity.Property(e => e.TeacherId).HasColumnName("teacher_id");
            entity.Property(e => e.Total).HasColumnName("total");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("date")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Arts).WithMany(p => p.Evaluates)
                .HasForeignKey(d => d.ArtsId)
                .HasConstraintName("FK__evaluates__arts___01142BA1");

            entity.HasOne(d => d.Teacher).WithMany(p => p.Evaluates)
                .HasForeignKey(d => d.TeacherId)
                .HasConstraintName("FK__evaluates__teach__02084FDA");
        });

        modelBuilder.Entity<Exibition>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__exibitio__3213E83FCAC33B1A");

            entity.ToTable("exibitions");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("date")
                .HasColumnName("created_at");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.EndDate)
                .HasColumnType("date")
                .HasColumnName("end_date");
            entity.Property(e => e.Name)
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
                .HasDefaultValueSql("('Pending')")
                .HasColumnName("status");
            entity.Property(e => e.Theme)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("theme");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("date")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserActive).HasColumnName("user_active");
            entity.Property(e => e.UserCreate).HasColumnName("user_create");

            entity.HasOne(d => d.UserActiveNavigation).WithMany(p => p.ExibitionUserActiveNavigations)
                .HasForeignKey(d => d.UserActive)
                .HasConstraintName("FK__exibition__user___6B24EA82");

            entity.HasOne(d => d.UserCreateNavigation).WithMany(p => p.ExibitionUserCreateNavigations)
                .HasForeignKey(d => d.UserCreate)
                .HasConstraintName("FK__exibition__user___6A30C649");
        });

        modelBuilder.Entity<ExibitionArt>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__exibitio__3213E83F80897809");

            entity.ToTable("exibition_arts");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ArtId).HasColumnName("art_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("date")
                .HasColumnName("created_at");
            entity.Property(e => e.ExibitionId).HasColumnName("exibition_id");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("('Pending')")
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("date")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserActive).HasColumnName("user_active");
            entity.Property(e => e.UserCreate).HasColumnName("user_create");

            entity.HasOne(d => d.Art).WithMany(p => p.ExibitionArts)
                .HasForeignKey(d => d.ArtId)
                .HasConstraintName("FK__exibition__art_i__10566F31");

            entity.HasOne(d => d.Exibition).WithMany(p => p.ExibitionArts)
                .HasForeignKey(d => d.ExibitionId)
                .HasConstraintName("FK__exibition__exibi__0F624AF8");

            entity.HasOne(d => d.UserActiveNavigation).WithMany(p => p.ExibitionArtUserActiveNavigations)
                .HasForeignKey(d => d.UserActive)
                .HasConstraintName("FK__exibition__user___123EB7A3");

            entity.HasOne(d => d.UserCreateNavigation).WithMany(p => p.ExibitionArtUserCreateNavigations)
                .HasForeignKey(d => d.UserCreate)
                .HasConstraintName("FK__exibition__user___114A936A");
        });

        modelBuilder.Entity<Judge>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__judges__3213E83F2B839C3D");

            entity.ToTable("judges");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CompetitionId).HasColumnName("competition_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("date")
                .HasColumnName("created_at");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("('Pending')")
                .HasColumnName("status");
            entity.Property(e => e.TeacherId).HasColumnName("teacher_id");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("date")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserActive).HasColumnName("user_active");
            entity.Property(e => e.UserCreate).HasColumnName("user_create");

            entity.HasOne(d => d.Competition).WithMany(p => p.Judges)
                .HasForeignKey(d => d.CompetitionId)
                .HasConstraintName("FK__judges__competit__09A971A2");

            entity.HasOne(d => d.Teacher).WithMany(p => p.Judges)
                .HasForeignKey(d => d.TeacherId)
                .HasConstraintName("FK__judges__teacher___08B54D69");

            entity.HasOne(d => d.UserActiveNavigation).WithMany(p => p.JudgeUserActiveNavigations)
                .HasForeignKey(d => d.UserActive)
                .HasConstraintName("FK__judges__user_act__0B91BA14");

            entity.HasOne(d => d.UserCreateNavigation).WithMany(p => p.JudgeUserCreateNavigations)
                .HasForeignKey(d => d.UserCreate)
                .HasConstraintName("FK__judges__user_cre__0A9D95DB");
        });

        modelBuilder.Entity<Manager>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__managers__3213E83F848E5CD8");

            entity.ToTable("managers");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("address");
            entity.Property(e => e.Avatar)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("avatar");
            entity.Property(e => e.Birthday)
                .HasColumnType("date")
                .HasColumnName("birthday");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("date")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.JoinDate)
                .HasColumnType("date")
                .HasColumnName("join_date");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("status");
            entity.Property(e => e.Telephone)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("telephone");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("date")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Role).WithMany(p => p.Managers)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__managers__role_i__60A75C0F");
        });

        modelBuilder.Entity<Prize>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__prizes__3213E83F69662146");

            entity.ToTable("prizes");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ConpetitionId).HasColumnName("conpetition_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("date")
                .HasColumnName("created_at");
            entity.Property(e => e.Detail)
                .IsUnicode(false)
                .HasColumnName("detail");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.Slug)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("slug");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("('Pending')")
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("date")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserActive).HasColumnName("user_active");
            entity.Property(e => e.UserCreate).HasColumnName("user_create");

            entity.HasOne(d => d.Conpetition).WithMany(p => p.Prizes)
                .HasForeignKey(d => d.ConpetitionId)
                .HasConstraintName("FK__prizes__conpetit__72C60C4A");

            entity.HasOne(d => d.UserActiveNavigation).WithMany(p => p.PrizeUserActiveNavigations)
                .HasForeignKey(d => d.UserActive)
                .HasConstraintName("FK__prizes__user_act__75A278F5");

            entity.HasOne(d => d.UserCreateNavigation).WithMany(p => p.PrizeUserCreateNavigations)
                .HasForeignKey(d => d.UserCreate)
                .HasConstraintName("FK__prizes__user_cre__74AE54BC");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__roles__3213E83F4E988A10");

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
            entity.HasKey(e => e.Id).HasName("PK__users__3213E83F6FE341A7");

            entity.ToTable("users");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("address");
            entity.Property(e => e.Avatar)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("avatar");
            entity.Property(e => e.Birthday)
                .HasColumnType("date")
                .HasColumnName("birthday");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("date")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.JoinDate)
                .HasColumnType("date")
                .HasColumnName("join_date");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("status");
            entity.Property(e => e.Telephone)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("telephone");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("date")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserCreate).HasColumnName("user_create");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__users__role_id__6383C8BA");

            entity.HasOne(d => d.UserCreateNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.UserCreate)
                .HasConstraintName("FK__users__user_crea__6477ECF3");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
