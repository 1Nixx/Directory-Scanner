using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
	internal interface IFileTreeRepository
	{
		TreeNode GetNodeById(int id);
		TreeNode GetRootNode();
		bool AddNode(int? parentId, TreeNode newNode);
	}
}
