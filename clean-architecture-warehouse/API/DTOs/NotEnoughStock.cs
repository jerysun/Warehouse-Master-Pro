namespace API.DTOs;

public record class NotEnoughStock(int AmountToRemove, int QuantityInStock, string Message);
