using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TksHelpers
{
    public class TreeNode : TreeViewItem
    {
        public int Level { get; set; }
        public Brush ForeGround { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        public void AddToLast(TreeNode node)
        {
            if (node.Level == 0 || node.Level - 1 == Level)
            {
                Items.Add(node);
                return;
            }

            Items.Cast<TreeNode>().Last().AddToLast(node);
        }

        public override string ToString()
        {
            var str = Header.ToString().Indent(Level) + "\n";
            return Items.Cast<object>().Aggregate(str, (current, item) => current + item.ToString());
        }

        public TreeViewItem AsTreeViewItem()
        {
            var t = new TreeViewItem { Header = new Label() { Content = Header, Foreground = ForeGround ?? Brushes.Black, Padding = new Thickness(0, 0, 0, 0) } };
            if (Items.IsEmpty)
                return t;
            var it = (from object item in Items select (item as TreeNode).AsTreeViewItem()).ToList();

            foreach (var treeViewItem in it.Where(treeViewItem => !t.Items.Contains(treeViewItem)))
            {
                t.Items.Add(treeViewItem);
            }
            return t;
        }
    }

    public static class TreeViewExtension
    {
        public static void GenerateTree(this TreeView treeView, TreeNode tree)
        {
            var items = tree.AsTreeViewItem();
            treeView.Items.Add(items);
        }
    }
}