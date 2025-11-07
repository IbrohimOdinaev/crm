namespace crm.BusinessLogic.Dtos;

public sealed record UpdateStockDto(Guid ProductId, int NewQuantity);