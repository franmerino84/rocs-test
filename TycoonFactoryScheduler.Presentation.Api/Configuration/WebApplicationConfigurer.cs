using Microsoft.EntityFrameworkCore;
using TycoonFactoryScheduler.Infrastructure.Persistence.EF;

namespace TycoonFactoryScheduler.Presentation.Api.Configuration
{
    public static class WebApplicationConfigurer
    {
        public static WebApplication GetConfiguredWebApplication(string[] args) => 
            GetConfiguredWebApplication(WebApplicationBuilderConfigurer.GetConfiguredWebApplicationBuilder(args));

        public static WebApplication GetConfiguredWebApplication(WebApplicationBuilder builder)
        {
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider
                    .GetRequiredService<TycoonFactorySchedulerContext>();

                dbContext.Database.Migrate();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            return app;
        }

    }
}
