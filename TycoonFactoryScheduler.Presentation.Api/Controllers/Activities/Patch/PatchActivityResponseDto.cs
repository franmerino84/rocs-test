namespace TycoonFactoryScheduler.Presentation.Api.Controllers.Activities.Patch
{
    public class PatchActivityResponseDto
    {
        public int Id { get; set; }
        public string Description { get; set; } = default!;
        public string ActivityType { get; set; } = default!;
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public List<char> Workers { get; set; } = default!;
    }
}