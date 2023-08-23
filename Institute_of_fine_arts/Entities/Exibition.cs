using System;
using System.Collections.Generic;

namespace Institute_of_fine_arts.Entities;

public partial class Exibition
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Slug { get; set; } = null!;

    public string? Theme { get; set; }

    public string? Description { get; set; }

    public string? Status { get; set; }

    public int? UserCreate { get; set; }

    public int? UserActive { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<ExibitionArt> ExibitionArts { get; set; } = new List<ExibitionArt>();

    public virtual Manager? UserActiveNavigation { get; set; }

    public virtual Manager? UserCreateNavigation { get; set; }
}
