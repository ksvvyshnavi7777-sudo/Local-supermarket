using RetailManagement.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RetailManagement.Entities
{
    public class Purchase
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime PurchaseDate { get; set; }

        [Required]
        public decimal TotalAmount { get; set; }

        public virtual ICollection<PurchaseLine> PurchaseLines { get; set; }

        public Purchase()
        {
            PurchaseDate = DateTime.Now;
            PurchaseLines = new List<PurchaseLine>();
        }

        public Purchase(decimal totalAmount)
        {
            PurchaseDate = DateTime.Now;
            TotalAmount = totalAmount;
            PurchaseLines = new List<PurchaseLine>();
        }

        public override string ToString()
        {
            return $"Purchase #{Id} | {PurchaseDate:dd/MM/yyyy HH:mm} | Total: £{TotalAmount:F2}";
        }
    }
}