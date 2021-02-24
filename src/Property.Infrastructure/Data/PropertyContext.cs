using System;
using Microsoft.EntityFrameworkCore;
using Property.Infrastructure.Data.EntityConfiguration;

namespace Property.Infrastructure.Data
{
    using MediatR;
    using Property.Core;
    using System.Configuration;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class PropertyContext:DbContext
    {

        public DbSet<Property> Properties { get; set; }
        private readonly IMediator mediator;

        public PropertyContext()
        {

        }

        public PropertyContext(DbContextOptions<PropertyContext> options) : base(options) { }
        public PropertyContext(DbContextOptions<PropertyContext> options, IMediator mediator) : base(options)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //if the options haven't been configuration then default to the following.
            if (!optionsBuilder.IsConfigured)
            {
                //var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                string connectionString = "Server=.\\;Database=PropertyDB;User ID=sa;Password=Pass@word;";
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PropertyTypeConfig());
            //base.OnModelCreating(modelBuilder);
            //Add-Migration InitialCreate -StartupProject Property.Infrastructure -Context PropertyContext -OutputDir "Data/Migrations" -Namespace Data.Migrations
            //remove-migration -startupproject Property.Infrastructure
            //Script-Migration -StartupProject Property.Infrastructure -From 0 -To InitialCreate -Project Property.Infrastructure 
        }

        public async Task<bool> SaveEntityBaseAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            // Dispatch Domain Events collection. 
            // Choices:
            // A) Right BEFORE committing data (EF SaveChanges) into the DB will make a single transaction including  
            // side effects from the domain event handlers which are using the same DbContext with "InstancePerLifetimeScope" or "scoped" lifetime
            // B) Right AFTER committing data (EF SaveChanges) into the DB will make multiple transactions. 
            // You will need to handle eventual consistency and compensatory actions in case of failures in any of the Handlers. 

            //Get the list of entities with events
            var domainEntities = this.ChangeTracker
                .Entries<EntityBase>()
                .Where(x => x.Entity.Events.Any());

            //From the entities get a list of events we need to publish
            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.Events)
                .ToList();

            //clear the events from the entities before publishing to prevent duplication of events
            domainEntities.ToList()
                .ForEach(entity => entity.Entity.Events.Clear());

            foreach (var domainEvent in domainEvents)
            {
                await mediator.Publish(domainEvent);
            }

            var result = await base.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
