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

    public string? Avatar { get; set; }

    public string? Telephone { get; set; }

    public int? RoleId { get; set; }

    public string? Address { get; set; }

    public DateTime? JoinDate { get; set; }

    public string? Status { get; set; }

    public int? UserCreate { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Art> Arts { get; set; } = new List<Art>();

    public virtual ICollection<Evaluate> Evaluates { get; set; } = new List<Evaluate>();

    public virtual ICollection<Judge> JudgeTeacherId1Navigations { get; set; } = new List<Judge>();

    public virtual ICollection<Judge> JudgeTeacherId2Navigations { get; set; } = new List<Judge>();

    public virtual ICollection<Judge> JudgeTeacherId3Navigations { get; set; } = new List<Judge>();

    public virtual ICollection<Judge> JudgeTeacherId4Navigations { get; set; } = new List<Judge>();

    public virtual Role? Role { get; set; }

    public virtual Manager? UserCreateNavigation { get; set; }
}
