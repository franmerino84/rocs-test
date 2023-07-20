using TycoonFactoryScheduler.Domain.Entities.Activities;

namespace TycoonFactoryScheduler.Application.Handlers.Activities.Commands.Post
{
    public class PostActivityCommandResponse
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public ActivityType ActivityType { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public List<char> Workers { get; set; } = new List<char>();
    }
}