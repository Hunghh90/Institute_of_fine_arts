using System;
using System.Collections.Generic;

namespace Institute_of_fine_arts.Entities;

public partial class Art
{
    public int Id { get; set; }

    public string Description { get; set; } = null!;

    public decimal? Price { get; set; }

    public bool? IsSell { get; set; }

    public bool? IsSold { get; set; }

    public bool? IsExibition { get; set; }

    public int? UserId { get; set; }

    public int? CompetitionId { get; set; }

    public int? PrizeId { get; set; }

    public int? Favorite { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? Status { get; set; }

    public virtual Competition? Competition { get; set; }

    public virtual Prize? Prize { get; set; }

    public virtual User? User { get; set; }
}
