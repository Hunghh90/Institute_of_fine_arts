using System;
using System.Collections.Generic;

namespace Institute_of_fine_arts.Entities;

public partial class Exibition
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Theme { get; set; }

    public string? Description { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? Status { get; set; }

    public string Slug { get; set; } = null!;

    public int? UserId { get; set; }

    public virtual User? User { get; set; }
}
