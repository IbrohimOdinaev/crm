
using System.Runtime.InteropServices;
using crm.DataAccess.Entities;

namespace test.DataAccessTests.Helpers;

public static class EntityHelper
{
    public static object CreateDefaultEntity<T>()
    {
        if (typeof(T) == typeof(User))
        {
            return new User()
            {
                Id = Guid.NewGuid(),
            };
        }
        else if (typeof(T) == typeof(Order))
        {
            return new Order()
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid()
            };
        }
        else if (typeof(T) == typeof(Product))
        {
            return new Product()
            {
                Id = Guid.NewGuid(),
            };
        }
        else if (typeof(T) == typeof(OrderItem))
        {
            return new OrderItem()
            {
                OrderId = Guid.NewGuid(),
                ProductId = Guid.NewGuid(),
                ProductName = string.Empty,
                UnitPrice = 3,
                Quantity = 2,
                Subtotal = 1
            };
        }
        else if (typeof(T) == typeof(Payment))
        {
            return new Payment()
            {
                UserId = Guid.NewGuid(),
                OrderId = Guid.NewGuid()
            };
        }
        else if (typeof(T) == typeof(Wallet))
        {
            return new Wallet()
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid()
            };
        }
        return default!;
    }
}