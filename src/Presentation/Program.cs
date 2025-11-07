using System.Net.Security;
using System.Runtime.CompilerServices;
using crm.BusinessLogic.IServices;
using crm.BusinessLogic.Services;
using crm.DataAccess.IRepositories;
using crm.BusinessLogic.Dtos;
using crm.DataAccess.Enums;
using static crm.Presentation.Helpers.UserHelpers;
using crm.DataAccess.Repositories;
using System.Drawing;
using static crm.Presentation.Helpers.BaseHelpers;
using crm.BussinessLogic.IServices;
using crm.Presentation.Helpers;
using crm.BusinessLogic.Extensions;

IProductRepository _productRepository = new ProductRepository();
IUserRepository _userRepository = new UserRepository();
IOrderRepository _orderRepository = new OrderRepository();
IPaymentRepository _paymentRepository = new PaymentRepository();
IWalletRepository _walletRepository = new WalletRepository();
IOrderItemRepository _orderItemRepository = new OrderItemRepository();

IWalletService _walletService = new WalletService(_walletRepository);
IUserService _userService = new UserService(_userRepository, _walletService, _walletRepository);
IOrderService _orderService = new OrderService(_orderRepository, _productRepository);
IPaymentService _paymentService = new PaymentService(_paymentRepository, _orderRepository, _userRepository, _walletService);
IProductService _productService = new ProductService(_productRepository);

MenuHelper _menuHelper = new(_userService, _orderService, _walletService, _paymentService, _productService);

await _userService.RegisterAsync(new UserDto() { Email = "jfkdjf", Id = Guid.NewGuid(), Name = "ibo", UserRole = UserRole.Customer, IsActive = true, PasswordHash = "1111"});

await _userService.RegisterAsync(new UserDto() {Id = Guid.NewGuid(), Name = "Admin", UserRole = UserRole.Admin, IsActive = true, PasswordHash = "admin", Wallet = null});

string[] menuItemsLayer1 = { "Register Customer", "Login" };

Console.Clear();
await _menuHelper.Menu(menuItemsLayer1, new UserDto(), new OrderDto(), new ProductDto());

