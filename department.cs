using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RetailManagement.Entities
{
    public class Department
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string DepartmentName { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        public virtual ICollection<Item> Items { get; set; }

        public Department()
        {
            Items = new List<Item>();
        }

        public Department(string departmentName, string description)
        {
            DepartmentName = departmentName;
            Description = description;
            Items = new List<Item>();
        }

        public override string ToString()
        {
            return $"{Id} - {DepartmentName}";
        }
    }
}