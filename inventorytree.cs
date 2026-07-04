using System;
using RetailManagement.Entities;

namespace RetailManagement.Structure
{
    public class InventoryTree
    {
        private TreeNode root;

        public InventoryTree()
        {
            root = null;
        }

        // Insert a product into the Binary Search Tree
        public void AddItem(Item item)
        {
            root = Insert(root, item);
        }

        private TreeNode Insert(TreeNode node, Item item)
        {
            if (node == null)
                return new TreeNode(item);

            int comparison = string.Compare(
                item.Name,
                node.Data.Name,
                StringComparison.OrdinalIgnoreCase);

            if (comparison < 0)
            {
                node.Left = Insert(node.Left, item);
            }
            else if (comparison > 0)
            {
                node.Right = Insert(node.Right, item);
            }

            return node;
        }

        // Search by Product Name (BST Search)
        public Item FindByName(string name)
        {
            return Search(root, name);
        }

        private Item Search(TreeNode node, string name)
        {
            if (node == null)
                return null;

            int comparison = string.Compare(
                name,
                node.Data.Name,
                StringComparison.OrdinalIgnoreCase);

            if (comparison == 0)
                return node.Data;

            if (comparison < 0)
                return Search(node.Left, name);

            return Search(node.Right, name);
        }

        // Display all products in alphabetical order
        public void DisplayAll()
        {
            Console.WriteLine();
            Console.WriteLine("=========== PRODUCT LIST ===========");
            InOrder(root);
        }

        private void InOrder(TreeNode node)
        {
            if (node == null)
                return;

            InOrder(node.Left);

            Console.WriteLine(
                $"{node.Data.Id,-5}" +
                $"{node.Data.Name,-20}" +
                $"{node.Data.Barcode,-12}" +
                $"£{node.Data.Price,-8:F2}" +
                $"Qty: {node.Data.Quantity}");

            InOrder(node.Right);
        }

        // Low Stock Report
        public void ShowLowStock(int minimumStock)
        {
            Console.WriteLine();
            Console.WriteLine("========== LOW STOCK ITEMS ==========");

            bool found = false;
            DisplayLowStock(root, minimumStock, ref found);

            if (!found)
            {
                Console.WriteLine("No low stock items found.");
            }
        }

        private void DisplayLowStock(TreeNode node, int minimumStock, ref bool found)
        {
            if (node == null)
                return;

            DisplayLowStock(node.Left, minimumStock, ref found);

            if (node.Data.Quantity <= minimumStock)
            {
                found = true;

                Console.WriteLine(
                    $"{node.Data.Name,-20}" +
                    $"Stock: {node.Data.Quantity,-5}" +
                    $"Barcode: {node.Data.Barcode}");
            }

            DisplayLowStock(node.Right, minimumStock, ref found);
        }

        // Clear tree (used after updates/deletes)
        public void Clear()
        {
            root = null;
        }
    }
}