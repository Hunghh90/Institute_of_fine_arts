using System;
using System.Collections.Generic;

namespace Institute_of_fine_arts.Entities;

public partial class Competition
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string Theme { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? Status { get; set; }

    public int? UserCreate { get; set; }

    public string Slug { get; set; } = null!;

    public virtual ICollection<Art> Arts { get; set; } = new List<Art>();

    public virtual ICollection<Evaluate> Evaluates { get; set; } = new List<Evaluate>();

    public virtual ICollection<Prize> Prizes { get; set; } = new List<Prize>();

    public virtual User? UserCreateNavigation { get; set; }
}
