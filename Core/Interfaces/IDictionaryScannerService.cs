using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
	public interface IDictionaryScannerService
	{
		public bool IsFinished { get; }
		void StartScan(string startDir);
		void StopScan();
		TreeNode GetResult();
	}
}
