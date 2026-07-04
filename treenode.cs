using RetailManagement.Entities;

namespace RetailManagement.Structure
{
    public class TreeNode
    {
        public Item Data { get; set; }

        public TreeNode Left { get; set; }

        public TreeNode Right { get; set; }

        public TreeNode(Item item)
        {
            Data = item;
            Left = null;
            Right = null;
        }
    }
}