﻿using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Institute_of_fine_arts.Dto
{
    public class competitionDto
    {
        public string? Name { get; set; }
        public string? Slug { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Theme { get; set; }
        public string? Description { get; set; }
    }
}
