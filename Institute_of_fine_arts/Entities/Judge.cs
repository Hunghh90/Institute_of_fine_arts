using System;
using System.Collections.Generic;

namespace Institute_of_fine_arts.Entities;

public partial class Judge
{
    public int Id { get; set; }

    public int? TeacherId1 { get; set; }

    public int? TeacherId2 { get; set; }

    public int? TeacherId3 { get; set; }

    public int? TeacherId4 { get; set; }

    public int? UserCreater { get; set; }

    public int? UserActive { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? CompetitionId { get; set; }

    public virtual Competition? Competition { get; set; }

    public virtual User? TeacherId1Navigation { get; set; }

    public virtual User? TeacherId2Navigation { get; set; }

    public virtual User? TeacherId3Navigation { get; set; }

    public virtual User? TeacherId4Navigation { get; set; }

    public virtual Manager? UserActiveNavigation { get; set; }

    public virtual Manager? UserCreaterNavigation { get; set; }
}
