using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Resturant.Domain.Entity;

namespace Resturant.Infrastructure.SeedData;

public class SeedMenuItems : IEntityTypeConfiguration<MenuItems>
{
    public void Configure(EntityTypeBuilder<MenuItems> builder)
    {
        
        // builder.Ignore("ShoppingCartItemsId");
        //
        // builder.HasData(
        //     new MenuItems() { Id = 1, Name = "Egg Roll Platter", Price = 14.95m }, 
        //     new MenuItems() { Id = 2, Name = "Papaya Salad", Price = 8.95m },
        //     new MenuItems() { Id = 3, Name = "Tofu", Price = 10.5m },
        //     new MenuItems() { Id = 4, Name = "Chopped Beef", Price = 12.95m },
        //     new MenuItems() { Id = 5, Name = "Veggie Platter", Price = 8.95m }
        // ); 
        
    }
}