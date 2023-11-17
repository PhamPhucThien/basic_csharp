using basic_csharp.Models;
using basic_csharp.Payload.Response;
using basic_csharp.SQLAdapter;
using System.Collections.Generic;
using System.Data;

namespace basic_csharp  
{
    /// <summary>
    /// Create object: user, product, cart, order database scheme
    /// Create SQL Adapter: AppDBAdapter: insert, update, delete, select
    /// Create cart service: add product to user's cart, dump user's cart
    /// Create order service: create user's order, add product's from user's cart; delete product in cart
    /// 
    /// </summary>
    public class Program
    {
        static string response = "";
        static string username, password, name = "", role = "";
        static Guid accountId;

        /// <summary>
        /// Ctrl K D: chỉnh lại code
        /// Ctrl M O: đóng gói lại code
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            bool useApp = true;

            while (useApp)
            {
                int value = 0;
                Console.Clear();
                Console.WriteLine("---BookStore App---\n***");
                Console.Write("1 - Login\n" +
                              "2 - Register\n" +
                              "Any - Quit\n" +
                              "Value: ");

                value = getValue();
                switch (value)
                {
                    case 1:
                        Login();
                        break;
                    case 2:
                        Register();
                        break;
                    default:
                        useApp = false;
                        break;
                }

            }
        }

        private static void Register()
        {
            bool useApp = true;

            while (useApp)
            {
                string createUsername, createPassword, createName;
                Console.Clear();
                Console.WriteLine("---BookStore App---\n***");
                AccountSQLAdapter accountSQLAdapter = new AccountSQLAdapter();

                Console.Write("Username: ");
                createUsername = Console.ReadLine();
                Console.Write("Password: ");
                createPassword = Console.ReadLine();
                Console.Write("Your name: ");
                createName = Console.ReadLine();
                response = "";

                bool checkUsername = accountSQLAdapter.GetByUsername(createUsername);
                if (checkUsername || createUsername.Length < 4 || createPassword.Length < 4 || createName.Length < 4)
                {
                    Console.Clear();
                    if (checkUsername) Console.Write("Username has been used\n");
                    if (createUsername.Length < 4) Console.Write("Username length must be greater than 3.\n");
                    if (createPassword.Length < 4) Console.Write("Password length must be greater than 3.\n");
                    if (createName.Length < 4) Console.Write("Name length must be greater than 3.\n");
                    Console.Write("Do you want to still register? (Press 1 to continue login, any to go back)\n" +
                                "Value: ");
                    if (getValue() == 1)
                        useApp = true;
                    else useApp = false;
                }
                else
                {
                    Console.Write("Do you want to create account? (Press 1 to continue, any to go back): ");
                    int value = getValue();
                    if (value == 1)
                    {
                        ResponseObject<Account> account = accountSQLAdapter.Register(createUsername, createPassword, createName);

                        if (account.Object != null)
                        {
                            role = account.Object.Role;
                            name = account.Object.Name;
                            accountId = account.Object.Id;
                            UserOption();
                            role = "";
                            name = "";
                            useApp = false;
                        }
                        else
                        {
                            Console.Clear();
                            response = account.Log;
                            Console.WriteLine(response);
                            Console.Write("Do you want to still register? (Press 1 to continue login, any to go back)\n" +
                                "Value: ");
                            if (getValue() == 1)
                                useApp = true;
                            else useApp = false;
                        }
                    }
                    else useApp = false;
                }
            }
        }

        private static int getValue()
        {
            string value;
            value = Console.ReadLine();
            int input = 0;
            try
            {
                input = Int32.Parse(value);
            }
            catch
            {
                input = 0;
            }
            return input;
        }

        private static void Login()
        {
            bool useApp = true;

            while (useApp)
            {
                Console.Clear();
                Console.WriteLine("---BookStore App---\n***");
                AccountSQLAdapter accountSQLAdapter = new AccountSQLAdapter();

                bool loginStatus = false;
                Console.Write("Username: ");
                username = Console.ReadLine();
                Console.Write("Password: ");
                password = Console.ReadLine();
                response = "";

                ResponseObject<Account> account = accountSQLAdapter.Login(username, password);
                if (account.Object != null)
                {
                    loginStatus = true;
                    role = account.Object.Role;
                    name = account.Object.Name;
                    accountId = account.Object.Id;
                }
                else
                {
                    response = account.Log;
                    Console.Clear();
                    Console.WriteLine(response);
                    Console.Write("Do you want to still login? (Press 1 to continue login, any to go back)\n" +
                        "Value: ");
                    if (getValue() == 1)
                        useApp = true;
                    else useApp = false;
                }

                if (loginStatus)
                {
                    if (role == "User") UserOption();
                    if (role == "Admin") AdminOption();
                    role = "";
                    name = "";
                    useApp = false; 
                } 

            }
        }
        public static void UserOption()
        {
            bool useApp = true;

            while (useApp)
            {
                int value;
                Console.Clear();
                Console.WriteLine("---BookStore App---\n***");
                Console.Write(
                    "1 - Add a new item to cart\n" +
                    "2 - Delete an item in cart\n" +
                    "3 - Change quantity of product\n" +
                    "4 - Create order from cart\n" +
                    "5 - View cart\n" +
                    "6 - View orders\n" +
                    "Any - Log out\n" +
                    "Value: ");
                value = getValue();
                switch (value)
                {
                    case 1:
                        AddItemToCart();
                        break;
                    case 2:
                        DeleteItemInCart();
                        break;
                    case 3:
                        ChangeQuantityInCart();
                        break;
                    case 4:
                        CreateOrder();
                        break;
                    case 5:
                        ViewCart();
                        break;
                    case 6:
                        ViewOrder();
                        break;
                    default:
                        useApp = false;
                        break;
                }
            }
        }

        private static void ViewOrder()
        {
            bool useApp = true;
            int page = 1, getPage = 0;
            string searchValue = "", lastSearchValue = "";
            OrderSQLAdapter orderSQLAdapter = new OrderSQLAdapter();

            while (useApp)
            {
                int value, getCode;
                Console.Clear();
                Console.WriteLine("---BookStore App---\n***");
                ResponseObject<Order> order = orderSQLAdapter.GetOrderWithPage(accountId, page);
                if (order.Object != null)
                {
                    string[] records = order.Object.OrderRecord.Split('/');
                    foreach (string record in records)
                    {
                        if (record != "")
                        {
                            string[] recordInfo = record.Split("&");
                            Console.WriteLine(recordInfo[0] + " - " + recordInfo[1] + " - " + recordInfo[2] + "$ " + recordInfo[3] + (recordInfo[3].Equals("1") ? " book" : " books"));
                        }
                    }
                }
                else
                {
                    Console.WriteLine("There is no order!\nPress any key to go back!");
                    Console.ReadLine();
                    break;
                }

                Console.WriteLine("________________________\n" +
                    "Create date: " + order.Object.CreateDate.ToString() + "\n" +
                    "Total price: " + order.Object.TotalPrice + "\n" +
                    "Total page: " + order.Log + "\n" +
                    "Current page: " + page + "\n" +
                    "________________________");

                Console.Write("1 - Next page\n" +
                              "2 - Previous page\n" +
                              "3 - Go to page\n" +
                              "Any - Log out\n" +
                              "Value: ");
                value = getValue();
                switch (value)
                {
                    case 1:
                        try
                        {
                            if (page == Convert.ToInt32(order.Log))
                            {
                                Console.Clear();
                                Console.WriteLine("This is the last page.\nPress any key to go back!");
                                Console.ReadLine();
                            }
                            else
                            {
                                page++;
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Some sh*t happened and I don't know why!");
                        }
                        break;
                    case 2:
                        try
                        {
                            if (page == 1)
                            {
                                Console.Clear();
                                Console.WriteLine("This is the first page.\nPress any key to go back!");
                                Console.ReadLine();
                            }
                            else
                            {
                                page--;
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Some sh*t happened and I don't know why!");
                        }
                        break;
                    case 3:
                        Console.Write("Enter page number (Press 0 or not number to go back)\n" +
                            "Value: ");
                        getPage = getValue();
                        if (getPage != 0)
                        {
                            if (getPage < 1 || getPage > Convert.ToInt32(order.Log))
                            {
                                Console.Clear();
                                Console.WriteLine("Page is out of range! \nPlease enter valid number.");
                                Console.ReadLine();
                            }
                            else
                            {
                                page = getPage;
                            }

                        }
                        break;
                    default:
                        useApp = false;
                        break;
                }
            }
        }

        private static void ViewCart()
        {
            bool useApp = true;
            CartSQLAdapter cartSQLAdapter = new CartSQLAdapter();
            ProductSQLAdapter productSQLAdapter = new ProductSQLAdapter();

            while (useApp)
            {
                int value;
                Console.Clear();
                Console.WriteLine("---BookStore App---\n***");

                ResponseObject<Cart> getCart = cartSQLAdapter.GetCartByAccountId(accountId);
                string[] records = getCart.Object.CartRecord.Split('/');
                List<int> codes = new List<int>();
                foreach (string record in records)
                {
                    if (record != "")
                    {
                        string[] recordInfo = record.Split("&");
                        try
                        {
                            codes.Add(Convert.ToInt32(recordInfo[0]));
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Some sh*t happened and I don't know why!");
                        }
                    }
                }

                ResponseObject<List<Product>> getProducts = productSQLAdapter.GetByListOfCode(codes);
                if (getProducts.Log != null)
                {
                    Console.WriteLine(getProducts.Log + "\nPress any key to go back!");
                    Console.ReadLine();
                    break;
                }

                foreach (string record in records)
                {
                    if (record != "")
                    {
                        string[] recordInfo = record.Split("&");
                        try
                        {
                            int code = Convert.ToInt32(recordInfo[0]);
                            string quantityInString = recordInfo[recordInfo.Length - 1];
                            Console.Write(code + " - ");
                            foreach (Product product in getProducts.Object)
                            {
                                if (product.Code == code) Console.WriteLine(product.ProductName + " - " + product.Price + "$ - " + quantityInString + (quantityInString.Equals("1") ? " book" : " books"));
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Some sh*t happened and I don't know why!");
                        }
                    }
                }
                Console.WriteLine("________________________\n" +
                    "Press any key to go back!");
                Console.ReadLine();
                useApp = false;
            }
        }
        private static void CreateOrder()
        {
            CartSQLAdapter cartSQLAdapter = new CartSQLAdapter();
            ProductSQLAdapter productSQLAdapter = new ProductSQLAdapter();
            OrderSQLAdapter orderSQLAdapter = new OrderSQLAdapter();


            ResponseObject<Cart> getCart = cartSQLAdapter.GetCartByAccountId(accountId);
            Console.Clear();
            Console.WriteLine("---BookStore App---\n***");

            if (getCart.Object.CartRecord == null || getCart.Object.CartRecord == "")
            {
                Console.Clear();
                Console.WriteLine("The cart is empty, we can not create order.\nPress any key to go back");
                Console.ReadLine();
            }
            else
            {
                string[] records = getCart.Object.CartRecord.Split('/');
                List<int> codes = new List<int>();
                foreach (string record in records)
                {
                    if (record != "")
                    {
                        string[] recordInfo = record.Split("&");
                        try
                        {
                            codes.Add(Convert.ToInt32(recordInfo[0]));
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Some sh*t happened and I don't know why!");
                        }
                    }
                }

                ResponseObject<List<Product>> getProducts = productSQLAdapter.GetByListOfCode(codes);
                string newRecords = "";
                long total = 0;
                foreach (string record in records)
                {
                    if (record != "")
                    {
                        string[] recordInfo = record.Split("&");
                        try
                        {
                            int code = Convert.ToInt32(recordInfo[0]);
                            string quantityInString = recordInfo[recordInfo.Length - 1];
                            int quantity = Convert.ToInt32(quantityInString);
                            Console.Write(code + " - ");
                            foreach (Product product in getProducts.Object)
                            {
                                if (product.Code == code)
                                {
                                    Console.WriteLine(product.ProductName + " - " + product.Price + "$ - " + quantityInString + (quantityInString.Equals("1") ? " book" : " books"));
                                    newRecords += product.Code + "&" + product.ProductName + "&" + product.Price + "&" + quantity + "/";
                                    total += product.Price * quantity;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Some sh*t happened and I don't know why!");
                        }
                    }
                }

                Console.WriteLine("----------------Total: " + total + "$");
                Console.Write("Do you want to create order? (Press 1 to confirm, any to deny): ");
                int value = getValue();
                if (value == 1)
                {
                    orderSQLAdapter.AddOrder(accountId, newRecords, total);
                    cartSQLAdapter.UpdateCartRecord(accountId, "");
                    Console.Clear();
                    Console.Write("Create record successfully.\nPress any key to go back!");
                    Console.ReadLine();
                }
            }
        }

        private static void ChangeQuantityInCart()
        {
            bool useApp = true;
            CartSQLAdapter cartSQLAdapter = new CartSQLAdapter();
            ProductSQLAdapter productSQLAdapter = new ProductSQLAdapter();

            while (useApp)
            {
                int value;
                Console.Clear();
                Console.WriteLine("---BookStore App---\n***");

                ResponseObject<Cart> getCart = cartSQLAdapter.GetCartByAccountId(accountId);

                if (getCart.Object == null)
                {
                    Console.WriteLine("The cart is empty.\nPress any key to go back");
                    Console.ReadLine();
                    break;
                }
                string[] records = getCart.Object.CartRecord.Split('/');
                List<int> codes = new List<int>();
                foreach (string record in records)
                {
                    if (record != "")
                    {
                        string[] recordInfo = record.Split("&");
                        try
                        {
                            codes.Add(Convert.ToInt32(recordInfo[0]));
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Some sh*t happened and I don't know why!");
                        }
                    }
                }

                ResponseObject<List<Product>> getProducts = productSQLAdapter.GetByListOfCode(codes);
                if (getProducts.Log != null)
                {
                    Console.WriteLine(getProducts.Log + "\nPress any key to go back!");
                    Console.ReadLine();
                    break;
                }

                foreach (string record in records)
                {
                    if (record != "")
                    {
                        string[] recordInfo = record.Split("&");
                        try
                        {
                            int code = Convert.ToInt32(recordInfo[0]);
                            string quantityInString = recordInfo[recordInfo.Length - 1];
                            Console.Write(code + " - ");
                            foreach (Product product in getProducts.Object)
                            {
                                if (product.Code == code) Console.WriteLine(product.ProductName + " - " + product.Price + "$ - " + quantityInString + (quantityInString.Equals("1") ? " book" : " books"));
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Some sh*t happened and I don't know why!");
                        }
                    }
                }
                Console.Write("________________________\n" +
                    "Choose product to change quantity (0 and any to go back): ");
                value = getValue();
                if (value != 0)
                {
                    Console.Write("Enter the quantity (Press 0 or not number to go back): ");
                    int getQuantity = getValue();
                    bool falseUpdate = false, noContent = true;
                    if (getQuantity != 0)
                    {
                        ResponseObject<Product> getByCode = productSQLAdapter.GetProductByCodeAndQuantity(value, getQuantity);
                        if (getByCode.Log == null)
                        {
                            string newRecords = "";
                            foreach (string record in records)
                            {
                                if (record != "")
                                {
                                    if (record.StartsWith(getByCode.Object.Code + "&"))
                                    {
                                        try
                                        {
                                            if (getQuantity <= getByCode.Object.Quantity)
                                            {
                                                newRecords += getByCode.Object.Code + "&" + getQuantity + "/";
                                                noContent = false;
                                            }
                                            else
                                            {
                                                falseUpdate = true;
                                                getByCode.Log = "The quantity is out of range!";
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine("Some sh*t happened and I don't know why!");
                                        }
                                    }
                                    else
                                    {
                                        newRecords += record + "/";
                                    }
                                }
                            }
                            if (noContent)
                            {
                                Console.Clear();
                                Console.Write("Can not find product in cart.\nPress any key to go back!");
                                Console.ReadLine();
                            }
                            else if (!falseUpdate)
                            {
                                Console.Clear();
                                int x = cartSQLAdapter.UpdateCartRecord(accountId, newRecords);
                                if (x >= 1)
                                {
                                    Console.WriteLine("Update quantity successfully!\nPress any key to go back!");
                                }
                                else
                                {
                                    Console.WriteLine("Update quantity failed!\nPress any key to go back!");
                                }
                                Console.ReadLine();
                            }
                            else
                            {
                                Console.Clear();
                                Console.Write(getByCode.Log + "\nPress any key to go back!");
                                Console.ReadLine();
                            }
                        }
                        else
                        {
                            Console.Clear();
                            Console.Write(getByCode.Log + "\nPress any key to go back!");
                            Console.ReadLine();
                        }
                    }
                }
                else
                {
                    useApp = false;
                }
            }
        }

        private static void DeleteItemInCart()
        {
            bool useApp = true;
            CartSQLAdapter cartSQLAdapter = new CartSQLAdapter();
            ProductSQLAdapter productSQLAdapter = new ProductSQLAdapter();

            while (useApp)
            {
                int value;
                Console.Clear();
                Console.WriteLine("---BookStore App---\n***");

                ResponseObject<Cart> getCart = cartSQLAdapter.GetCartByAccountId(accountId);

                if (getCart.Object == null)
                {
                    Console.WriteLine("The cart is empty.\nPress any key to go back");
                    Console.ReadLine();
                    break;
                }
                string[] records = getCart.Object.CartRecord.Split('/');
                List<int> codes = new List<int>();
                foreach (string record in records)
                {
                    if (record != "")
                    {
                        string[] recordInfo = record.Split("&");
                        try
                        {
                            codes.Add(Convert.ToInt32(recordInfo[0]));
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Some sh*t happened and I don't know why!");
                        }
                    }
                }

                ResponseObject<List<Product>> getProducts = productSQLAdapter.GetByListOfCode(codes);
                if (getProducts.Log != null)
                {
                    Console.WriteLine(getProducts.Log + "\nPress any key to go back!");
                    Console.ReadLine();
                    break;
                }

                foreach (string record in records)
                {
                    if (record != "")
                    {
                        string[] recordInfo = record.Split("&");
                        try
                        {
                            int code = Convert.ToInt32(recordInfo[0]);
                            string quantityInString = recordInfo[recordInfo.Length - 1];
                            Console.Write(code + " - ");
                            foreach (Product product in getProducts.Object)
                            {
                                if (product.Code == code) Console.WriteLine(product.ProductName + " - " + product.Price + "$ - " + quantityInString + (quantityInString.Equals("1") ? " book" : " books"));
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Some sh*t happened and I don't know why!");
                        }
                    }
                }
                Console.Write("________________________\n" +
                    "Choose product to delete (0 and any to go back): ");
                value = getValue();
                if (value != 0)
                {
                    bool isDeleted = false;
                    foreach (Product product in getProducts.Object)
                    {
                        if (value == product.Code)
                        {
                            isDeleted = true;
                            string newRecords = "";
                            foreach (string record in records)
                            {
                                if (record != "")
                                {
                                    if (!record.StartsWith(value + "&"))
                                    {
                                        newRecords += record + "/";
                                    }
                                }
                            }
                            cartSQLAdapter.UpdateCartRecord(accountId, newRecords);

                            break;
                        }
                    }

                    Console.Clear();
                    if (isDeleted)
                    {
                        Console.WriteLine("Delete product in cart succesfully");
                    }
                    else
                    {
                        Console.WriteLine("Delete product in cart failed");
                    }
                    Console.WriteLine("Press any key to go back!");
                    Console.ReadLine();
                }
                else
                {
                    useApp = false;
                }
            }
        }

        private static void AddItemToCart()
        {
            bool useApp = true, falseUpdate = false, isDuplicated = false;
            int page = 1, lastPage = 0, getPage = 0, getCode = 0, getQuantity = 0;
            string searchValue = "", lastSearchValue = "";
            ProductSQLAdapter productSQLAdapter = new ProductSQLAdapter();
            CartSQLAdapter cartSQLAdapter = new CartSQLAdapter();
            while (useApp)
            {
                int value = 0;
                Console.Clear();
                Console.WriteLine("---BookStore App---\n***");
                ResponseObject<List<Product>> products = productSQLAdapter.GetProductsByPageAndSearch(page, searchValue);
                if (products.Object != null)
                {
                    foreach (Product p in products.Object)
                    {
                        Console.WriteLine(p.Code + " - " + p.ProductName + " - " + p.Price + "$");
                    }


                    Console.WriteLine("________________________\n" +
                        "Total page: " + products.Log + "\n" +
                        "Current search value: " + searchValue + "\n" +
                        "Current page: " + page + "\n" +
                        "________________________");


                    Console.Write("1 - Search product\n" +
                                  "2 - Add a product to card with code\n" +
                                  "3 - Next page\n" +
                                  "4 - Previous page\n" +
                                  "5 - Go to page\n" +
                                  "Any - Log out\n" +
                                  "Value: ");
                    value = getValue();
                    switch (value)
                    {
                        case 1:
                            Console.Write("Enter product's name: ");
                            searchValue = Console.ReadLine();
                            break;
                        case 2:
                            Console.Write("Enter the code (Press 0 or not number to go back): ");
                            getCode = getValue();
                            if (getCode != 0)
                            {
                                Console.Write("Enter the quantity (Press 0 or not number to go back): ");
                                getQuantity = getValue();
                                if (getQuantity != 0)
                                {
                                    ResponseObject<Product> getByCode = productSQLAdapter.GetProductByCodeAndQuantity(getCode, getQuantity);
                                    if (getByCode.Log == null)
                                    {
                                        isDuplicated = false;
                                        ResponseObject<Cart> getCart = cartSQLAdapter.GetCartByAccountId(accountId);

                                        string[] records = getCart.Object.CartRecord.Split('/');
                                        string newRecords = "";
                                        foreach (string record in records)
                                        {
                                            if (record != "")
                                            {
                                                if (record.StartsWith(getByCode.Object.Code + "&"))
                                                {
                                                    isDuplicated = true;
                                                    try
                                                    {
                                                        string[] recordInfo = record.Split("&");
                                                        int currentQuantity = Convert.ToInt32(recordInfo[recordInfo.Length - 1]);
                                                        if (getQuantity + currentQuantity <= getByCode.Object.Quantity)
                                                        {
                                                            newRecords += getByCode.Object.Code + "&" + (getQuantity + currentQuantity) + "/";
                                                        }
                                                        else
                                                        {
                                                            falseUpdate = true;
                                                            getByCode.Log = "The quantity is out of range!";
                                                        }
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        Console.WriteLine("Some sh*t happened and I don't know why!");
                                                    }
                                                }
                                                else
                                                {
                                                    newRecords += record + "/";
                                                }
                                            }
                                        }

                                        if (!isDuplicated)
                                        {
                                            Console.Clear();
                                            if (getQuantity <= getByCode.Object.Quantity)
                                            {
                                                newRecords += getByCode.Object.Code + "&" + (getQuantity) + "/";
                                            }
                                            else
                                            {
                                                falseUpdate = true;
                                                getByCode.Log = "The quantity is out of range!";
                                            }
                                        }

                                        if (!falseUpdate)
                                        {
                                            Console.Clear();
                                            int x = cartSQLAdapter.UpdateCartRecord(accountId, newRecords);
                                            if (x >= 1)
                                            {
                                                Console.WriteLine("Add successfully!\nPress any key to go back!");
                                            }
                                            else
                                            {
                                                Console.WriteLine("Add failed!\nPress any key to go back!");
                                            }
                                            Console.ReadLine();
                                        }
                                        else
                                        {
                                            Console.Clear();
                                            Console.Write(getByCode.Log + "\nPress any key to go back!");
                                            Console.ReadLine();
                                        }
                                    }
                                    else
                                    {
                                        Console.Clear();
                                        Console.Write(getByCode.Log + "\nPress any key to go back!");
                                        Console.ReadLine();
                                    }
                                }
                            }
                            break;
                        case 3:
                            try
                            {
                                if (page == Convert.ToInt32(products.Log))
                                {
                                    Console.Clear();
                                    Console.WriteLine("This is the last page.\nPress any key to go back!");
                                    Console.ReadLine();
                                }
                                else
                                {
                                    page++;
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Some sh*t happened and I don't know why!");
                            }
                            break;
                        case 4:
                            try
                            {
                                if (page == 1)
                                {
                                    Console.Clear();
                                    Console.WriteLine("This is the first page.\nPress any key to go back!");
                                    Console.ReadLine();
                                }
                                else
                                {
                                    page--;
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Some sh*t happened and I don't know why!");
                            }
                            break;
                        case 5:
                            Console.Write("Enter page number (Press 0 or not number to go back)\n" +
                                "Value: ");
                            getPage = getValue();
                            if (getPage != 0)
                            {
                                if (getPage < 1 || getPage > Convert.ToInt32(products.Log))
                                {
                                    Console.Clear();
                                    Console.WriteLine("Page is out of range! \nPlease enter valid number.");
                                    Console.ReadLine();
                                }
                                else
                                {
                                    page = getPage;
                                }

                            }
                            break;
                        default:
                            useApp = false;
                            break;
                    }
                }
                else
                {
                    if (searchValue == "")
                    {
                        Console.WriteLine("There is no product in store.\nPress any key to go back!");
                        useApp = false;
                        Console.ReadLine();
                    }
                    else
                    {
                        Console.WriteLine("Can not find the product.\nPress any key to go back!");
                        searchValue = "";
                        Console.ReadLine();
                    }
                }
            }
        }

        public static void AdminOption()
        {
            bool useApp = true;

            while (useApp)
            {
                int value;
                Console.Clear();
                Console.WriteLine("---BookStore App---\n***");
                Console.Write("1 - Change product's info\n" +
                              "2 - Change user's status\n" +
                              "Any - Log out\n" +
                              "Value: ");
                value = getValue();
                switch (value)
                {
                    case 1:
                        ChangeProductInfo();
                        break;
                    case 2:
                        ChangeUserStatus();
                        break;
                    default:
                        useApp = false;
                        break;
                }
            }
        }

        private static void ChangeUserStatus()
        {
            bool useApp = true;
            int page = 1, getPage = 0;
            string searchValue = "", lastSearchValue = "";
            AccountSQLAdapter accountSQLAdapter = new AccountSQLAdapter();

            while (useApp)
            {
                int value, getCode;
                Console.Clear();
                Console.WriteLine("---BookStore App---\n***");
                ResponseObject<List<Account>> list = accountSQLAdapter.GetAllActiveUser(page, searchValue);
                if (list.Object != null)
                {
                    foreach (Account account in list.Object)
                    {
                        Console.WriteLine(account.Code + "/ " + account.UserName + " - Name: " + account.Name + " - Status: " + (account.IsAvailable ? "Active" : "Banned"));
                    }
                }
                else
                {
                    Console.WriteLine("There is no account!\nPress any key to go back!");
                    Console.ReadLine();
                    break;
                }

                Console.WriteLine("________________________\n" +
                    "Total page: " + list.Log + "\n" +
                    "Current search value: " + searchValue + "\n" +
                    "Current page: " + page + "\n" +
                    "________________________");

                Console.Write("1 - Search username\n" +
                              "2 - Change account's status with code\n" +
                              "3 - Next page\n" +
                              "4 - Previous page\n" +
                              "5 - Go to page\n" +
                              "Any - Log out\n" +
                              "Value: ");
                value = getValue();
                switch (value)
                {
                    case 1:
                        Console.Write("Enter username: ");
                        searchValue = Console.ReadLine(); 
                        break;
                    case 2:
                        Console.Write("Enter the code (Press 0 or not number to go back): ");
                        getCode = getValue();
                        if (getCode != 0)
                        {
                            ResponseObject<Account> getByCode = accountSQLAdapter.GetProductByCode(getCode);
                            if (getByCode.Log == null)
                            {
                                accountSQLAdapter.ChangeStatus(getByCode.Object.Code, getByCode.Object.IsAvailable);
                                Console.Clear();
                                Console.Write("Change status successfully.\nPress any key to go back!");
                                Console.ReadLine();
                            }
                            else
                            {
                                Console.Clear();
                                Console.Write(getByCode.Log + "\nPress any key to go back!");
                                Console.ReadLine();
                            }
                        }
                        break;
                    case 3:
                        try
                        {
                            if (page == Convert.ToInt32(list.Log))
                            {
                                Console.Clear();
                                Console.WriteLine("This is the last page.\nPress any key to go back!");
                                Console.ReadLine();
                            }
                            else
                            {
                                page++;
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Some sh*t happened and I don't know why!");
                        }
                        break; 
                    case 4:
                        try
                        {
                            if (page == 1)
                            {
                                Console.Clear();
                                Console.WriteLine("This is the first page.\nPress any key to go back!");
                                Console.ReadLine();
                            }
                            else
                            {
                                page--;
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Some sh*t happened and I don't know why!");
                        }
                        break;
                    case 5:
                        Console.Write("Enter page number (Press 0 or not number to go back)\n" +
                            "Value: ");
                        getPage = getValue();
                        if (getPage != 0)
                        {
                            if (getPage < 1 || getPage > Convert.ToInt32(list.Log))
                            {
                                Console.Clear();
                                Console.WriteLine("Page is out of range! \nPlease enter valid number.");
                                Console.ReadLine();
                            }
                            else
                            {
                                page = getPage;
                            }

                        }
                        break;
                    default:
                        useApp = false;
                        break;
                }
            }
        }

        private static void ChangeProductInfo()
        {

            bool useApp = true, falseUpdate = false, isDuplicated = false;
            int page = 1, lastPage = 0, getPage = 0, getCode = 0, getQuantity = 0;
            string searchValue = "", lastSearchValue = "";
            ProductSQLAdapter productSQLAdapter = new ProductSQLAdapter();
            CartSQLAdapter cartSQLAdapter = new CartSQLAdapter();
            while (useApp)
            {
                int value = 0;
                Console.Clear();
                Console.WriteLine("---BookStore App---\n***");
                ResponseObject<List<Product>> products = productSQLAdapter.GetProductsByPageAndSearchForAdmin(page, searchValue);
                if (products.Object != null)
                {
                    foreach (Product p in products.Object)
                    {
                        Console.WriteLine(p.Code + " - " + p.ProductName + " - " + p.Price + "$ /Quantity: " + p.Quantity + " Status: " + (p.IsAvailable ? "On sales" : "Stop selling"));
                    }
                }
                else Console.WriteLine("The store is currently empty!");

                Console.WriteLine("________________________\n" +
                    "Total page: " + products.Log + "\n" +
                    "Current search value: " + searchValue + "\n" +
                    "Current page: " + page + "\n" +
                    "________________________");

                Console.Write("1 - Search product\n" +
                              "2 - Add new product\n" +
                              "3 - Change product's status\n" +
                              "4 - Change product's quantity\n" +
                              "5 - Next page\n" +
                              "6 - Previous page\n" +
                              "7 - Go to page\n" +
                              "Any - Log out\n" +
                              "Value: ");
                value = getValue();
                switch (value)
                {
                    case 1:
                        if (products.Object != null)
                        {
                            Console.Write("Enter product's name: ");
                            searchValue = Console.ReadLine();
                        } else
                        {
                            Console.Clear();
                            Console.WriteLine("Can not do this function.\nPress any key to go back!");
                            Console.ReadLine();
                        }
                        break;
                    case 2:
                        Console.Clear();
                        string productName;
                        int price, quantity;
                        Console.Write("Product Name: ");
                        productName = Console.ReadLine();
                        Console.Write("Price: ");
                        price = getValue();
                        Console.Write("Quantity: ");
                        quantity = getValue();

                        if (productName.Length < 4 || price <= 0 || quantity <= 0)
                        {
                            Console.Clear();
                            if (productName.Length < 4) Console.Write("Product name length must be greater than 3.\n");
                            if (price <= 0) Console.Write("Price must be greater than 1.\n");
                            if (quantity <= 0) Console.Write("Quantity must be greater than 1.\n");
                            Console.Write("Do you want to add product again? (Press 1 to continue login, any to go back)\n" +
                                        "Value: ");
                            if (getValue() == 1)
                                useApp = true;
                            else useApp = false;
                        }
                        else
                        {
                            productSQLAdapter.AddProduct(productName, price, quantity);    
                            Console.Clear() ;
                            Console.WriteLine("Add product successfully.\nPress any key to go back!");
                            Console.ReadLine();
                        }
                        break;
                    case 3:
                        if (products.Object != null)
                        {
                            Console.Write("Enter the code (Press 0 or not number to go back): ");
                            getCode = getValue();
                            if (getCode != 0)
                            {
                                ResponseObject<Product> getByCode = productSQLAdapter.GetProductByCode(getCode);
                                if (getByCode.Log == null)
                                {
                                    productSQLAdapter.ChangeStatus(getByCode.Object.Code, getByCode.Object.IsAvailable);
                                    Console.Clear();
                                    Console.Write("Change status successfully.\nPress any key to go back!");
                                    Console.ReadLine();
                                }
                                else
                                {
                                    Console.Clear();
                                    Console.Write(getByCode.Log + "\nPress any key to go back!");
                                    Console.ReadLine();
                                }
                            }
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("Can not do this function.\nPress any key to go back!");
                            Console.ReadLine();
                        }
                        break;
                    case 4:
                        if (products.Object != null)
                        {
                            Console.Write("Enter the code (Press 0 or not number to go back): ");
                            getCode = getValue();
                            if (getCode != 0)
                            {
                                Console.Write("Enter the quantity (Press 0 or not number to go back): ");
                                getQuantity = getValue();
                                if (getQuantity != 0)
                                {
                                    if (getQuantity > 0)
                                    {
                                        ResponseObject<Product> getByCode = productSQLAdapter.GetProductByCode(getCode);
                                        if (getByCode.Log == null)
                                        {
                                            productSQLAdapter.ChangeQuantity(getByCode.Object.Code, getQuantity);
                                            Console.Clear();
                                            Console.Write("Change quantity successfully.\nPress any key to go back!");
                                            Console.ReadLine();
                                        }
                                        else
                                        {
                                            Console.Clear();
                                            Console.Write(getByCode.Log + "\nPress any key to go back!");
                                            Console.ReadLine();
                                        }
                                    }
                                    else
                                    {
                                        Console.Clear();
                                        Console.Write("Can not input negative number.\nPress any key to go back!");
                                        Console.ReadLine();
                                    }
                                }
                            }
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("Can not do this function.\nPress any key to go back!");
                            Console.ReadLine();
                        }
                        break;
                    case 5:
                        if (products.Object != null)
                        {
                            try
                            {
                                if (page == Convert.ToInt32(products.Log))
                                {
                                    Console.Clear();
                                    Console.WriteLine("This is the last page.\nPress any key to go back!");
                                    Console.ReadLine();
                                }
                                else
                                {
                                    page++;
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Some sh*t happened and I don't know why!");
                            }
                        } else
                        {
                            Console.Clear();
                            Console.WriteLine("Can not do this function.\nPress any key to go back!");
                            Console.ReadLine();
                        }

                        break;
                    case 6:
                        if (products.Object != null)
                        {
                            try
                            {
                                if (page == 1)
                                {
                                    Console.Clear();
                                    Console.WriteLine("This is the first page.\nPress any key to go back!");
                                    Console.ReadLine();
                                }
                                else
                                {
                                    page--;
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Some sh*t happened and I don't know why!");
                            }
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("Can not do this function.\nPress any key to go back!");
                            Console.ReadLine();
                        }
                        break;
                    case 7:
                        if (products.Object != null)
                        {
                            Console.Write("Enter page number (Press 0 or not number to go back)\n" +
                            "Value: ");
                            getPage = getValue();
                            if (getPage != 0)
                            {
                                if (getPage < 1 || getPage > Convert.ToInt32(products.Log))
                                {
                                    Console.Clear();
                                    Console.WriteLine("Page is out of range! \nPlease enter valid number.");
                                    Console.ReadLine();

                                }
                                else
                                {
                                    page = getPage;
                                }

                            }
                        } else
                        {
                            Console.Clear();
                            Console.WriteLine("Can not do this function.\nPress any key to go back!");
                            Console.ReadLine();
                        }
                        break;
                    default:
                        useApp = false;
                        break;
                }
            }
        }
    }
}


