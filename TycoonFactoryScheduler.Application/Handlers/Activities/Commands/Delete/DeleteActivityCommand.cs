using MediatR;

namespace TycoonFactoryScheduler.Application.Handlers.Activities.Commands.Delete
{
    public class DeleteActivityCommand : IRequest
    {
        public DeleteActivityCommand(int id)
        {
            Id = id;
        }

        public int Id { get; }
    }
}