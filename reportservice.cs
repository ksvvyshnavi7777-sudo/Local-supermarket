using Microsoft.EntityFrameworkCore;
using RetailManagement.Database;
using RetailManagement.Entities;

namespace RetailManagement.Business
{
    public class ReportService
    {
        private readonly RetailContext _context;

        public ReportService(RetailContext context)
        {
            _context = context;
        }

        // ---------------------------------------
        // Low Stock Report
        // ---------------------------------------
        public List<Item> GetLowStockReport(int minimumStock = 10)
        {
            return _context.Items
                .Include(i => i.Department)
                .Include(i => i.Vendor)
                .Where(i => i.Quantity <= minimumStock)
                .OrderBy(i => i.Quantity)
                .ToList();
        }

        // ---------------------------------------
        // Products by Category
        // ---------------------------------------
        public List<CategoryReport> GetProductsByCategory()
        {
            return _context.Departments
                .Include(d => d.Items)
                .Select(d => new CategoryReport
                {
                    CategoryName = d.DepartmentName,
                    ProductCount = d.Items.Count
                })
                .OrderBy(r => r.CategoryName)
                .ToList();
        }

        // ---------------------------------------
        // Supplier Stock Report
        // ---------------------------------------
        public List<SupplierStockReport> GetSupplierStockReport()
        {
            return _context.Vendors
                .Include(v => v.Items)
                .Select(v => new SupplierStockReport
                {
                    SupplierName = v.CompanyName,
                    NumberOfProducts = v.Items.Count,
                    TotalStock = v.Items.Sum(i => i.Quantity)
                })
                .OrderBy(r => r.SupplierName)
                .ToList();
        }

        // ---------------------------------------
        // Sales by Product
        // ---------------------------------------
        public List<SalesProductReport> GetSalesByProductReport()
        {
            return _context.PurchaseLines
                .Include(pl => pl.Item)
                .GroupBy(pl => pl.Item.Name)
                .Select(g => new SalesProductReport
                {
                    ProductName = g.Key,
                    QuantitySold = g.Sum(x => x.Quantity),
                    TotalRevenue = g.Sum(x => x.Quantity * x.UnitPrice)
                })
                .OrderByDescending(r => r.TotalRevenue)
                .ToList();
        }

        // ---------------------------------------
        // Dashboard Statistics
        // ---------------------------------------
        public DashboardReport GetDashboard()
        {
            return new DashboardReport
            {
                TotalProducts = _context.Items.Count(),
                TotalSuppliers = _context.Vendors.Count(),
                TotalPurchases = _context.Purchases.Count(),
                TotalSales = _context.Purchases.Sum(p => (decimal?)p.TotalAmount) ?? 0
            };
        }
    }

    // ---------------------------------------
    // Report Models
    // ---------------------------------------

    public class CategoryReport
    {
        public string CategoryName { get; set; }
        public int ProductCount { get; set; }
    }

    public class SupplierStockReport
    {
        public string SupplierName { get; set; }
        public int NumberOfProducts { get; set; }
        public int TotalStock { get; set; }
    }

    public class SalesProductReport
    {
        public string ProductName { get; set; }
        public int QuantitySold { get; set; }
        public decimal TotalRevenue { get; set; }
    }

    public class DashboardReport
    {
        public int TotalProducts { get; set; }
        public int TotalSuppliers { get; set; }
        public int TotalPurchases { get; set; }
        public decimal TotalSales { get; set; }
    }
}