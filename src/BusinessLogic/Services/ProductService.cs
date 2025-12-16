using crm.BusinessLogic.Dtos;
using crm.BusinessLogic.Helpers.Extensions;
using crm.DataAccess.DataBase;
using crm.DataAccess.Entities;
using crm.DataAccess.Enums;
using crm.DataAccess.IRepositories;
using crm.BusinessLogic.IServices;
using crm.BussinessLogic.IServices;

namespace crm.BusinessLogic.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Guid> CreateProductAsync(ProductDto productDto, CancellationToken token)
    {
        await _productRepository.AddAsync(productDto.ToDbProduct(), token);

        return productDto.Id;
    }
    public async Task<bool> UpdateProductAsync(ProductDto productDto, CancellationToken token)
    {
        Product? product = await _productRepository.GetByIdAsync(productDto.Id, token);

        if (product is null) return false;

        await _productRepository.UpdateAsync(product, productDto.ToDbProduct());

        return true;
    }

    public async Task<bool> DeleteProductAsync(Guid productId, CancellationToken token)
    {
        Product? product = await _productRepository.GetByIdAsync(productId, token);

        if (product is null) return false;

        product.IsAvailable = false;

        return true;
    }

    public async Task<bool> UpdateStockAsync(UpdateStockDto updateStockDto, CancellationToken token)
    {
        Product? product = await _productRepository.GetByIdAsync(updateStockDto.ProductId, token);

        if (product is null) return false;

        product.StockQuantity = updateStockDto.NewQuantity;

        return true;
    }

    public async Task<bool> UpdatePriceAsync(UpdatePriceDto updatePriceDto, CancellationToken token)
    {
        Product? product = await _productRepository.GetByIdAsync(updatePriceDto.ProductId, token);

        if (product is null) return false;

        product.Price = updatePriceDto.NewPrice;

        return true;
    }

    public async Task<ProductDto?> GetByIdAsync(Guid productId, CancellationToken token)
    {
        Product? product = await _productRepository.GetByIdAsync(productId);

        return product == null ? null : product.ToProductDto();
    }

    public async Task<IEnumerable<ProductDto>> GetAllAsync(CancellationToken token)
        => (await _productRepository.GetAllAsync()).Select(product => product.ToProductDto());

    public async Task<IEnumerable<ProductDto>> SearchAsync(string productName, CancellationToken token)
        => (await _productRepository.SearchAsync(productName)).Select(product => product.ToProductDto());

    public async Task<IEnumerable<ProductDto>> GetByCategoryAsync(ProductCategory productCategory, CancellationToken token)
        => (await _productRepository.GetByCategoryAsync(productCategory)).Select(product => product.ToProductDto());

    public async Task<IEnumerable<ProductDto>> GetAvailableProductsAsync(CancellationToken token)
        => (await _productRepository.GetAvailableProductsAsync()).Select(product => product.ToProductDto());

    public async Task<ProductDto> UpdateDtoDataAsync(Guid productId, CancellationToken token)
        => (await _productRepository.GetByIdAsync(productId))!.ToProductDto();
}