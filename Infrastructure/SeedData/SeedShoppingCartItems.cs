using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Resturant.Domain.Entity;

namespace Resturant.Infrastructure.SeedData;

public class SeedShoppingCartItems : IEntityTypeConfiguration<ShoppingCartItems>
{
    public void Configure(EntityTypeBuilder<ShoppingCartItems> builder)
    {
        builder.HasData(
            new ShoppingCartItems()
            {
                Id = 1,
                Identity = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"),
                Created =  DateTime.UtcNow.ToUniversalTime(),
                TotalPrice = 0, 
                CustomerInformationId = null
            });
    }
}