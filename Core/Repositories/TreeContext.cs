using Core.Models;
using System.Collections.Concurrent;
using System.Linq;

namespace Core.Repositories
{
	public class TreeContext
	{
		public bool IsEmptyTree { get => RootNode is null; }

		public TreeNode RootNode { get; private set; }

		private ConcurrentDictionary<int, TreeNode> IdToNodeDict { get; set; } = new();

		public void Add(TreeNode node)
		{
			lock (IdToNodeDict)
			{
				var newId = IdToNodeDict.OrderByDescending(x => x.Key).Select(x => x.Key).FirstOrDefault(0) + 1;
				node.Id = newId;
			}		

			RootNode ??= node;
			var res = IdToNodeDict.TryAdd(node.Id, node);
		}

		public TreeNode? Find(int id)
		{
			var isSucseed = IdToNodeDict.TryGetValue(id, out var result);
			if (isSucseed)
				return result;
			else
				return null;
		}
	}
}