using Microsoft.EntityFrameworkCore;
using RetailManagement.Business;
using RetailManagement.Database;
using RetailManagement.Entities;

namespace RetailManagement
{
    internal class Program
    {
        static RetailContext context;
        static ItemService itemService;
        static VendorService vendorService;
        static PurchaseService purchaseService;
        static ReportService reportService;

        static void Main(string[] args)
        {
            var Options = new DbContextOptionsBuilder<RetailContext>()
     .UseInMemoryDatabase("RetailManagementDB")
     .Options;
            context = new RetailContext(Options);

            context.Database.EnsureCreated();

            SeedData.Initialize(context);

            itemService = new ItemService(context);
            vendorService = new VendorService(context);
            purchaseService = new PurchaseService(context);
            reportService = new ReportService(context);

            MainMenu();
        }

        static void MainMenu()
        {
            while (true)
            {
                Console.Clear();

                Console.WriteLine("=========================================");
                Console.WriteLine(" LOCAL SUPERMARKET MANAGEMENT SYSTEM");
                Console.WriteLine("=========================================");
                Console.WriteLine("1. Product Management");
                Console.WriteLine("2. Vendor Management");
                Console.WriteLine("3. Sales");
                Console.WriteLine("4. Reports");
                Console.WriteLine("0. Exit");

                Console.Write("\nChoice: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        ProductMenu();
                        break;

                    case "2":
                        VendorMenu();
                        break;

                    case "3":
                        SalesMenu();
                        break;

                    case "4":
                        ReportMenu();
                        break;

                    case "0":
                        return;

                    default:
                        Console.WriteLine("Invalid Choice.");
                        Pause();
                        break;
                }
            }
        }

        static void ProductMenu()
        {
            while (true)
            {
                Console.Clear();

                Console.WriteLine("========== PRODUCT MANAGEMENT ==========");
                Console.WriteLine("1. Add Product");
                Console.WriteLine("2. View Products");
                Console.WriteLine("3. Search By Name");
                Console.WriteLine("4. Search By Barcode");
                Console.WriteLine("5. Delete Product");
                Console.WriteLine("6. Low Stock Report");
                Console.WriteLine("0. Back");

                Console.Write("\nChoice: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        AddProduct();
                        break;

                    case "2":
                        ViewProducts();
                        break;

                    case "3":
                        SearchByName();
                        break;

                    case "4":
                        SearchByBarcode();
                        break;

                    case "5":
                        DeleteProduct();
                        break;

                    case "6":
                        LowStockReport();
                        break;

                    case "0":
                        return;
                }
            }
        }

        static void AddProduct()
        {
            Console.Clear();

            Item item = new Item();

            Console.Write("Barcode: ");
            item.Barcode = Console.ReadLine();

            Console.Write("Name: ");
            item.Name = Console.ReadLine();

            Console.Write("Brand: ");
            item.Brand = Console.ReadLine();

            Console.Write("Price: ");
            item.Price = decimal.Parse(Console.ReadLine());

            Console.Write("Quantity: ");
            item.Quantity = int.Parse(Console.ReadLine());

            Console.Write("Department ID: ");
            item.DepartmentId = int.Parse(Console.ReadLine());

            Console.Write("Vendor ID: ");
            item.VendorId = int.Parse(Console.ReadLine());

            item.RestockDate = DateTime.Today.AddDays(7);

            if (itemService.AddItem(item))
                Console.WriteLine("\nProduct Added Successfully.");
            else
                Console.WriteLine("\nUnable to Add Product.");

            Pause();
        }

        static void ViewProducts()
        {
            Console.Clear();

            var items = itemService.GetAllItems();

            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("ID   Name                 Barcode      Price      Qty");
            Console.WriteLine("---------------------------------------------------------------");

            foreach (var item in items)
            {
                Console.WriteLine($"{item.Id,-5}{item.Name,-20}{item.Barcode,-12}£{item.Price,-10:F2}{item.Quantity}");
            }

            Pause();
        }

        static void SearchByName()
        {
            Console.Clear();

            Console.Write("Enter Product Name: ");
            string name = Console.ReadLine();

            var item = itemService.SearchByName(name);

            if (item == null)
            {
                Console.WriteLine("\nProduct Not Found.");
            }
            else
            {
                Console.WriteLine("\nProduct Found");
                Console.WriteLine("--------------------------------");
                Console.WriteLine($"ID       : {item.Id}");
                Console.WriteLine($"Barcode  : {item.Barcode}");
                Console.WriteLine($"Name     : {item.Name}");
                Console.WriteLine($"Brand    : {item.Brand}");
                Console.WriteLine($"Price    : £{item.Price:F2}");
                Console.WriteLine($"Quantity : {item.Quantity}");
                Console.WriteLine($"Status   : {item.StockStatus}");
            }

            Pause();
        }

        static void SearchByBarcode()
        {
            Console.Clear();

            Console.Write("Enter Barcode: ");
            string barcode = Console.ReadLine();

            var item = itemService.SearchByBarcode(barcode);

            if (item == null)
            {
                Console.WriteLine("\nProduct Not Found.");
            }
            else
            {
                Console.WriteLine("\nProduct Found");
                Console.WriteLine("--------------------------------");
                Console.WriteLine($"ID       : {item.Id}");
                Console.WriteLine($"Barcode  : {item.Barcode}");
                Console.WriteLine($"Name     : {item.Name}");
                Console.WriteLine($"Brand    : {item.Brand}");
                Console.WriteLine($"Price    : £{item.Price:F2}");
                Console.WriteLine($"Quantity : {item.Quantity}");
                Console.WriteLine($"Status   : {item.StockStatus}");
            }

            Pause();
        }

        static void DeleteProduct()
        {
            Console.Clear();

            Console.Write("Enter Product ID: ");

            int id = int.Parse(Console.ReadLine());

            if (itemService.DeleteItem(id))
                Console.WriteLine("\nProduct Deleted.");
            else
                Console.WriteLine("\nProduct Not Found.");

            Pause();
        }

        static void LowStockReport()
        {
            Console.Clear();

            var items = itemService.GetLowStockItems();

            Console.WriteLine("LOW STOCK ITEMS");
            Console.WriteLine("--------------------------");

            foreach (var item in items)
            {
                Console.WriteLine($"{item.Name,-20} Qty: {item.Quantity}");
            }

            Pause();
        }

        static void Pause()
        {
            Console.WriteLine("\nPress ENTER to continue...");
            Console.ReadLine();
        }
        static void VendorMenu()
        {
            while (true)
            {
                Console.Clear();

                Console.WriteLine("========== VENDOR MANAGEMENT ==========");
                Console.WriteLine("1. View Vendors");
                Console.WriteLine("2. Add Vendor");
                Console.WriteLine("3. Delete Vendor");
                Console.WriteLine("0. Back");

                Console.Write("\nChoice: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        ViewVendors();
                        break;

                    case "2":
                        AddVendor();
                        break;

                    case "3":
                        DeleteVendor();
                        break;

                    case "0":
                        return;
                }
            }
        }

        static void ViewVendors()
        {
            Console.Clear();

            var vendors = vendorService.GetAllVendors();

            Console.WriteLine("========== VENDORS ==========\n");

            foreach (var vendor in vendors)
            {
                Console.WriteLine($"ID      : {vendor.Id}");
                Console.WriteLine($"Company : {vendor.CompanyName}");
                Console.WriteLine($"Contact : {vendor.ContactPerson}");
                Console.WriteLine($"Phone   : {vendor.Phone}");
                Console.WriteLine($"Email   : {vendor.Email}");
                Console.WriteLine("--------------------------------------");
            }

            Pause();
        }

        static void AddVendor()
        {
            Console.Clear();

            Vendor vendor = new Vendor();

            Console.Write("Company Name : ");
            vendor.CompanyName = Console.ReadLine();

            Console.Write("Contact Name : ");
            vendor.ContactPerson = Console.ReadLine();

            Console.Write("Phone        : ");
            vendor.Phone = Console.ReadLine();

            Console.Write("Email        : ");
            vendor.Email = Console.ReadLine();

            if (vendorService.AddVendor(vendor))
                Console.WriteLine("\nVendor Added Successfully.");
            else
                Console.WriteLine("\nUnable to Add Vendor.");

            Pause();
        }

        static void DeleteVendor()
        {
            Console.Clear();

            Console.Write("Vendor ID: ");

            int id = int.Parse(Console.ReadLine());

            if (vendorService.DeleteVendor(id))
                Console.WriteLine("\nVendor Deleted.");
            else
                Console.WriteLine("\nVendor Could Not Be Deleted.");

            Pause();
        }

        static void SalesMenu()
        {
            while (true)
            {
                Console.Clear();

                Console.WriteLine("============== SALES ==============");
                Console.WriteLine("1. Record Sale");
                Console.WriteLine("2. View Sales");
                Console.WriteLine("0. Back");

                Console.Write("\nChoice: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        RecordSale();
                        break;

                    case "2":
                        ViewSales();
                        break;

                    case "0":
                        return;
                }
            }
        }

        static void RecordSale()
        {
            Console.Clear();

            Console.Write("Product ID: ");
            int productId = int.Parse(Console.ReadLine());

            Console.Write("Quantity: ");
            int quantity = int.Parse(Console.ReadLine());

            var order = new List<(int ItemId, int Quantity)>
            {
                (productId, quantity)
            };

            if (purchaseService.RecordPurchase(order))
                Console.WriteLine("\nSale Recorded Successfully.");
            else
                Console.WriteLine("\nSale Failed.");

            Pause();
        }

        static void ViewSales()
        {
            Console.Clear();

            var sales = purchaseService.GetAllPurchases();

            Console.WriteLine("============= SALES =============\n");

            foreach (var sale in sales)
            {
                Console.WriteLine($"Sale ID : {sale.Id}");
                Console.WriteLine($"Date    : {sale.PurchaseDate}");
                Console.WriteLine($"Total   : £{sale.TotalAmount:F2}");
                Console.WriteLine("--------------------------------------");
            }

            Pause();
        }

        static void ReportMenu()
        {
            while (true)
            {
                Console.Clear();

                Console.WriteLine("============== REPORTS ==============");
                Console.WriteLine("1. Low Stock");
                Console.WriteLine("2. Products By Category");
                Console.WriteLine("3. Supplier Stock");
                Console.WriteLine("4. Sales By Product");
                Console.WriteLine("5. Dashboard");
                Console.WriteLine("0. Back");

                Console.Write("\nChoice: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        LowStockReport();
                        break;

                    case "2":
                        ShowProductsByCategory();
                        break;

                    case "3":
                        ShowSupplierStock();
                        break;

                    case "4":
                        ShowSalesByProduct();
                        break;

                    case "5":
                        ShowDashboard();
                        break;

                    case "0":
                        return;
                }
            }
        }

        static void ShowProductsByCategory()
        {
            Console.Clear();

            foreach (var report in reportService.GetProductsByCategory())
            {
                Console.WriteLine($"{report.CategoryName,-20} {report.ProductCount} Products");
            }

            Pause();
        }

        static void ShowSupplierStock()
        {
            Console.Clear();

            foreach (var report in reportService.GetSupplierStockReport())
            {
                Console.WriteLine($"{report.SupplierName,-25} Products: {report.NumberOfProducts,-5} Stock: {report.TotalStock}");
            }

            Pause();
        }

        static void ShowSalesByProduct()
        {
            Console.Clear();

            foreach (var report in reportService.GetSalesByProductReport())
            {
                Console.WriteLine($"{report.ProductName,-20} Qty: {report.QuantitySold,-5} Revenue: £{report.TotalRevenue:F2}");
            }

            Pause();
        }

        static void ShowDashboard()
        {
            Console.Clear();

            var dashboard = reportService.GetDashboard();

            Console.WriteLine("========== DASHBOARD ==========\n");
            Console.WriteLine($"Total Products  : {dashboard.TotalProducts}");
            Console.WriteLine($"Total Vendors   : {dashboard.TotalSuppliers}");
            Console.WriteLine($"Total Sales     : {dashboard.TotalPurchases}");
            Console.WriteLine($"Total Revenue   : £{dashboard.TotalSales:F2}");

            Pause();
        }
    }
}