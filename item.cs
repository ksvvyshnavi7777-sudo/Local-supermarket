using RetailManagement.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RetailManagement.Entities
{
    public class Item
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        public string Barcode { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string Brand { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public DateTime RestockDate { get; set; }

        [Required]
        [StringLength(30)]
        public string StockStatus { get; set; }

        [ForeignKey("Department")]
        public int DepartmentId { get; set; }

        public Department Department { get; set; }

        [ForeignKey("Vendor")]
        public int VendorId { get; set; }

        public Vendor Vendor { get; set; }

        public Item()
        {
        }

        public Item(
            string barcode,
            string name,
            string brand,
            decimal price,
            int quantity,
            DateTime restockDate,
            string stockStatus,
            int departmentId,
            int vendorId)
        {
            Barcode = barcode;
            Name = name;
            Brand = brand;
            Price = price;
            Quantity = quantity;
            RestockDate = restockDate;
            StockStatus = stockStatus;
            DepartmentId = departmentId;
            VendorId = vendorId;
        }

        public override string ToString()
        {
            return $"{Id,-5} {Barcode,-12} {Name,-20} {Brand,-15} £{Price,-8:F2} Qty:{Quantity}";
        }
    }
}