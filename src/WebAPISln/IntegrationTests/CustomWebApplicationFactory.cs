using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntegrationTests
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TStartup"></typeparam>
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ScreamBackend.DB.ScreamDB>));
                services.Remove(descriptor);

                services.AddDbContext<ScreamBackend.DB.ScreamDB>(options =>
                {
                    options.UseSqlite("Filename:memory:");
                });

                var sp = services.BuildServiceProvider();

                using var scope = sp.CreateScope();
                var scopedServices = scope.ServiceProvider;

                var db = scopedServices.GetRequiredService<ScreamBackend.DB.ScreamDB>();
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                try
                {
                    new InitializeDb(db).Init();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            });
        }
    }
}
