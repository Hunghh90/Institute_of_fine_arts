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
            entity.HasKey(e => e.Id).HasName("PK__arts__3213E83FBBC082C2");

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
                .HasConstraintName("FK__arts__competitio__412EB0B6");

            entity.HasOne(d => d.Owner).WithMany(p => p.Arts)
                .HasForeignKey(d => d.OwnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__arts__owner_id__403A8C7D");

            entity.HasOne(d => d.Prize).WithMany(p => p.Arts)
                .HasForeignKey(d => d.PrizeId)
                .HasConstraintName("FK__arts__prize_id__4222D4EF");
        });

        modelBuilder.Entity<Competition>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__competit__3213E83F166C434F");

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
                .HasConstraintName("FK__competiti__user___440B1D61");

            entity.HasOne(d => d.UserCreateNavigation).WithMany(p => p.CompetitionUserCreateNavigations)
                .HasForeignKey(d => d.UserCreate)
                .HasConstraintName("FK__competiti__user___4316F928");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__customer__3213E83F287E705E");

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
            entity.HasKey(e => e.Id).HasName("PK__evaluate__3213E83F182ED0A5");

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
                .HasConstraintName("FK__evaluates__arts___44FF419A");

            entity.HasOne(d => d.Teacher).WithMany(p => p.Evaluates)
                .HasForeignKey(d => d.TeacherId)
                .HasConstraintName("FK__evaluates__teach__45F365D3");
        });

        modelBuilder.Entity<Exibition>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__exibitio__3213E83F05D8DC11");

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
                .HasConstraintName("FK__exibition__user___4BAC3F29");

            entity.HasOne(d => d.UserCreateNavigation).WithMany(p => p.ExibitionUserCreateNavigations)
                .HasForeignKey(d => d.UserCreate)
                .HasConstraintName("FK__exibition__user___4AB81AF0");
        });

        modelBuilder.Entity<ExibitionArt>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__exibitio__3213E83FB51C643C");

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
                .HasConstraintName("FK__exibition__art_i__47DBAE45");

            entity.HasOne(d => d.Exibition).WithMany(p => p.ExibitionArts)
                .HasForeignKey(d => d.ExibitionId)
                .HasConstraintName("FK__exibition__exibi__46E78A0C");

            entity.HasOne(d => d.UserActiveNavigation).WithMany(p => p.ExibitionArtUserActiveNavigations)
                .HasForeignKey(d => d.UserActive)
                .HasConstraintName("FK__exibition__user___49C3F6B7");

            entity.HasOne(d => d.UserCreateNavigation).WithMany(p => p.ExibitionArtUserCreateNavigations)
                .HasForeignKey(d => d.UserCreate)
                .HasConstraintName("FK__exibition__user___48CFD27E");
        });

        modelBuilder.Entity<Judge>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__judges__3213E83FF81A0A78");

            entity.ToTable("judges");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CompetitionId).HasColumnName("competition_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("status");
            entity.Property(e => e.TeacherId1).HasColumnName("teacher_id1");
            entity.Property(e => e.TeacherId2).HasColumnName("teacher_id2");
            entity.Property(e => e.TeacherId3).HasColumnName("teacher_id3");
            entity.Property(e => e.TeacherId4).HasColumnName("teacher_id4");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserActive).HasColumnName("user_active");
            entity.Property(e => e.UserCreater).HasColumnName("user_creater");

            entity.HasOne(d => d.Competition).WithMany(p => p.Judges)
                .HasForeignKey(d => d.CompetitionId)
                .HasConstraintName("FK__judges__competit__66603565");

            entity.HasOne(d => d.TeacherId1Navigation).WithMany(p => p.JudgeTeacherId1Navigations)
                .HasForeignKey(d => d.TeacherId1)
                .HasConstraintName("FK__judges__teacher___60A75C0F");

            entity.HasOne(d => d.TeacherId2Navigation).WithMany(p => p.JudgeTeacherId2Navigations)
                .HasForeignKey(d => d.TeacherId2)
                .HasConstraintName("FK__judges__teacher___619B8048");

            entity.HasOne(d => d.TeacherId3Navigation).WithMany(p => p.JudgeTeacherId3Navigations)
                .HasForeignKey(d => d.TeacherId3)
                .HasConstraintName("FK__judges__teacher___628FA481");

            entity.HasOne(d => d.TeacherId4Navigation).WithMany(p => p.JudgeTeacherId4Navigations)
                .HasForeignKey(d => d.TeacherId4)
                .HasConstraintName("FK__judges__teacher___6383C8BA");

            entity.HasOne(d => d.UserActiveNavigation).WithMany(p => p.JudgeUserActiveNavigations)
                .HasForeignKey(d => d.UserActive)
                .HasConstraintName("FK__judges__user_act__656C112C");

            entity.HasOne(d => d.UserCreaterNavigation).WithMany(p => p.JudgeUserCreaterNavigations)
                .HasForeignKey(d => d.UserCreater)
                .HasConstraintName("FK__judges__user_cre__6477ECF3");
        });

        modelBuilder.Entity<Manager>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__managers__3213E83FEAB4C26C");

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
                .HasConstraintName("FK__managers__role_i__5070F446");
        });

        modelBuilder.Entity<Prize>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__prizes__3213E83F76BEE8E8");

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
                .HasConstraintName("FK__prizes__conpetit__5165187F");

            entity.HasOne(d => d.UserActiveNavigation).WithMany(p => p.PrizeUserActiveNavigations)
                .HasForeignKey(d => d.UserActive)
                .HasConstraintName("FK__prizes__user_act__534D60F1");

            entity.HasOne(d => d.UserCreateNavigation).WithMany(p => p.PrizeUserCreateNavigations)
                .HasForeignKey(d => d.UserCreate)
                .HasConstraintName("FK__prizes__user_cre__52593CB8");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__roles__3213E83F80FE48EE");

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
            entity.HasKey(e => e.Id).HasName("PK__users__3213E83FF3843C5F");

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
                .HasConstraintName("FK__users__role_id__5441852A");

            entity.HasOne(d => d.UserCreateNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.UserCreate)
                .HasConstraintName("FK__users__user_crea__5535A963");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
