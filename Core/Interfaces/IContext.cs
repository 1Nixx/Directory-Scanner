using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
	internal interface IContext
	{
		void Add(TreeNode node);
		TreeNode Find(int id);
	}
}
