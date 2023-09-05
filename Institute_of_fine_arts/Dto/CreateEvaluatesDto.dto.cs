namespace Institute_of_fine_arts.Dto
{
    public class CreateEvaluatesDto : EvaluateDto
    {
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string? ArtSlug { get; set; }
        public int CompetitionId { get; set; }
    }
}
