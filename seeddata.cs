using System;
using System.Linq;
using RetailManagement.Entities;

namespace RetailManagement.Database
{
    public static class SeedData
    {
        public static void Initialize(RetailContext context)
        {
            // Database already contains data
            if (context.Items.Any())
                return;

            // -------------------------
            // Departments
            // -------------------------

            var drinks = new Department
            {
                DepartmentName = "Drinks",
                Description = "Soft Drinks, Water and Juice"
            };

            var snacks = new Department
            {
                DepartmentName = "Snacks",
                Description = "Crisps, Biscuits and Chocolate"
            };

            var dairy = new Department
            {
                DepartmentName = "Dairy",
                Description = "Milk, Cheese and Butter"
            };

            context.Departments.AddRange(drinks, snacks, dairy);
            context.SaveChanges();

            // -------------------------
            // Vendors
            // -------------------------

            var vendor1 = new Vendor
            {
                CompanyName = "Fresh Wholesale",
                ContactPerson = "James Smith",
                Phone = "07111111111",
                Email = "sales@freshwholesale.co.uk"
            };

            var vendor2 = new Vendor
            {
                CompanyName = "UK Food Supplies",
                ContactPerson = "Sarah Jones",
                Phone = "07222222222",
                Email = "contact@ukfoods.co.uk"
            };

            context.Vendors.AddRange(vendor1, vendor2);
            context.SaveChanges();

            // -------------------------
            // Products
            // -------------------------

            context.Items.AddRange(

                new Item
                {
                    Barcode = "100001",
                    Name = "Orange Juice",
                    Brand = "SunFresh",
                    Price = 2.50m,
                    Quantity = 30,
                    RestockDate = DateTime.Today.AddDays(15),
                    StockStatus = "Available",
                    DepartmentId = drinks.Id,
                    VendorId = vendor1.Id
                },

                new Item
                {
                    Barcode = "100002",
                    Name = "Potato Chips",
                    Brand = "Crunch",
                    Price = 1.75m,
                    Quantity = 8,
                    RestockDate = DateTime.Today.AddDays(4),
                    StockStatus = "Low Stock",
                    DepartmentId = snacks.Id,
                    VendorId = vendor2.Id
                },

                new Item
                {
                    Barcode = "100003",
                    Name = "Whole Milk",
                    Brand = "Farm Fresh",
                    Price = 1.35m,
                    Quantity = 25,
                    RestockDate = DateTime.Today.AddDays(7),
                    StockStatus = "Available",
                    DepartmentId = dairy.Id,
                    VendorId = vendor1.Id
                }

            );

            context.SaveChanges();
        }
    }
}