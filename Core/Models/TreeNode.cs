using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
	public class TreeNode
	{
		private string? _route;

		public int Id { get; set; }
		public string FileName { get; set; }
		public string Route { get => _route ??= GetRoute(); }
		public FileType FileTreeType { get; set; }
		public long? FileSize { get; set; }
		public double PercentFileSize { get; set; }

		public TreeNode? Parent { get; set; }
		public ConcurrentBag<TreeNode>? Childrens { get; set; }

		private string GetRoute()
		{
			return Parent is null ? FileName : Parent.Route + "/" + FileName;
		}
	}

	public enum FileType
	{
		Folder,
		File
	}
}
