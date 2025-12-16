using crm.DataAccess.Repositories;
using crm.DataAccess.IRepositories;
using crm.DataAccess.DataBase;
using crm.DataAccess.Entities;
using static test.DataAccessTests.Helpers.EntityHelper;
using System.Data.Common;
using crm.DataAccess.Enums;

namespace test.DataAccessTests.RepositoriesTests;

public class OrderRepositoryTests
{
    IOrderRepository _orderRepository = new OrderRepository();
    [Fact]
    public async Task AddAsync_ValidValue()
    {
        //Arrange
        Order newOrder = (Order)CreateDefaultEntity<Order>();

        //Act
        await _orderRepository.AddAsync(newOrder);

        //Assert
        Assert.Contains(newOrder, DataStorage.Orders);
    }


    [Fact]
    public async Task GetByIdAsync_ValidId()
    {
        //Arrange 
        Order order = (Order)CreateDefaultEntity<Order>();
        DataStorage.Orders.Add(order);

        //Act
        var result = await _orderRepository.GetByIdAsync(order.Id);

        //Assert
        Assert.Equal(order, result);
    }

    [Fact]
    public async Task GetByIdAsync_InvalidId()
    {
        Order order = (Order)CreateDefaultEntity<Order>();
        DataStorage.Orders.Add(order);

        //Act
        var result = await _orderRepository.GetByIdAsync(Guid.NewGuid());

        //Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllUserOrdersAsync_InvalidId()
    {
        Order order = (Order)CreateDefaultEntity<Order>();
        DataStorage.Orders.Add(order);

        //Act
        var result = await _orderRepository.GetAllUserOrdersAsync(Guid.NewGuid());

        //Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetAllUserOrdersAsync_ValidId()
    {
        Order order = (Order)CreateDefaultEntity<Order>();
        DataStorage.Orders.Add(order);

        //Act
        var result = await _orderRepository.GetAllUserOrdersAsync(order.UserId);

        //Assert
        Assert.Equal(result, new List<Order> {order});
    }

    [Fact]
    public async Task GetAllAsync_ReturnAllOrders()
    {
        //Arrange
        DataStorage.Orders.Clear();
        List<Order> orders = new();
        for (int i = 0; i < 10; i++)
        {
            orders.Add((Order)CreateDefaultEntity<Order>());
        }
        DataStorage.Orders.AddRange(orders);

        //Act
        var result = await _orderRepository.GetAllAsync();

        //Assert
        Assert.Equal(result, orders);
    }

    [Fact]
    public async Task GetByStatusAsync_ReturnValidList()
    {
        //Arrange
        DataStorage.Orders.Clear();
        DataStorage.Orders.AddRange(new List<Order>()
        {
            new Order() {OrderStatus = OrderStatus.Cancelled },
            new Order() {OrderStatus = OrderStatus.Cancelled },
            new Order() {OrderStatus = OrderStatus.Cancelled },
            new Order() {OrderStatus = OrderStatus.Pending },
            new Order() {OrderStatus = OrderStatus.Confirmed },
            new Order() {OrderStatus = OrderStatus.Delivered },
            new Order() {OrderStatus = OrderStatus.Cancelled}
        });

        //Act 
        var result = (await _orderRepository.GetByStatusAsync(OrderStatus.Cancelled));

        //Assert
        Assert.Equal(4, result.Count());
    }
}