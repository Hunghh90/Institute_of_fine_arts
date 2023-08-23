using System;
using System.Collections.Generic;

namespace Institute_of_fine_arts.Entities;

public partial class ExibitionArt
{
    public int Id { get; set; }

    public int? ExibitionId { get; set; }

    public int? ArtId { get; set; }

    public int? UserCreate { get; set; }

    public int? UserActive { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Art? Art { get; set; }

    public virtual Exibition? Exibition { get; set; }

    public virtual Manager? UserActiveNavigation { get; set; }

    public virtual Manager? UserCreateNavigation { get; set; }
}
