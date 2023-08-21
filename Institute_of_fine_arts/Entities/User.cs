using System;
using System.Collections.Generic;

namespace Institute_of_fine_arts.Entities;

public partial class User
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime? Birthday { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Tel { get; set; } = null!;

    public DateTime? JoinDate { get; set; }

    public int? RoleId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? Status { get; set; }

    public string? Address { get; set; }

    public virtual ICollection<Art>? Arts { get; set; } = new List<Art>();

    public virtual ICollection<Competition>? Competitions { get; set; } = new List<Competition>();

    public virtual ICollection<Evaluate>? Evaluates { get; set; } = new List<Evaluate>();

    public virtual ICollection<Exibition>? Exibitions { get; set; } = new List<Exibition>();

    public virtual ICollection<Prize>? Prizes { get; set; } = new List<Prize>();

    public virtual Role? Role { get; set; }
}
