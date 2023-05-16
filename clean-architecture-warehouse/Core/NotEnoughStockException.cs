﻿namespace Core;

public class NotEnoughStockException : Exception
{
    public NotEnoughStockException(int quantityInStock, int amountToRemove)
        : base($"You cannot remove {amountToRemove} item(s) when there are only {quantityInStock} item(s) in stock!")
    {
        QuantityInStock = quantityInStock;
        AmountToRemove = amountToRemove;
    }

    public int QuantityInStock { get; }
    public int AmountToRemove { get; }
}
