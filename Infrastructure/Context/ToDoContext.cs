using Microsoft.EntityFrameworkCore;

using System.Reflection.Metadata;

using Resturant.Domain.Entity;
using Resturant.Domain.DomainEvents;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using MediatR;
using Resturant.Application.DomainEventHandler;
using Resturant.Domain.EventSourcing;



namespace Resturant.Infrastructure.Context
{
    public class ToDoContext : DbContext
    {
        // public ToDoContext(DbContextOptions<ToDoContext> options, IMediator mediator) : base(options)
        // {
        //     _mediator = mediator ?? throw new ArgumentNullException("nonononon" + nameof(mediator));
        //  //   _mediator = mediator; 
        // }
        
        public ToDoContext(DbContextOptions<ToDoContext> options) : base(options)
        {
   
        }
        
        //this is needed for unit testing a parameterless constructer
        public ToDoContext()
        {
        }

      //  private readonly IMediator _mediator;
        public DbSet<User> Users { get; set; }
        public DbSet<OrderInformation> OrderInformation { get; set; }
        //   public DbSet<DomainEvent> DomainEvents { get; set; }    
        public DbSet<Event> Events { get; set; } 
        public DbSet<TemporaryCartItems> TemporaryCartItems { get; set; }
        public DbSet<MenuItemsVO> MenuItems { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<BookingInformation> BookingInformation { get; set; }
        public DbSet<UserInformation> UserInformation { get; set; }




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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
         //   optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.LogTo(Console.WriteLine);    
        }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(e => e.UserInformation)
                .WithOne(e => e.User)
                .HasForeignKey<UserInformation>(e => e.UserId)
                .IsRequired();
            
          //  modelBuilder.Entity<TemporaryCartItems>().HasMany(x => x.MenuItems);

            modelBuilder.Entity<MenuItemsVO>().HasOne(p => p.TemporaryCartItems).WithMany(p => p.MenuItems)
                .HasForeignKey(p => p.TemporaryCartItemsIndentity).HasPrincipalKey(x=>x.Indentity);
          
             
      
        }




    }
}
