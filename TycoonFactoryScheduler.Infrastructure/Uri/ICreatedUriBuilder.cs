using Microsoft.AspNetCore.Mvc;

namespace TycoonFactoryScheduler.Infrastructure.Uri
{
    public interface ICreatedUriBuilder
    {
        string Build(ControllerBase controller, object id);
    }
}