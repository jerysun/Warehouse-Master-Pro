using System.ComponentModel.DataAnnotations;

namespace Core.Models;
public class Product
{
    [Required]
    public string Name { get; init; } = string.Empty;
    public int QuantityInStock { get; private set; } = 0;
    [Key]
    public int Id { get; init; }

    public Product(string name, int quantityInStock)
    {
        Name = name;
        QuantityInStock = quantityInStock;
    }

    public void IncreaseStock(int amount)
    {
        if (amount == 0) return;
        if (amount < 0) throw new NegativeValueException(amount);
        QuantityInStock += amount;
    }

    public void DecreaseStock(int amount)
    {
        if (amount == 0) return;
        if (amount < 0) throw new NegativeValueException(amount);
        if (amount > QuantityInStock) throw new NotEnoughStockException(QuantityInStock, amount);
        QuantityInStock -= amount;
    }
}
