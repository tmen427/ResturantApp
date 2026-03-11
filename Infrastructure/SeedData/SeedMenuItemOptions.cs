using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Resturant.Domain.Entity;

namespace Resturant.Infrastructure.SeedData;

public class SeedMenuItemOptions :IEntityTypeConfiguration<MenuItemOption>

{
    public void Configure(EntityTypeBuilder<MenuItemOption> builder)
    {
        builder.HasData(
          
            new MenuItemOption() { Id = 1, MenuOptionName = "Extra Chicken", MenuOptionPrice = 1m},
            new MenuItemOption() { Id = 2, MenuOptionName = "Extra Beef", MenuOptionPrice = 1m},
            new MenuItemOption() { Id = 3, MenuOptionName = "Extra Pork", MenuOptionPrice = 1m}, 
            new MenuItemOption() { Id = 4, MenuOptionName = "Extra Tofu", MenuOptionPrice = 1m},
            new MenuItemOption() { Id = 5, MenuOptionName = "Soy Sauce", MenuOptionPrice = .25m}
        ); 
        
    }
}
