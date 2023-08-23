using System;
using System.Collections.Generic;

namespace Institute_of_fine_arts.Entities;

public partial class Customer
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Address { get; set; }

    public string? Telephone { get; set; }
}
