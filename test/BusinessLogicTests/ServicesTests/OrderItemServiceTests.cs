using crm.BusinessLogic.IServices;
using crm.DataAccess.IRepositories;
using Moq;
using crm.BusinessLogic.Services;
using crm.BusinessLogic.Dtos;
using crm.DataAccess.Entities;

namespace test.BusinessLogicTests.ServicesTEsts;

public class OrderItemServiceTests
{
    private readonly Mock<IOrderItemRepository> _orderItemRepositoryMoq;
    private readonly OrderItemService _orderItemService;
    public OrderItemServiceTests()
    {
        _orderItemRepositoryMoq = new();
        _orderItemService = new(_orderItemRepositoryMoq.Object);
    }



    [Fact]
    public async Task GetAllOrderItemsAsync_ReturnsListOrderItem()
    {
        // Arrange
        CancellationToken token = CancellationToken.None;
        _orderItemRepositoryMoq.Setup(r => r.GetAllOrderOrderItemsAsync(It.IsAny<Guid>(), token)).ReturnsAsync(new List<OrderItem>());

        // Act
        var result = await _orderItemService.GetOrderItemsAsync(Guid.NewGuid(), token);

        // Assert
        _orderItemRepositoryMoq.Verify(r => r.GetAllOrderOrderItemsAsync(It.IsAny<Guid>(), token));
    }
}