using System;
using System.Collections.Generic;

namespace Institute_of_fine_arts.Entities;

public partial class Evaluate
{
    public int Id { get; set; }

    public string Feedback { get; set; } = null!;

    public int Layout { get; set; }

    public int Color { get; set; }

    public int Content { get; set; }

    public int Creative { get; set; }

    public decimal? Total { get; set; }

    public int? CompetitionId { get; set; }

    public int? UserId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? Status { get; set; }

    public virtual Competition? Competition { get; set; }

    public virtual User? User { get; set; }
}
