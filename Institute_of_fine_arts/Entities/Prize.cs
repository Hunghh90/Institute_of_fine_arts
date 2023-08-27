using System;
using System.Collections.Generic;

namespace Institute_of_fine_arts.Entities;

public partial class Prize
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Slug { get; set; } = null!;

    public int Quantity { get; set; }

    public int? ConpetitionId { get; set; }

    public string? Status { get; set; }

    public int? UserCreate { get; set; }

    public int? UserActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public decimal? Price { get; set; }

    public virtual ICollection<Art> Arts { get; set; } = new List<Art>();

    public virtual Competition? Conpetition { get; set; }

    public virtual Manager? UserActiveNavigation { get; set; }

    public virtual Manager? UserCreateNavigation { get; set; }
}
