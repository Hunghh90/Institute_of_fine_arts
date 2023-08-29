namespace Institute_of_fine_arts.Dto
{
    public class artDto
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string? Slug { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool IsSell { get; set; }
        public int CompetitionId { get; set; }
    }
}
