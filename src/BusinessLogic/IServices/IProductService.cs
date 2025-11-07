namespace crm.BussinessLogic.IServices;

using crm.DataAccess.Entities;
using crm.DataAccess.Enums;
using crm.BusinessLogic.Dtos;

public interface IProductService
{
    Task<Guid> CreateProductAsync(ProductDto productDto, CancellationToken token = default);

    Task<bool> UpdateProductAsync(ProductDto productDto, CancellationToken token = default);

    Task<bool> DeleteProductAsync(Guid productId, CancellationToken token = default);

    Task<bool> UpdateStockAsync(UpdateStockDto updateStockDto, CancellationToken token = default);

    Task<bool> UpdatePriceAsync(UpdatePriceDto updatePriceDto, CancellationToken token = default);

    Task<ProductDto?> GetByIdAsync(Guid paymentId, CancellationToken token = default);

    Task<IEnumerable<ProductDto>> GetAllAsync(CancellationToken token = default);

    Task<IEnumerable<ProductDto>> SearchAsync(string productName, CancellationToken token = default);

    Task<IEnumerable<ProductDto>> GetByCategoryAsync(ProductCategory productCategory, CancellationToken token = default);

    Task<IEnumerable<ProductDto>> GetAvailableProductsAsync(CancellationToken token = default);

    Task<ProductDto> UpdateDtoDataAsync(Guid productId, CancellationToken token = default);

}