using System.Threading.Tasks;
using crm.DataAccess.DataBase;
using crm.DataAccess.Entities;
using crm.DataAccess.IRepositories;
using crm.DataAccess.Repositories;
using static test.DataAccessTests.Helpers.EntityHelper;

namespace crm.test.DataAccessTests.RepositoriesTests.OrderItemRepositoryTests;

public class OrderItemRepositoryTests
{
    private readonly IOrderItemRepository _orderItemRepository = new OrderItemRepository();

    [Fact]
    public async Task GetByIdAsync_ValidValue_OrderItem()
    {
        // Arrange 
        OrderItem orderItem = (OrderItem)CreateDefaultEntity<OrderItem>();
        Guid searchId = orderItem.Id;
        DataStorage.OrderItems.Add(orderItem);

        //Act 
        var result = await _orderItemRepository.GetByIdAsync(searchId);

        //Assert
        Assert.Equal(orderItem, result);
    }

    [Fact]
    public async Task GetAllOrderOrderItems_ValidId_ListOrderItem()
    {
        //Arrange
        DataStorage.OrderItems.Clear();
        Guid orderId = Guid.NewGuid();
        List<OrderItem> orderItemList = new()
        {
            new OrderItem() {OrderId = orderId},
            new OrderItem() {OrderId = orderId},
            new OrderItem() {OrderId = orderId},
            new OrderItem() {OrderId = orderId},
            new OrderItem() {OrderId = orderId},
        };
        DataStorage.OrderItems.AddRange(orderItemList);

        //Act 
        var result = (await _orderItemRepository.GetAllOrderOrderItemsAsync(orderId));
        var expected = orderItemList;

        //Assert
        Assert.Equal(result, expected);
    }
}