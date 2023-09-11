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
        modelBuilder.Entity<Art>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__arts__3213E83F7B72C0D4");

            entity.ToTable("arts");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CompetitionId).HasColumnName("competition_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("date")
                .HasColumnName("created_at");
            entity.Property(e => e.Description)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.Granded)
                .HasDefaultValueSql("((0))")
                .HasColumnName("granded");
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
            entity.Property(e => e.TotalScore)
                .HasColumnType("decimal(4, 2)")
                .HasColumnName("total_score");
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
                .HasConstraintName("FK__arts__competitio__5535A963");

            entity.HasOne(d => d.Owner).WithMany(p => p.Arts)
                .HasForeignKey(d => d.OwnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__arts__owner_id__5441852A");

            entity.HasOne(d => d.Prize).WithMany(p => p.Arts)
                .HasForeignKey(d => d.PrizeId)
                .HasConstraintName("FK__arts__prize_id__5629CD9C");
        });

        modelBuilder.Entity<Competition>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__competit__3213E83F06342B05");

            entity.ToTable("competitions");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Awards)
                .HasDefaultValueSql("((0))")
                .HasColumnName("awards");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("date")
                .HasColumnName("created_at");
            entity.Property(e => e.Description)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.EndDate)
                .HasColumnType("date")
                .HasColumnName("end_date");
            entity.Property(e => e.Image)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("image");
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
                .HasConstraintName("FK__competiti__user___5812160E");

            entity.HasOne(d => d.UserCreateNavigation).WithMany(p => p.CompetitionUserCreateNavigations)
                .HasForeignKey(d => d.UserCreate)
                .HasConstraintName("FK__competiti__user___571DF1D5");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__customer__3213E83F94D73056");

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
            entity.HasKey(e => e.Id).HasName("PK__evaluate__3213E83F64FBF9EA");

            entity.ToTable("evaluates");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ArtId).HasColumnName("art_id");
            entity.Property(e => e.Color).HasColumnName("color");
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
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("status");
            entity.Property(e => e.TeacherId).HasColumnName("teacher_id");
            entity.Property(e => e.Total)
                .HasColumnType("decimal(4, 2)")
                .HasColumnName("total");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Art).WithMany(p => p.Evaluates)
                .HasForeignKey(d => d.ArtId)
                .HasConstraintName("FK__evaluates__art_i__59063A47");

            entity.HasOne(d => d.Teacher).WithMany(p => p.Evaluates)
                .HasForeignKey(d => d.TeacherId)
                .HasConstraintName("FK__evaluates__teach__59FA5E80");
        });

        modelBuilder.Entity<Exibition>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__exibitio__3213E83FE30EEA9C");

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
                .HasConstraintName("FK__exibition__user___5FB337D6");

            entity.HasOne(d => d.UserCreateNavigation).WithMany(p => p.ExibitionUserCreateNavigations)
                .HasForeignKey(d => d.UserCreate)
                .HasConstraintName("FK__exibition__user___5EBF139D");
        });

        modelBuilder.Entity<ExibitionArt>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__exibitio__3213E83FC03F60E8");

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
                .HasConstraintName("FK__exibition__art_i__5BE2A6F2");

            entity.HasOne(d => d.Exibition).WithMany(p => p.ExibitionArts)
                .HasForeignKey(d => d.ExibitionId)
                .HasConstraintName("FK__exibition__exibi__5AEE82B9");

            entity.HasOne(d => d.UserActiveNavigation).WithMany(p => p.ExibitionArtUserActiveNavigations)
                .HasForeignKey(d => d.UserActive)
                .HasConstraintName("FK__exibition__user___5DCAEF64");

            entity.HasOne(d => d.UserCreateNavigation).WithMany(p => p.ExibitionArtUserCreateNavigations)
                .HasForeignKey(d => d.UserCreate)
                .HasConstraintName("FK__exibition__user___5CD6CB2B");
        });

        modelBuilder.Entity<Judge>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__judges__3213E83F7ADA936D");

            entity.ToTable("judges");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CompetitionId).HasColumnName("competition_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TeacherId).HasColumnName("teacher_id");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserActive).HasColumnName("user_active");
            entity.Property(e => e.UserCreate).HasColumnName("user_create");

            entity.HasOne(d => d.Competition).WithMany(p => p.Judges)
                .HasForeignKey(d => d.CompetitionId)
                .HasConstraintName("FK__judges__competit__60A75C0F");

            entity.HasOne(d => d.Teacher).WithMany(p => p.Judges)
                .HasForeignKey(d => d.TeacherId)
                .HasConstraintName("FK__judges__teacher___619B8048");

            entity.HasOne(d => d.UserActiveNavigation).WithMany(p => p.JudgeUserActiveNavigations)
                .HasForeignKey(d => d.UserActive)
                .HasConstraintName("FK__judges__user_act__6383C8BA");

            entity.HasOne(d => d.UserCreateNavigation).WithMany(p => p.JudgeUserCreateNavigations)
                .HasForeignKey(d => d.UserCreate)
                .HasConstraintName("FK__judges__user_cre__628FA481");
        });

        modelBuilder.Entity<Manager>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__managers__3213E83F73E37A98");

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
                .HasConstraintName("FK__managers__role_i__6477ECF3");
        });

        modelBuilder.Entity<Prize>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__prizes__3213E83FF7D29876");

            entity.ToTable("prizes");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ConpetitionId).HasColumnName("conpetition_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("date")
                .HasColumnName("created_at");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("price");
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
                .HasConstraintName("FK__prizes__conpetit__656C112C");

            entity.HasOne(d => d.UserActiveNavigation).WithMany(p => p.PrizeUserActiveNavigations)
                .HasForeignKey(d => d.UserActive)
                .HasConstraintName("FK__prizes__user_act__6754599E");

            entity.HasOne(d => d.UserCreateNavigation).WithMany(p => p.PrizeUserCreateNavigations)
                .HasForeignKey(d => d.UserCreate)
                .HasConstraintName("FK__prizes__user_cre__66603565");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__roles__3213E83FBFF978E3");

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
            entity.HasKey(e => e.Id).HasName("PK__users__3213E83F8FDA9B0F");

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
                .HasConstraintName("FK__users__role_id__68487DD7");

            entity.HasOne(d => d.UserCreateNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.UserCreate)
                .HasConstraintName("FK__users__user_crea__693CA210");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
