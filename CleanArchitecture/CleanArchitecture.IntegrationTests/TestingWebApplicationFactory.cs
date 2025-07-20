using CleanArchitecture.ApplicationCore.Entities;
using CleanArchitecture.Infrastructure.Databases;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.IntegrationTests
{
    public class TestingWebApplicationFactory<TEntryPoint> : WebApplicationFactory<Program> where TEntryPoint : Program
    {
        public const string Akb48= "AKB48";
        public Guid Akb48Id { get; } = Guid.Parse("618577CE-3C7B-4244-BC04-8AEFC2ED0019");
        public Guid TestUpdateId { get; } = Guid.NewGuid();
        public Guid TestDeleteId { get; } = Guid.NewGuid();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IDbContextOptionsConfiguration<ApplicationContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<ApplicationContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryCleanArchitectureTest");
                });

                ServiceProvider sp = services.BuildServiceProvider();
                using IServiceScope scope = sp.CreateScope();
                using ApplicationContext appContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
                appContext.Database.EnsureCreated();

                // Seed the database with test data.
                appContext.Artists.AddRange(
                    new Artist { ArtistId = Akb48Id, Name = Akb48, ActiveFrom=new DateTime(2005,12,8) },
                    new Artist { ArtistId = Guid.NewGuid(), Name = "NMB48", ActiveFrom = new DateTime(2010, 10, 9) },
                    new Artist { ArtistId = Guid.NewGuid(), Name = "HKT48", ActiveFrom = new DateTime(2011, 10, 23) },
                    new Artist { ArtistId = Guid.NewGuid(), Name = "SKE48", ActiveFrom = new DateTime(2008, 10, 5) },
                    new Artist { ArtistId = Guid.NewGuid(), Name = "NGT48", ActiveFrom = new DateTime(2015, 1, 10) },
                    new Artist { ArtistId = Guid.NewGuid(), Name = "STU48", ActiveFrom = new DateTime(2017, 3, 20) },
                    new Artist { ArtistId = TestUpdateId, Name = "Test Update Artist", ActiveFrom = DateTime.UtcNow },
                    new Artist { ArtistId = TestDeleteId, Name = "Test Delete Artist", ActiveFrom = DateTime.UtcNow }
                );

                appContext.SaveChanges();
            });
        }
    }
}
