using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RetailManagement.Entities
{
    public class Vendor
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string CompanyName { get; set; }

        [Required]
        [StringLength(100)]
        public string ContactPerson { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public virtual ICollection<Item> Items { get; set; }

        public Vendor()
        {
            Items = new List<Item>();
        }

        public Vendor(string companyName, string contactPerson, string phone, string email)
        {
            CompanyName = companyName;
            ContactPerson = contactPerson;
            Phone = phone;
            Email = email;
            Items = new List<Item>();
        }

        public override string ToString()
        {
            return $"{Id} - {CompanyName} ({ContactPerson})";
        }
    }
}