
using crm.DataAccess.DataBase;
using crm.DataAccess.Entities;
using crm.DataAccess.Enums;
using crm.DataAccess.IRepositories;
using crm.DataAccess.Repositories;
using static test.DataAccessTests.Helpers.EntityHelper;

namespace test.DataAccessTests.RepositoriesTests;

public class ProductRepositoryTests
{
    IProductRepository _productRepository = new ProductRepository();

    [Fact]
    public async Task AddAsync_AddsProductToDb()
    {
        //Arrange
        Product product = (Product)CreateDefaultEntity<Product>();

        //Act
        await _productRepository.AddAsync(product);

        //Assert
        Assert.Contains(product, DataStorage.Products);
    }

    [Fact]
    public async Task GetByIdAsync_CorrectId_ReturnsCorrectProduct()
    {
        //Arrange
        Product product = (Product)CreateDefaultEntity<Product>();
        DataStorage.Products.Add(product);

        //Act 
        var result = await _productRepository.GetByIdAsync(product.Id);

        //Assert
        Assert.Equal(product, result);
    }

    [Fact]
    public async Task GetByIdAsync_UnCorrecId_ReturnsCorrectProduct()
    {
        //Arrange
        Product product = (Product)CreateDefaultEntity<Product>();
        DataStorage.Products.Add(product);

        //Act 
        var result = await _productRepository.GetByIdAsync(Guid.NewGuid());

        //Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllProducts()
    {
        //Arrange
        DataStorage.Products.Clear();
        List<Product> products = new();
        for (int i = 0; i < 10; i++)
        {
            products.Add((Product)CreateDefaultEntity<Product>());
        }
        DataStorage.Products.AddRange(products);

        //Act
        var result = await _productRepository.GetAllAsync();

        //Assert
        Assert.Equal(products, result);
    }

    [Theory]
    [InlineData("Potato", "Potato", true)]
    [InlineData("Tomato", "Onion", false)]
    public async Task SearchAsync(string productName, string searchProduct, bool searchResult)
    {
        //Arrange
        DataStorage.Products.Clear();
        for (int i = 0; i < 10; i++)
        {
            DataStorage.Products.Add((Product)CreateDefaultEntity<Product>());
        }
        Product targetProduct = new Product() { Id = Guid.NewGuid(), Name = productName };
        DataStorage.Products.Add(targetProduct);

        //Act
        var result = await _productRepository.SearchAsync(searchProduct);

        //Assert
        if (searchResult)
        {
            Assert.Equal(targetProduct, result.First());
        }
        else
        {
            Assert.Empty(result);
        }
    }

    [Theory]
    [InlineData(ProductCategory.Beauty, 4)]
    [InlineData(ProductCategory.Clothing, 10)]
    [InlineData(ProductCategory.Food, 13)]
    public async Task GetbyCategoryAsync_ReturnsListProducts(ProductCategory category, int targetCategoryCount)
    {
        //Arrange
        DataStorage.Products.Clear();
        List<Product> targetCategoryProducts = new();
        for (int i = 0; i < targetCategoryCount; i++)
        {
            targetCategoryProducts.Add(new Product() { Id = Guid.NewGuid(), Category = category });
            DataStorage.Products.Add((Product)CreateDefaultEntity<Product>());
        }
        DataStorage.Products.AddRange(targetCategoryProducts);

        //Act
        var result = await _productRepository.GetByCategoryAsync(category);

        //Assert
        Assert.Equal(targetCategoryProducts, result);
    }

    [Fact]
    public async Task GetAvailableProductsAsync_ReturnsAvailableProducts()
    {
        //Arrange
        DataStorage.Products.Clear();
        List<Product> products = new()
        {
            new() {Id = Guid.NewGuid(), IsAvailable = true },
            new() {Id = Guid.NewGuid(), IsAvailable = true },
            new() {Id = Guid.NewGuid(), IsAvailable = true },
            new() {Id = Guid.NewGuid(), IsAvailable = false },
            new() {Id = Guid.NewGuid(), IsAvailable = true },
            new() {Id = Guid.NewGuid(), IsAvailable = false },
            new() {Id = Guid.NewGuid(), IsAvailable = true },
            new() {Id = Guid.NewGuid(), IsAvailable = false }
        };
        DataStorage.Products.AddRange(products);

        //Act
        var expected = products.Where(product => product.IsAvailable == true);
        var result = await _productRepository.GetAvailableProductsAsync();

        //Assert
        Assert.Equal(expected, result);
    }
    
    

}