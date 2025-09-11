using Microsoft.EntityFrameworkCore;

using System.Reflection.Metadata;

using Resturant.Domain.Entity;
using Resturant.Domain.DomainEvents;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using MediatR;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Resturant.Application.DomainEventHandler;
using Resturant.Domain.EventSourcing;
using Resturant.Infrastructure.SeedData;
using Microsoft.AspNetCore.Identity;


namespace Resturant.Infrastructure.Context
{
    public class RestaurantContext : IdentityDbContext<WebUser>
    {
        // public ToDoContext(DbContextOptions<ToDoContext> options, IMediator mediator) : base(options)
        // {
        //     _mediator = mediator ?? throw new ArgumentNullException("nonononon" + nameof(mediator));
        //  //   _mediator = mediator; 
        // }
        
   
        
        public RestaurantContext(DbContextOptions<RestaurantContext> options) : base(options)
        {
   
        }
        
        //this is needed for unit testing a parameterless constructer
        public RestaurantContext()
        {
        }

      //  private readonly IMediator _mediator;

        public DbSet<CustomerPaymentInformation> CustomerPaymentInformation { get; set; }
        
        //   public DbSet<DomainEvent> DomainEvents { get; set; }    
        public DbSet<Event> Events { get; set; } 
        public DbSet<ShoppingCartItems> ShoppingCartItems { get; set; }
        public DbSet<MenuItems> MenuItems { get; set; }
        public DbSet<CustomerInquiryInformation> CustomerInquiryInformation { get; set; }
        public DbSet<BookingInformation> BookingInformation { get; set; }
        
     //   public DbSet<WebUser> WebUser { get; set; }
        
        
        // public override async Task<int> SaveChangesAsync(CancellationToken cancellation = default)
        // {
        //     // track changes in List<INotification> DomainEvents 
        //     var domainEvents = ChangeTracker.Entries<Aggregateroot>()
        //                        .Where(x => x.Entity.DomainEvents != null)
        //                        .SelectMany(x => x.Entity.DomainEvents)
        //                        .ToList();
        
        //     foreach (var domainEvent in domainEvents)
        //     {
        //         Console.WriteLine("the domian event should work here!!!");
        //         Console.WriteLine(domainEvent);
        //         //this is the key right here !!!!, any handlers that have this event key will pick up the events!
        //         //some devs convert to object that they want here, whereas some devleopers will send reuults to handlers 
        //         if (domainEvent is PriceUpdateDomainEvent)
        //         {
        //             Console.WriteLine("hey this is a priceupdatedomianevent");
        //         }
        //         else
        //         {
        //             Console.WriteLine(domainEvent.GetType().Name);
        //
        //         }
        //
        //
        //         //maybe the domainhandler can't pick the event up since it is in a different assembly???
        //      //   await _mediator.Publish(domainEvent);
        //     }
        //     //when to you save changes....
        //     return await base.SaveChangesAsync();
        //     return 1; 
        // }

        
        //change logging for entitiy framework 
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
         //   optionsBuilder.EnableSensitiveDataLogging();
          //  optionsBuilder.LogTo(Console.WriteLine);    
          
          optionsBuilder.ConfigureWarnings(x => x.Ignore(RelationalEventId.PendingModelChangesWarning));
        }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
             base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ShoppingCartItems>()
                .HasMany(e => e.MenuItems)
                .WithOne(e => e.ShoppingCartItems)
                .HasForeignKey(e => e.ShoppingCartItemsIdentity)
                .HasPrincipalKey(e => e.Identity);

     
             modelBuilder.ApplyConfiguration(new SeedShoppingCartItems()); 
             modelBuilder.ApplyConfiguration(new SeedMenuItems()); 
        }
        
    }
}
