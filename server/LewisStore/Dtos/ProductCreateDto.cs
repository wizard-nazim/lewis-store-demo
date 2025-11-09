namespace LewisStore.Dtos;

public record ProductCreateDto(string Name, string SKU, decimal UnitPrice, int StockQty);
