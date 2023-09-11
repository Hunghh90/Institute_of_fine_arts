using System;
using System.Collections.Generic;

namespace Institute_of_fine_arts.Entities;

public partial class Art
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Slug { get; set; }

    public string Description { get; set; } = null!;

    public bool? IsSell { get; set; }

    public bool? IsSold { get; set; }

    public bool? IsExibition { get; set; }

    public decimal? Price { get; set; }

    public string? Url { get; set; }

    public string? Path { get; set; }

    public int OwnerId { get; set; }

    public int CompetitionId { get; set; }

    public int? PrizeId { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public byte? Granded { get; set; }

    public decimal? TotalScore { get; set; }

    public virtual Competition Competition { get; set; } = null!;

    public virtual ICollection<Evaluate> Evaluates { get; set; } = new List<Evaluate>();

    public virtual ICollection<ExibitionArt> ExibitionArts { get; set; } = new List<ExibitionArt>();

    public virtual User Owner { get; set; } = null!;

    public virtual Prize? Prize { get; set; }
}
