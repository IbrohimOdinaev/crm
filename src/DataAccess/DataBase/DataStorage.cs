namespace crm.DataAccess.DataBase;

using crm.DataAccess.Entities;

public class DataStorage
{
    public static readonly List<User> Users = new();
    

    public static readonly List<Product> Products = new()
    {
        new Product() {Id = Guid.NewGuid(), Name = "choy", Price = 100, StockQuantity = 200, IsAvailable = true, Description = "It is testy choy", Category = Enums.ProductCategory.Food, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now},
        new Product() {Id = Guid.NewGuid(), Name = "non", Price = 200 , StockQuantity = 50, IsAvailable = true,Description = "It is testy non",  Category = Enums.ProductCategory.Food, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now},
        new Product() {Id = Guid.NewGuid(), Name = "kola", Price = 300 , StockQuantity = 10, IsAvailable = true, Description = "It is testy kola",  Category = Enums.ProductCategory.Food, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now}
    };

    public static readonly List<Order> Orders = new();

    public static readonly List<OrderItem> OrderItems = new();

    public static readonly List<Payment> Payments = new();

    public static readonly List<Wallet> Wallets = new();
}
