using System;
using System.Collections.Generic;

namespace Institute_of_fine_arts.Entities;

public partial class Manager
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime? Birthday { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Avatar { get; set; }

    public string? Telephone { get; set; }

    public int? RoleId { get; set; }

    public string? Address { get; set; }

    public DateTime? JoinDate { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Competition> CompetitionUserActiveNavigations { get; set; } = new List<Competition>();

    public virtual ICollection<Competition> CompetitionUserCreateNavigations { get; set; } = new List<Competition>();

    public virtual ICollection<ExibitionArt> ExibitionArtUserActiveNavigations { get; set; } = new List<ExibitionArt>();

    public virtual ICollection<ExibitionArt> ExibitionArtUserCreateNavigations { get; set; } = new List<ExibitionArt>();

    public virtual ICollection<Exibition> ExibitionUserActiveNavigations { get; set; } = new List<Exibition>();

    public virtual ICollection<Exibition> ExibitionUserCreateNavigations { get; set; } = new List<Exibition>();

    public virtual ICollection<Prize> PrizeUserActiveNavigations { get; set; } = new List<Prize>();

    public virtual ICollection<Prize> PrizeUserCreateNavigations { get; set; } = new List<Prize>();

    public virtual Role? Role { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
