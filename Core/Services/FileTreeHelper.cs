using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
	public class FileTreeHelper
	{

		public static void RecalculateFileSizes(TreeNode node)
		{
			if (!node.FileSize.HasValue)
				node.FileSize = GetChildFileSize(node);
		}

		private static long GetChildFileSize(TreeNode node)
		{
			long size = 0;
			if (node.Childrens != null)
			{
				foreach (var child in node.Childrens)
				{
					RecalculateFileSizes(child);
					size += child.FileSize.Value;
				}
			}

			return size;
		}

		public static void RecalculatePercentFileSize(TreeNode node)
		{
			var div = node.FileSize.Value;
			if (node.Parent != null)
				div = node.Parent.FileSize.Value;

			node.PercentFileSize = (double)node.FileSize.Value / div;

			if (node.Childrens != null)
				foreach (var child in node.Childrens)
					RecalculatePercentFileSize(child);
		}
	}
}
