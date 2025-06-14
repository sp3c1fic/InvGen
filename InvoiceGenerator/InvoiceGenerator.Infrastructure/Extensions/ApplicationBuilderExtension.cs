using Microsoft.EntityFrameworkCore;

namespace InvoiceGenerator.InvoiceGenerator.Infrastructure.Extensions
{
    public static class ApplicationBuilderExtension
    {
        public static async Task<IApplicationBuilder> PrepareDatabase(this ApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var services = serviceScope?.ServiceProvider;
            MigrateDatabase(services!);
            return app;
        }

        private static void MigrateDatabase(IServiceProvider services)
        {
            var data = services.GetRequiredService<InvoiceGeneratorDbContext>();
            data.Database.Migrate();
        }
    }
}
