
using crm.DataAccess.DataBase;
using crm.DataAccess.Entities;
using crm.DataAccess.Enums;

namespace crm.DataAccess.IRepositories;

public interface IProductRepository
{
    Task AddAsync(Product product, CancellationToken token = default);

    Task<Product?> GetByIdAsync(Guid productId, CancellationToken token = default);

    Task<IEnumerable<Product>> GetByCategoryAsync(ProductCategory productCategory, CancellationToken token = default);

    Task<IEnumerable<Product>> GetAllAsync(CancellationToken token = default);

    Task<IEnumerable<Product>> SearchAsync(string name, CancellationToken token = default);

    Task<IEnumerable<Product>> GetAvailableProductsAsync(CancellationToken token = default);

    Task UpdateAsync(Product product, Product newProduct, CancellationToken token = default);
}