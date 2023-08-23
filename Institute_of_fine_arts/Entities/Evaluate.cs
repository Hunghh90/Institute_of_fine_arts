using System;
using System.Collections.Generic;

namespace Institute_of_fine_arts.Entities;

public partial class Evaluate
{
    public int Id { get; set; }

    public string? Feedback { get; set; }

    public int Layout { get; set; }

    public int Color { get; set; }

    public int Content { get; set; }

    public int Creative { get; set; }

    public int Total { get; set; }

    public string? Status { get; set; }

    public int? ArtsId { get; set; }

    public int? TeacherId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Art? Arts { get; set; }

    public virtual User? Teacher { get; set; }
}
