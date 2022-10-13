using Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI
{
	public class TreeViewNode
	{
		public string FileName { get; set; }
		public long FileSize { get; set; }
		public double PercentFileSize { get; set; }
		public string PhotoPath { get; set; }

		public ObservableCollection<TreeViewNode> TreeViewNodes { get; set; }

		public static string GetStringPath(FileType type)
		{
			return _imgPath[type];
		}

		private static Dictionary<FileType, string> _imgPath = new()
		{
			{FileType.File, "/resources/file.png" },
			{FileType.Folder, "/resources/folder.png" }
		};
	}
}
