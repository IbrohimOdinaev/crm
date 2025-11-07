
using crm.DataAccess.IRepositories;
using crm.DataAccess.DataBase;
using crm.DataAccess.Entities;
using crm.DataAccess.Enums;

namespace crm.DataAccess.Repositories;


public sealed class ProductRepository : IProductRepository
{
    public Task AddAsync(Product product, CancellationToken token)
    {
        DataStorage.Products.Add(product);

        return Task.CompletedTask;
    }

    public Task<Product?> GetByIdAsync(Guid productId, CancellationToken token)
        => Task.FromResult(DataStorage.Products.FirstOrDefault(product => product.Id == productId));

    public Task<IEnumerable<Product>> GetAllAsync(CancellationToken token)
        => Task.FromResult((IEnumerable<Product>)DataStorage.Products);

    public Task<IEnumerable<Product>> SearchAsync(string productName, CancellationToken token)
        => Task.FromResult((IEnumerable<Product>)DataStorage.Products.Where(product => product.Name == productName));

    public Task<IEnumerable<Product>> GetByCategoryAsync(ProductCategory productCategory, CancellationToken token)
        => Task.FromResult((IEnumerable<Product>)DataStorage.Products.Where(product => product.Category == productCategory));

    public Task<IEnumerable<Product>> GetAvailableProductsAsync(CancellationToken token)
        => Task.FromResult((IEnumerable<Product>)DataStorage.Products.Where(product => product.IsAvailable == true).ToList());

    public Task UpdateAsync(Product product, Product newProduct, CancellationToken token)
    {
        product.Category = newProduct.Category;
        product.Description = newProduct.Description;
        product.IsAvailable = newProduct.IsAvailable;
        product.Name = newProduct.Name;
        product.StockQuantity = newProduct.StockQuantity;
        product.UpdatedAt = newProduct.UpdatedAt;

        return Task.CompletedTask;
    }
}