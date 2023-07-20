using Microsoft.AspNetCore.Mvc;
using TycoonFactoryScheduler.Infrastructure.Uri;

namespace TycoonFactoryScheduler.Presentation.Api.Controllers.Activities
{
    public class CreatedUriBuilder : ICreatedUriBuilder
    {
        public string Build(ControllerBase controller, object id) => 
            $"{controller.Request.Scheme}://{controller.Request.Host.Value}{controller.Request.Path.Value}/{id}";
    }
}
