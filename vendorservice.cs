using Microsoft.EntityFrameworkCore;
using RetailManagement.Database;
using RetailManagement.Entities;

namespace RetailManagement.Business
{
    public class VendorService
    {
        private readonly RetailContext _context;

        public VendorService(RetailContext context)
        {
            _context = context;
        }

        // --------------------------
        // CREATE
        // --------------------------

        public bool AddVendor(Vendor vendor)
        {
            if (!ValidateVendor(vendor))
                return false;

            bool exists = _context.Vendors.Any(v =>
                v.CompanyName.ToLower() == vendor.CompanyName.ToLower());

            if (exists)
                return false;

            _context.Vendors.Add(vendor);
            _context.SaveChanges();

            return true;
        }

        // --------------------------
        // READ
        // --------------------------

        public List<Vendor> GetAllVendors()
        {
            return _context.Vendors
                .Include(v => v.Items)
                .OrderBy(v => v.CompanyName)
                .ToList();
        }

        public Vendor GetVendorById(int id)
        {
            return _context.Vendors
                .Include(v => v.Items)
                .FirstOrDefault(v => v.Id == id);
        }

        // --------------------------
        // UPDATE
        // --------------------------

        public bool UpdateVendor(Vendor updatedVendor)
        {
            var vendor = _context.Vendors.Find(updatedVendor.Id);

            if (vendor == null)
                return false;

            if (!ValidateVendor(updatedVendor))
                return false;

            bool duplicate = _context.Vendors.Any(v =>
                v.CompanyName.ToLower() == updatedVendor.CompanyName.ToLower()
                && v.Id != updatedVendor.Id);

            if (duplicate)
                return false;

            vendor.CompanyName = updatedVendor.CompanyName;
            vendor.ContactPerson = updatedVendor.ContactPerson;
            vendor.Phone = updatedVendor.Phone;
            vendor.Email = updatedVendor.Email;

            _context.SaveChanges();

            return true;
        }

        // --------------------------
        // DELETE
        // --------------------------

        public bool DeleteVendor(int id)
        {
            var vendor = _context.Vendors
                .Include(v => v.Items)
                .FirstOrDefault(v => v.Id == id);

            if (vendor == null)
                return false;

            // Don't allow deleting vendors that still supply products
            if (vendor.Items.Any())
                return false;

            _context.Vendors.Remove(vendor);
            _context.SaveChanges();

            return true;
        }

        // --------------------------
        // REPORT
        // --------------------------

        public List<Item> GetVendorProducts(int vendorId)
        {
            return _context.Items
                .Where(i => i.VendorId == vendorId)
                .OrderBy(i => i.Name)
                .ToList();
        }

        // --------------------------
        // VALIDATION
        // --------------------------

        private bool ValidateVendor(Vendor vendor)
        {
            if (string.IsNullOrWhiteSpace(vendor.CompanyName))
                return false;

            if (string.IsNullOrWhiteSpace(vendor.ContactPerson))
                return false;

            if (string.IsNullOrWhiteSpace(vendor.Phone))
                return false;

            if (string.IsNullOrWhiteSpace(vendor.Email))
                return false;

            return true;
        }
    }
}