using Microsoft.EntityFrameworkCore;
using RetailManagement.Database;
using RetailManagement.Entities;
using RetailManagement.Structure;

namespace RetailManagement.Business
{
    public class ItemService
    {
        private readonly RetailContext _context;
        private readonly InventoryTree _tree;

        public ItemService(RetailContext context)
        {
            _context = context;
            _tree = new InventoryTree();

            LoadTree();
        }

        private void LoadTree()
        {
            _tree.Clear();

            foreach (var item in _context.Items.AsNoTracking().ToList())
            {
                _tree.AddItem(item);
            }
        }

        // --------------------------
        // CREATE
        // --------------------------

        public bool AddItem(Item item)
        {
            if (!ValidateItem(item))
                return false;

            if (_context.Items.Any(i => i.Barcode == item.Barcode))
                return false;

            item.StockStatus = item.Quantity <= 10 ? "Low Stock" : "Available";

            _context.Items.Add(item);
            _context.SaveChanges();

            _tree.AddItem(item);

            return true;
        }

        // --------------------------
        // READ
        // --------------------------

        public List<Item> GetAllItems()
        {
            return _context.Items
                .Include(i => i.Department)
                .Include(i => i.Vendor)
                .OrderBy(i => i.Name)
                .ToList();
        }

        public Item GetById(int id)
        {
            return _context.Items
                .Include(i => i.Department)
                .Include(i => i.Vendor)
                .FirstOrDefault(i => i.Id == id);
        }

        // Binary Search Tree
        public Item SearchByName(string name)
        {
            return _tree.FindByName(name);
        }

        // Linear Search
        public Item SearchByBarcode(string barcode)
        {
            foreach (var item in _context.Items)
            {
                if (item.Barcode == barcode)
                    return item;
            }

            return null;
        }

        // --------------------------
        // UPDATE
        // --------------------------

        public bool UpdateItem(Item updatedItem)
        {
            var item = _context.Items.Find(updatedItem.Id);

            if (item == null)
                return false;

            if (!ValidateItem(updatedItem))
                return false;

            bool barcodeExists = _context.Items.Any(i =>
                i.Barcode == updatedItem.Barcode &&
                i.Id != updatedItem.Id);

            if (barcodeExists)
                return false;

            item.Barcode = updatedItem.Barcode;
            item.Name = updatedItem.Name;
            item.Brand = updatedItem.Brand;
            item.Price = updatedItem.Price;
            item.Quantity = updatedItem.Quantity;
            item.RestockDate = updatedItem.RestockDate;
            item.DepartmentId = updatedItem.DepartmentId;
            item.VendorId = updatedItem.VendorId;
            item.StockStatus = updatedItem.Quantity <= 10
                ? "Low Stock"
                : "Available";

            _context.SaveChanges();

            LoadTree();

            return true;
        }

        // --------------------------
        // DELETE
        // --------------------------

        public bool DeleteItem(int id)
        {
            var item = _context.Items.Find(id);

            if (item == null)
                return false;

            _context.Items.Remove(item);
            _context.SaveChanges();

            LoadTree();

            return true;
        }

        // --------------------------
        // REPORTS
        // --------------------------

        public List<Item> GetLowStockItems(int minimumStock = 10)
        {
            return _context.Items
                .Where(i => i.Quantity <= minimumStock)
                .OrderBy(i => i.Quantity)
                .ToList();
        }

        // --------------------------
        // STOCK
        // --------------------------

        public bool UpdateStock(int itemId, int quantitySold)
        {
            var item = _context.Items.Find(itemId);

            if (item == null)
                return false;

            if (quantitySold <= 0)
                return false;

            if (item.Quantity < quantitySold)
                return false;

            item.Quantity -= quantitySold;

            item.StockStatus = item.Quantity <= 10
                ? "Low Stock"
                : "Available";

            _context.SaveChanges();

            LoadTree();

            return true;
        }

        // --------------------------
        // VALIDATION
        // --------------------------

        private bool ValidateItem(Item item)
        {
            if (string.IsNullOrWhiteSpace(item.Name))
                return false;

            if (string.IsNullOrWhiteSpace(item.Barcode))
                return false;

            if (string.IsNullOrWhiteSpace(item.Brand))
                return false;

            if (item.Price <= 0)
                return false;

            if (item.Quantity < 0)
                return false;

            if (item.DepartmentId <= 0)
                return false;

            if (item.VendorId <= 0)
                return false;

            return true;
        }
    }
}