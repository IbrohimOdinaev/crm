namespace crm.BusinessLogic.Dtos;

public record UpdatePriceDto(Guid ProductId, decimal NewPrice);