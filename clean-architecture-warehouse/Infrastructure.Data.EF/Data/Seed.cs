using Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Infrastructure.Data.EF.Data;
public class Seed
{
    public static async Task SeedProductsAsync(ProductContext context)
    {
        if (await context.Products.AnyAsync())
        {
            return;
        }

        var currentAppPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
        var currentAppDir = Path.GetDirectoryName(currentAppPath);
        var productDataPath = Path.Combine(currentAppDir!, "Data/ProductData.json");
        var productData = await File.ReadAllTextAsync(productDataPath);
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var products = JsonSerializer.Deserialize<List<Product>>(productData, options);

        await context.Products.AddRangeAsync(products!);
        await context.SaveChangesAsync();
    }
}
