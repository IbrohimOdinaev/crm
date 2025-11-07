using crm.BusinessLogic.Services;
using crm.DataAccess.Entities;
using crm.DataAccess.IRepositories;
using crm.BusinessLogic.Dtos;
using Moq;
using crm.DataAccess.Enums;
using System.ComponentModel.DataAnnotations;

namespace test.BusinessLogicTests.ServicesTests;

public class OrderServiceTests
{
    private readonly Mock<IOrderRepository> _orderRepositoryMoq;
    private readonly Mock<IProductRepository> _productRepositoryMoq;
    private readonly OrderService _orderService;


    public OrderServiceTests()
    {
        _orderRepositoryMoq = new();
        _productRepositoryMoq = new();
        _orderService = new(_orderRepositoryMoq.Object, _productRepositoryMoq.Object);
    }

    [Fact]
    public async Task CreateOrderAsync_AddsNewOrderToDb()
    {
        // Arrange
        CancellationToken token = CancellationToken.None;
        OrderDto orderDto = new();
        _orderRepositoryMoq.Setup(r => r.AddAsync(It.IsAny<Order>(), token)).Returns(Task.CompletedTask);

        // Act
        await _orderService.CreateOrderAsync(orderDto, token);

        // Assert
        _orderRepositoryMoq.Verify(r => r.AddAsync(It.IsAny<Order>(), token));
    }

    [Fact]
    public async Task UpdateOrderStatusAsync_OrderNotFound_ReturnsFalse()
    {
        // Arrange
        CancellationToken token = CancellationToken.None;
        Guid orderId = Guid.NewGuid();
        OrderStatus newOrderStatus = OrderStatus.Delivered;
        OrderStatus orderStatus = OrderStatus.Pending;
        UpdateOrderStatusDto updateOrderStatusDto = new(orderId, newOrderStatus);
        Order order = new() { Id = orderId, OrderStatus = orderStatus };
        _orderRepositoryMoq.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), token)).ReturnsAsync((Order?)null);

        // Act
        var result = await _orderService.UpdateOrderStatusAsync(updateOrderStatusDto, token);

        // Assert
        Assert.False(result);
        Assert.Equal(orderStatus, order.OrderStatus);
        _orderRepositoryMoq.Verify(r => r.GetByIdAsync(It.IsAny<Guid>(), token));
    }

    [Fact]
    public async Task UpdateOrderStatusAsync_CorrectORder_ReturnsTrue()
    {
        // Arrange
        CancellationToken token = CancellationToken.None;
        Guid orderId = Guid.NewGuid();
        OrderStatus newOrderStatus = OrderStatus.Delivered;
        OrderStatus orderStatus = OrderStatus.Pending;
        UpdateOrderStatusDto updateOrderStatusDto = new(orderId, newOrderStatus);
        Order order = new() { Id = orderId, OrderStatus = orderStatus };
        _orderRepositoryMoq.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), token)).ReturnsAsync(order);

        // Act
        var result = await _orderService.UpdateOrderStatusAsync(updateOrderStatusDto, token);

        // Assert
        Assert.True(result);
        Assert.Equal(newOrderStatus, order.OrderStatus);
        _orderRepositoryMoq.Verify(r => r.GetByIdAsync(It.IsAny<Guid>(), token));
    }

    [Fact]
    public async Task CancelOrderAsync_OrderNotFound_ReturnsFalse()
    {
        // Arrange
        CancellationToken token = CancellationToken.None;
        Guid orderId = Guid.NewGuid();
        Order order = new() { Id = orderId };
        _orderRepositoryMoq.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), token)).ReturnsAsync((Order?)null);

        // Act
        var result = await _orderService.CancelOrderAsync(order.Id, token);

        // Assert
        Assert.False(result);
        Assert.NotEqual(OrderStatus.Cancelled, order.OrderStatus);
        _orderRepositoryMoq.Verify(r => r.GetByIdAsync(It.IsAny<Guid>(), token));
    }

    [Fact]
    public async Task CancelOrderAsync_CorrectOrderId_ReturnsTrue()
    {
        // Arrange
        CancellationToken token = CancellationToken.None;
        Guid orderId = Guid.NewGuid();
        Order order = new() { Id = orderId };
        _orderRepositoryMoq.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), token)).ReturnsAsync(order);

        // Act
        var result = await _orderService.CancelOrderAsync(order.Id, token);

        // Assert
        Assert.True(result);
        Assert.Equal(OrderStatus.Cancelled, order.OrderStatus);
        _orderRepositoryMoq.Verify(r => r.GetByIdAsync(It.IsAny<Guid>(), token));
    }

}