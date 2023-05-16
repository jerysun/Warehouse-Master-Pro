namespace Core;

public class ProductNotFoundException : Exception
{
    public ProductNotFoundException(int productId)
        : base($"The item with id = '{productId}' was not found.")
    {
        ProductId = productId;
    }

    public int ProductId { get; }
}
