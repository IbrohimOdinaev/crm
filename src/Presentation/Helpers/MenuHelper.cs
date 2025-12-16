using System.Runtime.CompilerServices;
using System.Security.Principal;
using crm.BusinessLogic.Dtos;
using crm.BusinessLogic.Extensions;
using crm.BusinessLogic.Helpers.Extenions;
using crm.BusinessLogic.Helpers.Extensions;
using crm.BusinessLogic.IServices;
using crm.BusinessLogic.Services;
using crm.BussinessLogic.IServices;
using crm.DataAccess.DataBase;
using crm.DataAccess.Enums;
using crm.DataAccess.IRepositories;
using crm.DataAccess.Repositories;
using ConsoleTables;
using crm.Presentation.Helpers;
using crm.DataAccess.Entities;

namespace crm.Presentation.Helpers;

public class MenuHelper
{
    private readonly IUserService _userService;
    private readonly IOrderService _orderService;
    private readonly IWalletService _walletService;
    private readonly IPaymentService _paymentService;
    private readonly IProductService _productService;
    
    public MenuHelper(IUserService userService, IOrderService orderService, IWalletService walletService, IPaymentService paymentService, IProductService productService)
    {
        _userService = userService;
        _orderService = orderService;
        _walletService = walletService;
        _paymentService = paymentService;
        _productService = productService;
    }

    string[] loginAdminMenuItems = { "Register Manager", "Add Product", "Watch Products", "Watch Payments", "Watch Orders", "Watch Wallets", "Watch Users" };
    string[] loginCustomerMenuItems = { "My Profile", "Top Up Wallet", "Change Password", "Place an Order", "Watch my Payments", "Watch my own Orders", "Edit Profile"};
    string[] loginManagerMenuItems = { "Add Product", "Watch Products", "Watch Orders", "Watch Wallets", "Watch Users" };

    public async Task Menu(string[] menuItem, UserDto user, OrderDto order, ProductDto product)
    {
        Console.Clear();
        string[] menuItems = menuItem;
        int selectedIndex = 0;

        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== Главное меню ===\n");

            for (int i = 0; i < menuItems.Length; i++)
            {
                if (i == selectedIndex)
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"> {menuItems[i]}");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine($"  {menuItems[i]}");
                }
            }
            Console.WriteLine("\n↑ ↓ — выбрать товар | Enter — действия | Esc — назад");
            ConsoleKey key = Console.ReadKey(true).Key;
            switch (key)
            {
                case ConsoleKey.UpArrow:
                    Console.Clear();
                    selectedIndex = (selectedIndex - 1 + menuItem.Count()) % menuItem.Count();
                    break;
                case ConsoleKey.DownArrow:
                    Console.Clear();
                    selectedIndex = (selectedIndex + 1) % menuItem.Count();
                    break;
                case ConsoleKey.Escape:
                    Console.Clear();
                    return;
                case ConsoleKey.Enter:
                    if (user.Name != string.Empty)
                    {
                        user = await _userService.UpdateDtoDataAsync(user.Id);
                    }
                    if (order.DeliveryAddress != string.Empty)
                    {
                        order = await _orderService.UpdateDtoDataAsync(order.Id);
                    }
                    if (product!.Name != string.Empty)
                    {
                        product = await _productService.UpdateDtoDataAsync(product.Id);
                    }
                    Console.Clear();
                    Console.WriteLine($"Вы выбрали: {menuItems[selectedIndex]}");
                    string currentMenuItem = menuItems[selectedIndex];

                    switch (currentMenuItem)
                    {
                        case "Register Customer":
                            await _userService.RegisterAsync(await UserHelpers.GetUserDtoAsync(UserRole.Customer, _walletService));
                            Console.WriteLine("New User have benn registred");
                            break;
                        case "Register Manager":
                            await _userService.RegisterAsync(await UserHelpers.GetUserDtoAsync(UserRole.Manager, _walletService));
                            Console.WriteLine("New Manager have benn registred");
                            break;
                        case "Login":
                            LoginDto loginDto = await UserHelpers.GetLoginDataAsync();

                            if (await _userService.LoginAsync(loginDto))
                            {
                                UserDto loginedUser = (await _userService.GetByNameAsync(loginDto.Name))!;

                                Console.WriteLine($"\nYou have loginned as( {user.UserRole})\n");

                                switch (loginedUser.UserRole)
                                {
                                    case UserRole.Admin:
                                        await Menu(loginAdminMenuItems, loginedUser, order, product!);
                                        break;
                                    case UserRole.Customer:
                                        await Menu(loginCustomerMenuItems, loginedUser, order, product!);
                                        break;
                                    case UserRole.Manager:
                                        await Menu(loginManagerMenuItems, loginedUser, order, product!);
                                        break;

                                }
                            }
                            else Console.WriteLine("Uncoorect Password or User Not found Try Again!");
                            break;
                        case "Add Product":
                            await _productService.CreateProductAsync(await ProductHelpers.GetProductDtoAsync());
                            break;
                        case "Change Password":
                            await _userService.ChangePasswordAsync(await UserHelpers.GetChangePasswordDtoAsync());
                            break;
                        case "Watch Users":
                            await Menu(user, order, product!, async () => (await _userService.GetAllAsync()).Where(user => user.UserRole == UserRole.Customer).ToList());
                            break;
                        case "Watch Wallets":
                            await Menu(user, order, product!, async () => (await _walletService.GetAllAsync()).ToList());
                            break;
                        case "Search":
                            await Menu(user, order, product, async () => (await _productService.SearchAsync((await BaseHelpers.GetStringAsync<string>("Product Name: "))!)).ToList());
                            break;
                        case "Update Product":
                            await _productService.UpdateProductAsync(await ProductHelpers.GetProductDtoAsync());
                            break;
                        case "Update Stock":
                            await _productService.UpdateStockAsync(new UpdateStockDto(product!.Id, await BaseHelpers.GetStringAsync<int>("Quantity")));
                            break;
                        case "Update Price":
                            await _productService.UpdatePriceAsync(new UpdatePriceDto(product!.Id, await BaseHelpers.GetStringAsync<decimal>("Price")));
                            break;
                        case "Watch Products":
                            await Menu(user, order, product!,async () => (await _productService.GetAllAsync()).ToList());
                            break;
                        case "Top Up Wallet":
                            await _walletService.AddMoneyAsync(user.Wallet!.Id, await BaseHelpers.GetStringAsync<decimal>("Top up Amount: "));
                            break;
                        case "Watch Orders":
                            await Menu(user, order, product!, async () => (await _orderService.GetAllAsync()).ToList());
                            break;
                        case "My Profile":
                            Console.WriteLine(user);
                            Console.WriteLine(user.Wallet!.Balance);
                            Console.WriteLine("\nPress any Key to continue\n");
                            Console.ReadKey();
                            break;
                        case "Watch my own Orders":
                            await Menu(user, order, product!, async () => (await _orderService.GetAllUserOrdersAsync(user.Id)).ToList());
                            break;
                        case "Edit Profile":
                            await _userService.UpdateUserAsync(await UserHelpers.GetUserDtoAsync(UserRole.Customer, _walletService));
                            break;
                        case "Watch my Payments":
                            await Menu(user, order, product, async () => (await _paymentService.GetByUserIdAsync(user.Id)).ToList());
                            break;
                        case "Watch Payments":
                            await Menu(user, order, product!, async () => (await _paymentService.GetAllAsync()).ToList());
                            break;
                        case "Cancel Order":
                            if (order.OrderStatus == OrderStatus.Pending)
                            {
                                Console.WriteLine("Order has been cancellaed");
                                await _orderService.CancelOrderAsync(order.Id);
                                break;
                            }
                            Console.WriteLine("You can't Cancel this Order");
                            break;
                        case "Refund Order":
                            if (order.PaymentStatus == PaymentStatus.Completed)
                            {
                                if (await _paymentService.RefundPaymentAsync(order.PaymentId) == true) Console.WriteLine("Order has been refunded");
                                else Console.WriteLine("Error"); 
                                Console.WriteLine("\nPress any Key to continue\n");
                                Console.ReadKey();
                                break;
                            }
                            Console.WriteLine("You can't refund this Payment");
                            Console.WriteLine("\nPress any Key to continue\n");
                            Console.ReadKey();
                            break;
                        case "Place an Order":
                            OrderDto orderDto = await OrderHelpers.GetOrderDtoAsync(user!.Id);

                            await _orderService.CreateOrderAsync(orderDto);

                            await Menu(new string[] {"Search", "Watch Products"}, user, (await _orderService.GetByIdAsync(orderDto.Id))!, product!);
                            break;
                        case "Pay for Order":
                            if (order.PaymentStatus == PaymentStatus.Completed || order.PaymentStatus == PaymentStatus.Refunded)
                            {
                                Console.WriteLine("This order is Already Payed or Refunded!");
                                Console.WriteLine("\nPress any Key to continue\n");
                                Console.ReadKey();
                                break;
                            }

                            PaymentDto paymentDto = new() { Id = Guid.NewGuid(), UserId = user.Id, OrderId = order.Id, Amount = order.TotalAmount, PaymentStatus = PaymentStatus.Pending };
                            await _paymentService.CreatePaymentAsync(paymentDto);


                            if (await _paymentService.ProcessPaymentAsync(paymentDto.Id) == false)
                            {
                                Console.WriteLine("Payment Failed. Please Try Again");
                                Console.WriteLine("\nPress any Key to continue\n");
                                Console.ReadKey();
                            }
                            else
                            {
                                Console.WriteLine("Payment Completed");
                                Console.WriteLine($"Payment Id: {paymentDto.Id}");
                            }
                            Console.WriteLine((await _orderService.GetByIdAsync(order.Id))!.PaymentId);
                            Console.WriteLine(order.Id);
                            Console.WriteLine("\nPress any Key to continue\n");
                            Console.ReadKey();
                            break;
                        case "Add":
                            int quantity = await BaseHelpers.GetStringAsync<int>("Quantity");
                            if (quantity > product!.StockQuantity)
                            {
                                Console.WriteLine("This is much more than over stock please try again!");
                                Console.WriteLine("\nPress any Key to continue\n");
                                Console.ReadKey();
                                return;
                            }
                            await _orderService.AddProductToOrderAsync(order.Id, product!.Id, quantity);
                            Console.WriteLine("\nPress any Key to continue\n");
                            Console.ReadKey();
                            break;
                        case "Info":
                            Console.WriteLine(product == null ? "Its is nuuL" : product.Description);
                            Console.WriteLine("\nPress any Key to continue\n");
                            Console.ReadKey();
                            break;
                        case "Delete":
                            await _productService.DeleteProductAsync(product!.Id);
                            Console.WriteLine("\nPress any Key to continue\n");
                            Console.ReadKey();
                            break;
                    }
                    break;
            }
        }
    }


    public async Task Menu(UserDto user, OrderDto order, ProductDto product, Func<Task<List<ProductDto>>> func)
    {
        List<ProductDto> products;
        int selectedIndex = 0;

        while (true)
        {
            products = await func();
            Console.Clear();
            Console.WriteLine("=== Products ===\n");

            var tableProduct = new ConsoleTable("Name", "Price", "Stock Quantity", "Category", "Id");

            foreach (var entity in products)
            {
                tableProduct.AddRow(entity.Name, entity.Price, entity.StockQuantity, entity.Category, entity.Id);
            }

            string tableText = tableProduct.ToString();

            var lines = tableText.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            for (int i = 0; i < lines.Length - 2; i++)
            {
                if (i % 2 == 0 || i <= 2 || i >= lines.Count() - 2)
                {
                    Console.WriteLine(lines[i]);
                    continue;
                }

                int productIndex = (i - 1) / 2 - 1;

                if (productIndex == selectedIndex)
                {
                    if (products[selectedIndex].StockQuantity == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.BackgroundColor = ConsoleColor.Red;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.BackgroundColor = ConsoleColor.Green;
                    }
                    
                }

                Console.WriteLine(lines[i]);
                Console.ResetColor();
            }

            Console.WriteLine("\n↑ ↓ — выбрать товар | Enter — действия | Esc — назад");

            ConsoleKey key = Console.ReadKey(true).Key;
            
            switch (key)
            {
                case ConsoleKey.UpArrow:
                    Console.Clear();
                    selectedIndex = (selectedIndex - 1 + products.Count) % products.Count;
                    break;
                case ConsoleKey.DownArrow:
                    Console.Clear();
                    selectedIndex = (selectedIndex + 1) % products.Count;
                    break;
                case ConsoleKey.Escape:
                    Console.Clear();
                    return;
                case ConsoleKey.Enter:
                    Console.Clear();
                    if (user.Name != string.Empty)
                    {
                        user = await _userService.UpdateDtoDataAsync(user.Id);
                    }
                    if (order.DeliveryAddress != string.Empty)
                    {
                        order = await _orderService.UpdateDtoDataAsync(order.Id);
                    }
                    if (product!.Name != string.Empty)
                    {
                        product = await _productService.UpdateDtoDataAsync(product.Id);
                    }
                    ProductDto selectedProduct = products[selectedIndex];

                    if (user.UserRole == UserRole.Customer)
                    {
                        await Menu(new string[] { "Info", "Add" }, user, order, (await _productService.GetByIdAsync(selectedProduct.Id))!);
                    }
                    else
                    {
                        await Menu(new string[] { "Info", "Delete", "Update Price", "Update Stock", "Update Prdouct" }, user, order, (await _productService.GetByIdAsync(selectedProduct.Id))!);
                    }


                    break;
            }
        }
    }
    public async Task Menu(UserDto user, OrderDto order, ProductDto product, Func<Task<List<OrderDto>>> func)
    {
        List<OrderDto> orders;
        int selectedIndex = 0;

        while (true)
        {
            orders = await func();
            Console.Clear();
            Console.WriteLine("=== Products ===\n");

            var tableOrder= new ConsoleTable("User Name", "Address", "Total Amount", "Items Count", "Order Status", "Payment Status", "Id");

            foreach (var entity in orders)
            {
                tableOrder.AddRow((await _userService.GetByIdAsync(entity.UserId))!.Name, entity.DeliveryAddress, entity.TotalAmount, entity.OrderItemIds.Count, entity.OrderStatus, entity.PaymentStatus, entity.Id);
            }

            string tableText = tableOrder.ToString();

            var lines = tableText.Split('\n');

            for (int i = 0; i < lines.Length - 2; i++)
            {
                if (i % 2 == 0 || i <= 2 || i >= lines.Count() - 2)
                {
                    Console.WriteLine(lines[i]);
                    continue;
                }

                int orderIndex = (i - 1) / 2 - 1;

                if (orderIndex == selectedIndex)
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    switch(orders[selectedIndex].PaymentStatus)
                    {
                        case (PaymentStatus.Completed):
                            Console.BackgroundColor = ConsoleColor.Green;
                            break;
                        case (PaymentStatus.Failed):
                            Console.BackgroundColor = ConsoleColor.Red;
                            break;
                        case (PaymentStatus.Pending):
                            Console.BackgroundColor = ConsoleColor.Yellow;
                            break;
                        case (PaymentStatus.Cancelled):
                            Console.BackgroundColor = ConsoleColor.DarkYellow;
                            break;
                        case (PaymentStatus.Refunded):
                            Console.BackgroundColor = ConsoleColor.Blue;
                            break;

                    }
                }

                Console.WriteLine(lines[i]);
                Console.ResetColor();
            }

            Console.WriteLine("\n↑ ↓ — выбрать товар | Enter — действия | Esc — назад");



            if (orders.Count() == 0)
            {
                Console.WriteLine("Press any Key to continue");
                Console.ReadKey();
                return;
            }
            ConsoleKey key = Console.ReadKey(true).Key;
            switch (key)
            {
                case ConsoleKey.UpArrow:
                    Console.Clear();
                    selectedIndex = (selectedIndex - 1 + orders.Count) % orders.Count;
                    break;
                case ConsoleKey.DownArrow:
                    Console.Clear();
                    selectedIndex = (selectedIndex + 1) % orders.Count;
                    break;
                case ConsoleKey.Escape:
                    Console.Clear();
                    return;
                case ConsoleKey.Enter:
                    Console.Clear();
                    if (user.Name != string.Empty)
                    {
                        user = await _userService.UpdateDtoDataAsync(user.Id);
                    }
                    if (order.DeliveryAddress != string.Empty)
                    {
                        order = await _orderService.UpdateDtoDataAsync(order.Id);
                    }
                    if (product!.Name != string.Empty)
                    {
                        product = await _productService.UpdateDtoDataAsync(product.Id);
                    }
                    Console.WriteLine($"Вы выбрали: {orders[selectedIndex]}");

                    OrderDto currentMenuItem = orders[selectedIndex];

                    if (user.UserRole == UserRole.Customer)
                    {

                        await Menu(new string[] { "Info", "Pay for Order", "Refund Order", "Cancel Order" }, user, (await _orderService.GetByIdAsync(currentMenuItem.Id))!, product);
                    }
                    else
                    {
                        await Menu(new string[] { "Info" }, user, (await _orderService.GetByIdAsync(currentMenuItem.Id))!, product!);
                    }
                    break;

            }
        }
    }


    public async Task Menu(UserDto user, OrderDto order, ProductDto product, Func<Task<List<WalletDto>>> func)
    {
        List<WalletDto> wallets;
        int selectedIndex = 0;

        while (true)
        {
            wallets = await func();
            Console.Clear();
            Console.WriteLine("=== Wallets ===\n");

            var tableWallet = new ConsoleTable("User Name", "Balance", "Id");

            foreach (var entity in wallets)
            {
                tableWallet.AddRow((await _userService.GetByIdAsync(entity.UserId))!.Name, entity.Balance, entity.Id);
            }

            string tableText = tableWallet.ToString();

            var lines = tableText.Split('\n');

            for (int i = 0; i < lines.Length - 2; i++)
            {
                if (i % 2 == 0 || i <= 2 || i >= lines.Count() - 2)
                {
                    Console.WriteLine(lines[i]);
                    continue;
                }

                int walletIndex = (i - 1) / 2 - 1;

                if (walletIndex == selectedIndex)
                {
                    bool activeUser = (await _userService.GetByIdAsync(wallets[selectedIndex].UserId))!.IsActive;
                    Console.ForegroundColor = ConsoleColor.Black;
                    if (activeUser)
                    {
                        Console.BackgroundColor = ConsoleColor.Green;
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.Red;
                    }
                }

                Console.WriteLine(lines[i]);
                Console.ResetColor();
            }

            Console.WriteLine("\n↑ ↓ — выбрать товар | Enter — действия | Esc — назад");

            ConsoleKey key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.UpArrow:
                    Console.Clear();
                    selectedIndex = (selectedIndex - 1 + wallets.Count) % wallets.Count;
                    break;
                case ConsoleKey.DownArrow:
                    Console.Clear();
                    selectedIndex = (selectedIndex + 1) % wallets.Count;
                    break;
                case ConsoleKey.Escape:
                    Console.Clear();
                    return;
                case ConsoleKey.Enter:
                    Console.Clear();
                    if (user.Name != string.Empty)
                    {
                        user = await _userService.UpdateDtoDataAsync(user.Id);
                    }
                    if (order.DeliveryAddress != string.Empty)
                    {
                        order = await _orderService.UpdateDtoDataAsync(order.Id);
                    }
                    if (product!.Name != string.Empty)
                    {
                        product = await _productService.UpdateDtoDataAsync(product.Id);
                    }
                    Console.WriteLine($"Info\n");

                    WalletDto currentMenuItem = wallets[selectedIndex];

                    Console.WriteLine($"User Id: {currentMenuItem.UserId}");
                    Console.WriteLine("\nPress any Key to continue\n");
                    Console.ReadKey();
                    break;
            }

        }
    }




    public async Task Menu(UserDto user, OrderDto order, ProductDto product, Func<Task<List<UserDto>>> func)
    {
        List<UserDto> users;
        int selectedIndex = 0;

        while (true)
        {
            users = await func();
            Console.Clear();
            Console.WriteLine("=== Products ===\n");

            var tableUser = new ConsoleTable("Name", "Password", "Email", "Balance", "Is Active");

            foreach (var entity in users)
            {
                tableUser.AddRow(entity.Name, entity.PasswordHash, entity.Email, entity.Wallet!.Balance, entity.IsActive);
            }

            string tableText = tableUser.ToString();

            var lines = tableText.Split('\n');

            for (int i = 0; i < lines.Length - 2; i++)
            {
                if (i % 2 == 0 || i <= 2 || i >= lines.Length - 2)
                {
                    Console.WriteLine(lines[i]);
                    continue;
                }

                int orderIndex = (i - 1) / 2 - 1;

                if (orderIndex == selectedIndex)
                {
                    bool activeUser = (await _userService.GetByIdAsync(users[selectedIndex].Id))!.IsActive;
                    Console.ForegroundColor = ConsoleColor.Black;
                    if (activeUser)
                    {
                        Console.BackgroundColor = ConsoleColor.Green;
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.Red;
                    }
                }

                Console.WriteLine(lines[i]);
                Console.ResetColor();
            }

            Console.WriteLine("\n↑ ↓ — выбрать товар | Enter — действия | Esc — назад");

            ConsoleKey key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.UpArrow:
                    Console.Clear();
                    selectedIndex = (selectedIndex - 1 + users.Count) % users.Count;
                    break;
                case ConsoleKey.DownArrow:
                    Console.Clear();
                    selectedIndex = (selectedIndex + 1) % users.Count;
                    break;
                case ConsoleKey.Escape:
                    Console.Clear();
                    return;
                case ConsoleKey.Enter:
                    if (user.Name != string.Empty)
                    {
                        user = await _userService.UpdateDtoDataAsync(user.Id);
                    }
                    if (order.DeliveryAddress != string.Empty)
                    {
                        order = await _orderService.UpdateDtoDataAsync(order.Id);
                    }
                    if (product!.Name != string.Empty)
                    {
                        product = await _productService.UpdateDtoDataAsync(product.Id);
                    }
                    Console.Clear();
                    Console.WriteLine($"Info\n");

                    UserDto currentMenuItem = users[selectedIndex];

                    Console.WriteLine($"Id: {currentMenuItem.Id}");
                    Console.WriteLine($"Created At: {currentMenuItem.CreatedAt}");

                    Console.WriteLine("\nPress any Key to continue\n");
                    Console.ReadKey();
                    break;
            }

        }
    }
    
    public async Task Menu(UserDto user, OrderDto order, ProductDto product, Func<Task<List<PaymentDto>>> func)
    {
        List<PaymentDto> payments;
        int selectedIndex = 0;
        
        while (true)
        {
            payments = await func();
            Console.Clear();
            Console.WriteLine("=== Products ===\n");

            var tablePayment = new ConsoleTable("User name", "Total Amount", "Status",  "Order Id", "Id");

            foreach (var entity in payments)
            {
                tablePayment.AddRow((await _userService.GetByIdAsync(entity.UserId))!.Name, entity.Amount, entity.PaymentStatus, entity.OrderId, entity.Id);
            }

            string tableText = tablePayment.ToString();

            var lines = tableText.Split('\n');

            for (int i = 0; i < lines.Length - 2; i++)
            {
                if (i % 2 == 0 || i <= 2 || i >= lines.Length - 2)
                {
                    Console.WriteLine(lines[i]);
                    continue;
                }

                int orderIndex = (i - 1) / 2 - 1;

                if (orderIndex == selectedIndex)
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    switch(payments[selectedIndex].PaymentStatus)
                    {
                        case (PaymentStatus.Completed):
                            Console.BackgroundColor = ConsoleColor.Green;
                            break;
                        case (PaymentStatus.Failed):
                            Console.BackgroundColor = ConsoleColor.Red;
                            break;
                        case (PaymentStatus.Pending):
                            Console.BackgroundColor = ConsoleColor.Yellow;
                            break;
                        case (PaymentStatus.Cancelled):
                            Console.BackgroundColor = ConsoleColor.DarkYellow;
                            break;
                        case (PaymentStatus.Refunded):
                            Console.BackgroundColor = ConsoleColor.Blue;
                            break;
                    }
                }

                Console.WriteLine(lines[i]);
                Console.ResetColor();
            }

            Console.WriteLine("\n↑ ↓ — выбрать товар | Enter — действия | Esc — назад");

            ConsoleKey key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.UpArrow:
                    Console.Clear();
                    selectedIndex = (selectedIndex - 1 + payments.Count) % payments.Count;
                    break;
                case ConsoleKey.DownArrow:
                    Console.Clear();
                    selectedIndex = (selectedIndex + 1) % payments.Count;
                    break;
                case ConsoleKey.Escape:
                    Console.Clear();
                    return;
                case ConsoleKey.Enter:
                    Console.Clear();
                    if (user.Name != string.Empty)
                    {
                        user = await _userService.UpdateDtoDataAsync(user.Id);
                    }
                    if (order.DeliveryAddress != string.Empty)
                    {
                        order = await _orderService.UpdateDtoDataAsync(order.Id);
                    }
                    if (product!.Name != string.Empty)
                    {
                        product = await _productService.UpdateDtoDataAsync(product.Id);
                    }
                    Console.WriteLine($"Info\n");

                    PaymentDto currentMenuItem = payments[selectedIndex];

                    Console.WriteLine($"User Id: {currentMenuItem.UserId}");

                    Console.WriteLine("\nPress any Key to continue\n");
                    Console.ReadKey();
                    break;
            }

        }
    }

}

