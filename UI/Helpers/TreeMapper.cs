using Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace UI.Helpers
{
    internal static class TreeMapper
    {
        public static TreeViewNode ToTreeViewNode(this TreeNode treeNode)
        {
            var viewNode = new TreeViewNode
            {
                FileName = treeNode.FileName,
                FileSize = treeNode.FileSize.GetValueOrDefault(0),
                PercentFileSize = treeNode.PercentFileSize,
                PhotoPath = TreeViewNode.GetStringPath(treeNode.FileTreeType)
            };

            if (treeNode.Childrens != null)
            {
                viewNode.TreeViewNodes = new ObservableCollection<TreeViewNode>();

                foreach (var node in treeNode.Childrens)
                {
                    var childNode = node.ToTreeViewNode();
                    viewNode.TreeViewNodes.Add(childNode);
                }
            }
            return viewNode;
        }
    }
}
