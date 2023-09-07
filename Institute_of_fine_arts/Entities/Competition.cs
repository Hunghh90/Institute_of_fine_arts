using System;
using System.Collections.Generic;

namespace Institute_of_fine_arts.Entities;

public partial class Competition
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Slug { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public string? Theme { get; set; }

    public string? Description { get; set; }

    public string? Status { get; set; }

    public int? UserCreate { get; set; }

    public int? UserActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? Image { get; set; }

    public virtual ICollection<Art> Arts { get; set; } = new List<Art>();

    public virtual ICollection<Judge> Judges { get; set; }

    public virtual ICollection<Prize> Prizes { get; set; } = new List<Prize>();

    public virtual Manager? UserActiveNavigation { get; set; }

    public virtual Manager? UserCreateNavigation { get; set; }
}
