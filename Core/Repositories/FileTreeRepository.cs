using Core.Interfaces;
using Core.Models;
using System;
using System.CodeDom.Compiler;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repositories
{
    internal class FileTreeRepository : IFileTreeRepository
    {
        private readonly TreeContext _context;
		private object obj = new();

		public FileTreeRepository(TreeContext context)
        {
            _context = context;
        }

        public FileTreeRepository()
        {
            _context = new TreeContext();
        }
        
		public bool AddNode(int? parentId, TreeNode newNode)
        {
            if (!_context.IsEmptyTree && parentId.HasValue)
            {
                lock (obj)
                {
                    var parentNode = GetNodeById(parentId.Value);
                    if (parentNode is null)
                        throw new ArgumentException();

                    newNode.Parent = parentNode;

                    parentNode.Childrens ??= new ConcurrentBag<TreeNode>();
                    parentNode.Childrens.Add(newNode);
                }
            }

            _context.Add(newNode);
            return true;
        }

        public TreeNode GetNodeById(int id)
        {
            return _context.Find(id);
        }

        public TreeNode GetRootNode()
        {
            return _context.RootNode;
        }
    }
}
