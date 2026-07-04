using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RetailManagement.Entities
{
    public class PurchaseLine
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PurchaseId { get; set; }

        [ForeignKey(nameof(PurchaseId))]
        public Purchase Purchase { get; set; }

        [Required]
        public int ItemId { get; set; }

        [ForeignKey(nameof(ItemId))]
        public Item Item { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal UnitPrice { get; set; }

        [NotMapped]
        public decimal LineTotal => Quantity * UnitPrice;

        public PurchaseLine()
        {
        }

        public PurchaseLine(int itemId, int quantity, decimal unitPrice)
        {
            ItemId = itemId;
            Quantity = quantity;
            UnitPrice = unitPrice;
        }

        public override string ToString()
        {
            return $"Item ID: {ItemId} | Qty: {Quantity} | Unit Price: £{UnitPrice:F2} | Total: £{LineTotal:F2}";
        }
    }
}