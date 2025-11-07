
using crm.DataAccess.Entities;
using crm.DataAccess.IRepositories;
using Moq;
using crm.BusinessLogic.Dtos;
using crm.BusinessLogic.Services;
using crm.DataAccess.DataBase;
using System.IO.Pipelines;

namespace test.BusinessLogicTests.ServicesTests;

public class ProductServiceTests
{
    private readonly Mock<IProductRepository> _productRepositoryMoq;
    private readonly ProductService _productService;

    public ProductServiceTests()
    {
        _productRepositoryMoq = new();
        _productService = new(_productRepositoryMoq.Object);
    }

    [Fact]
    public async Task CreateProductAsync_AddsNewProductToDb()
    {
        // Arrange
        CancellationToken token = CancellationToken.None;
        ProductDto productDto = new();
        _productRepositoryMoq.Setup(r => r.AddAsync(It.IsAny<Product>(), token)).Returns(Task.CompletedTask);

        // Act
        await _productService.CreateProductAsync(productDto, token);

        // Assert
        _productRepositoryMoq.Verify(r => r.AddAsync(It.IsAny<Product>(), token), Times.Once);
    }

    [Fact]
    public async Task UpdateProductAsync_ProductNotFound_ReturnsFalse()
    {
        // Arrange
        CancellationToken token = CancellationToken.None;
        Guid productId = Guid.NewGuid();
        Product product = new() { Id = productId };
        DataStorage.Products.Add(product);
        ProductDto productDto = new() { Id = Guid.NewGuid() };
        _productRepositoryMoq.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), token)).ReturnsAsync((Product?)null);

        // Act
        var result = await _productService.UpdateProductAsync(productDto, token);

        // Assert
        Assert.False(result);
        _productRepositoryMoq.Verify(r => r.GetByIdAsync(It.IsAny<Guid>(), token), Times.Once);
    }

    [Fact]
    public async Task UpdateProductAsync_CorrectProductDto_ReturnsTrue()
    {
        // Arrange
        CancellationToken token = CancellationToken.None;
        Guid productId = Guid.NewGuid();
        Product product = new() { Id = productId, Name = "Choy" };
        DataStorage.Products.Add(product);
        string newName = "Kurutop";
        ProductDto newProductDto = new() { Id = productId, Name = newName };
        _productRepositoryMoq.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), token)).ReturnsAsync(product);
        _productRepositoryMoq.Setup(r => r.UpdateAsync(It.IsAny<Product>(), It.IsAny<Product>(), token)).Returns(Task.CompletedTask);

        // Act
        var result = await _productService.UpdateProductAsync(newProductDto, token);

        // Assert
        Assert.True(result);
        _productRepositoryMoq.Verify(r => r.GetByIdAsync(It.IsAny<Guid>(), token), Times.Once);
        _productRepositoryMoq.Verify(r => r.UpdateAsync(It.IsAny<Product>(), It.IsAny<Product>(), token), Times.Once);
    }

    [Fact]
    public async Task DeleteProductAsyn_ProductNotFound_ReturnsFalse()
    {
        // Arrange
        CancellationToken token = CancellationToken.None;
        Guid productId = Guid.NewGuid();
        Product product = new() { Id = productId };
        DataStorage.Products.Add(product);
        _productRepositoryMoq.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), token)).ReturnsAsync((Product?)null);

        // Act
        var result = await _productService.DeleteProductAsync(productId, token);

        // Assert
        Assert.False(result);
        _productRepositoryMoq.Verify(r => r.GetByIdAsync(It.IsAny<Guid>(), token), Times.Once);
    }

    [Fact]
    public async Task DeleteProductAsyn_CorrectProductId_ReturnsTrue()
    {
        // Arrange
        CancellationToken token = CancellationToken.None;
        Guid productId = Guid.NewGuid();
        Product product = new() { Id = productId };
        DataStorage.Products.Add(product);
        _productRepositoryMoq.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), token)).ReturnsAsync(product);

        // Act
        var result = await _productService.DeleteProductAsync(productId, token);

        // Assert
        Assert.True(result);
        Assert.False(product.IsAvailable);
        _productRepositoryMoq.Verify(r => r.GetByIdAsync(It.IsAny<Guid>(), token), Times.Once);
    }

    [Fact]
    public async Task UpdateStockAsync_PrdoductNotFound_ReturnsFalse()
    {
        // Arrange
        CancellationToken token = CancellationToken.None;
        Guid productId = Guid.NewGuid();
        int productStockQuantity = 100;
        int newProductStockQuantity = 300;
        Product product = new() { Id = productId, StockQuantity = productStockQuantity };
        DataStorage.Products.Add(product);
        UpdateStockDto updateStockDto = new(Guid.NewGuid(), newProductStockQuantity);
        _productRepositoryMoq.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), token)).ReturnsAsync((Product?)null);

        // Act
        var result = await _productService.UpdateStockAsync(updateStockDto, token);

        // Assert
        Assert.False(result);
        _productRepositoryMoq.Verify(r => r.GetByIdAsync(It.IsAny<Guid>(), token), Times.Once);
    }

    [Fact]
    public async Task UpdateStockAsync_CorrectProductId_ReturnsFalse()
    {
        // Arrange
        CancellationToken token = CancellationToken.None;
        Guid productId = Guid.NewGuid();
        int productStockQuantity = 100;
        int newProductStockQuantity = 300;
        Product product = new() { Id = productId, StockQuantity = productStockQuantity };
        DataStorage.Products.Add(product);
        UpdateStockDto updateStockDto = new(productId, newProductStockQuantity);
        _productRepositoryMoq.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), token)).ReturnsAsync(product);

        // Act
        var result = await _productService.UpdateStockAsync(updateStockDto, token);

        // Assert
        Assert.True(result);
        Assert.Equal(newProductStockQuantity, product.StockQuantity);
        _productRepositoryMoq.Verify(r => r.GetByIdAsync(It.IsAny<Guid>(), token), Times.Once);
    }



}