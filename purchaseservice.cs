using Microsoft.EntityFrameworkCore;
using RetailManagement.Database;
using RetailManagement.Entities;

namespace RetailManagement.Business
{
    public class PurchaseService
    {
        private readonly RetailContext _context;

        public PurchaseService(RetailContext context)
        {
            _context = context;
        }

        // ----------------------------------
        // Record a Sale
        // ----------------------------------
        public bool RecordPurchase(List<(int ItemId, int Quantity)> items)
        {
            if (items == null || items.Count == 0)
                return false;

            bool itemSold = false;
            decimal total = 0;

            var purchase = new Purchase
            {
                PurchaseDate = DateTime.Now,
                TotalAmount = 0
            };

            _context.Purchases.Add(purchase);
            _context.SaveChanges();

            foreach (var order in items)
            {
                var product = _context.Items.Find(order.ItemId);

                if (product == null)
                    continue;

                if (order.Quantity <= 0)
                    continue;

                if (product.Quantity < order.Quantity)
                    continue;

                product.Quantity -= order.Quantity;
                product.StockStatus = product.Quantity <= 10 ? "Low Stock" : "Available";

                decimal lineTotal = product.Price * order.Quantity;
                total += lineTotal;

                _context.PurchaseLines.Add(new PurchaseLine
                {
                    PurchaseId = purchase.Id,
                    ItemId = product.Id,
                    Quantity = order.Quantity,
                    UnitPrice = product.Price
                });

                itemSold = true;
            }

            if (!itemSold)
            {
                _context.Purchases.Remove(purchase);
                _context.SaveChanges();
                return false;
            }

            purchase.TotalAmount = total;
            _context.SaveChanges();

            return true;
        }

        // ----------------------------------
        // View All Purchases
        // ----------------------------------
        public List<Purchase> GetAllPurchases()
        {
            return _context.Purchases
                .Include(p => p.PurchaseLines)
                .OrderByDescending(p => p.PurchaseDate)
                .ToList();
        }

        // ----------------------------------
        // View Purchase Details
        // ----------------------------------
        public Purchase GetPurchase(int id)
        {
            return _context.Purchases
                .Include(p => p.PurchaseLines)
                .FirstOrDefault(p => p.Id == id);
        }

        // ----------------------------------
        // Total Sales
        // ----------------------------------
        public decimal GetTotalSales()
        {
            if (!_context.Purchases.Any())
                return 0;

            return _context.Purchases.Sum(p => p.TotalAmount);
        }

        // ----------------------------------
        // Sales By Product
        // ----------------------------------
        public List<SalesReport> GetSalesByProduct()
        {
            return _context.PurchaseLines
                .Include(pl => pl.Item)
                .GroupBy(pl => pl.Item.Name)
                .Select(g => new SalesReport
                {
                    ProductName = g.Key,
                    QuantitySold = g.Sum(x => x.Quantity),
                    TotalSales = g.Sum(x => x.Quantity * x.UnitPrice)
                })
                .OrderByDescending(x => x.TotalSales)
                .ToList();
        }
    }

    // ----------------------------------
    // Report Model
    // ----------------------------------
    public class SalesReport
    {
        public string ProductName { get; set; }

        public int QuantitySold { get; set; }

        public decimal TotalSales { get; set; }
    }
}