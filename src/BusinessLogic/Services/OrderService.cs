namespace crm.BusinessLogic.Services;

using crm.DataAccess.Entities;
using crm.BusinessLogic.IServices;
using crm.BusinessLogic.Helpers.Extensions;
using crm.BusinessLogic.Dtos;
using crm.DataAccess.DataBase;
using crm.BusinessLogic.Helpers.Extenions;
using crm.DataAccess.Enums;
using crm.DataAccess.IRepositories;
using crm.DataAccess.Repositories;

public class OrderService : IOrderService
{
    private readonly IProductRepository _productRepository;
    private readonly IOrderRepository _orderRepository;

    public OrderService(IOrderRepository orderRepository, IProductRepository productRepository)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
    }
    public async Task<Guid> CreateOrderAsync(OrderDto orderDto, CancellationToken token)
    {
        await _orderRepository.AddAsync(orderDto.ToDbOrder(), token);

        return orderDto.Id;
    }

    public async Task AddProductToOrderAsync(Guid orderId, Guid productId, int quantity, CancellationToken token)
    {
        Order order = (await _orderRepository.GetByIdAsync(orderId))!;

        order.OrderItemIds.Add(productId);

        Product product = (await _productRepository.GetByIdAsync(productId))!;
        
        product.StockQuantity -= quantity;

        order.TotalAmount += (await _productRepository.GetByIdAsync(productId))!.Price * quantity;
    }

    public async Task<bool> UpdateOrderStatusAsync(UpdateOrderStatusDto updateOrderStatusDto, CancellationToken token)
    {
        Order? order = await _orderRepository.GetByIdAsync(updateOrderStatusDto.OrderId, token);

        if (order is null) return false;

        order.OrderStatus = updateOrderStatusDto.NewOrderStatus;

        return true;
    }

    public async Task<bool> CancelOrderAsync(Guid orderId, CancellationToken token)
    {
        Order? order = await _orderRepository.GetByIdAsync(orderId, token);

        if (order is null) return false;

        order.OrderStatus = OrderStatus.Cancelled;
        order.PaymentStatus = PaymentStatus.Cancelled;

        return true;
    }
    public async Task<OrderDto?> GetByIdAsync(Guid orderId, CancellationToken token)
    {
        Order? order = await _orderRepository.GetByIdAsync(orderId);

        if (order is null) return null;

        return order.ToOrderDto();
    }

    public async Task UpdateAsync(OrderDto orderDto, CancellationToken token)
    {
        Order order = (await _orderRepository.GetByIdAsync(orderDto.Id))!;

        await _orderRepository.UpdateAsync(order, orderDto.ToDbOrder());
    }

    public async Task<IEnumerable<OrderDto>> GetAllUserOrdersAsync(Guid userId, CancellationToken token)
        => (await _orderRepository.GetAllUserOrdersAsync(userId)).Select(order => order.ToOrderDto());

    public async Task<IEnumerable<OrderDto>> GetAllAsync(CancellationToken token)
        => (await _orderRepository.GetAllAsync()).Select(order => order.ToOrderDto());

    public async Task<OrderDto> UpdateDtoDataAsync(Guid orderId, CancellationToken token)
        => (await _orderRepository.GetByIdAsync(orderId))!.ToOrderDto();
}