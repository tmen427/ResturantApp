using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Resturant.Domain.Entity;

namespace Resturant.Infrastructure.SeedData;

public class SeedMenuItems : IEntityTypeConfiguration<MenuItem>
{
    
    public void Configure(EntityTypeBuilder<MenuItem> builder)
    {
        builder.HasData(
            new MenuItem() { Id = 1, MenuItemName = "Egg Roll Platter", MenuItemBasePrice = 14.95m, MenuItemDescription = "Carrots, Tomato, Lettuce", MenuItemImageUrl = "EggRollPlatter.png"},
            new MenuItem() { Id = 2, MenuItemName = "Papaya Salad", MenuItemBasePrice = 8.95m,   MenuItemDescription = "Green Papaya, snake beans, cherry tomato", MenuItemImageUrl = "PapayaSalad.png"},
            new MenuItem() { Id = 3, MenuItemName = "Tofu Stir Fry", MenuItemBasePrice = 10.95m, MenuItemDescription = "Tofu, bell peppers, green onion", MenuItemImageUrl = "TofuStirFry.png"},
            new MenuItem() { Id = 4, MenuItemName = "Chopped Beef", MenuItemBasePrice = 12.95m, MenuItemDescription = "Mushrooms, Carrots, Okra", MenuItemImageUrl =  "ChoppedBeef.png"},
            new MenuItem() { Id = 5, MenuItemName = "Veggie Platter", MenuItemBasePrice = 9.95m, MenuItemDescription = "Edamame, Carrots, Avocado", MenuItemImageUrl = "VeggiePlatter.png"},
            new MenuItem() { Id = 6, MenuItemName = "Caesar Salad", MenuItemBasePrice = 10.5m, MenuItemDescription = "Onion, lettuce, cheese", MenuItemImageUrl = "CaesarSalad.png" }
        );

    }
    
    
    //TODO come back to add base images in the backend instead of from the frontend in the future 
    private string CreateImageUrlPath(string menuItemName)
    {
      //  string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
      //UNSURE IF THIS PATH WILL WORK IN THE BUILD 
        string baseDirectory =  Directory.GetCurrentDirectory();
        string relativePath = $"menuImages/{menuItemName}.png";
        string fullPath = Path.Combine(baseDirectory, relativePath);
        return fullPath;
    }
    
}