using System;
using System.Collections.Generic;

namespace Institute_of_fine_arts.Entities;

public partial class Judge
{
    public int Id { get; set; }

    public int? TeacherId { get; set; }

    public int? CompetitionId { get; set; }

    public int? UserCreate { get; set; }

    public int? UserActive { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Competition? Competition { get; set; }

    public virtual User? Teacher { get; set; }

    public virtual Manager? UserActiveNavigation { get; set; }

    public virtual Manager? UserCreateNavigation { get; set; }
}
