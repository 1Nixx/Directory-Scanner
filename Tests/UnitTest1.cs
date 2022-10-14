using Core.Interfaces;
using Core.Models;
using Core.Services;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;

namespace Tests
{
	public class Tests
	{
		private IDictionaryScannerService _dictionaryScanner;
		[SetUp]
		public void Setup()
		{
			_dictionaryScanner = new DictionaryScannerService();
		}

		[Test]
		public void EmptyFolder()
		{
			_dictionaryScanner.StartScan("../../../TestFolders/test1");

			while (!_dictionaryScanner.IsFinished) { }

			_dictionaryScanner.StopScan();
			var result = _dictionaryScanner.GetResult();

			Assert.Multiple(() =>
			{
				Assert.IsNotNull(result);
				Assert.That(result.FileSize, Is.EqualTo(0));
				Assert.IsNull(result.Childrens);
			});
		}

		[Test]
		public void FolderWithEmptyFolder()
		{
			_dictionaryScanner.StartScan("../../../TestFolders/test2");

			while (!_dictionaryScanner.IsFinished) { }

			_dictionaryScanner.StopScan();
			var result = _dictionaryScanner.GetResult();

			Assert.Multiple(() =>
			{
				Assert.IsNotNull(result);
				Assert.That(result.FileSize, Is.EqualTo(0));
				Assert.IsNotNull(result.Childrens);
				result.Childrens.TryTake(out var treeNode);
				Assert.That(treeNode.FileName, Is.EqualTo("test"));
				Assert.That(treeNode.FileSize, Is.EqualTo(0));
				Assert.IsNull(treeNode.Childrens);
			});
		}

		[Test]
		public void FolderWithFilesAndFolders()
		{
			_dictionaryScanner.StartScan("../../../TestFolders/test3");

			while (!_dictionaryScanner.IsFinished) { }

			_dictionaryScanner.StopScan();
			var result = _dictionaryScanner.GetResult();

			Assert.Multiple(() =>
			{
				Assert.IsNotNull(result);
				Assert.That(result.FileSize, Is.EqualTo(12192));
				Assert.That(result.PercentFileSize, Is.EqualTo(1));
				Assert.IsNotNull(result.Childrens);
				var childrens1 = result.Childrens.ToArray();

				Assert.That(childrens1.Length, Is.EqualTo(4));
				Assert.IsTrue(IsAllFilesExist(childrens1, new List<string>() { "empty", "test2", "1.txt", "test2.txt"}));

				TreeNode[] test2Childs = childrens1.Where(y => y.Childrens is not null && y.Childrens.Count == 2)
									   .Select(x => x.Childrens)
									   .First()
									   .ToArray();
				Assert.IsTrue(IsAllFilesExist(test2Childs, new List<string>() { "asd.txt", "asd.docx" }));
				Assert.That(childrens1.Where(y => y.FileName == "test2").Select(y => y.FileSize).First(), Is.EqualTo(12162));
			});
		}

		[Test]
		public void UnexistingFolder()
		{
			Assert.Throws(typeof(ArgumentException), () => _dictionaryScanner.StartScan("../../../TestFolders/test3adfs.txt"));
		}

		private bool IsAllFilesExist(TreeNode[] childrens1, List<string> list)
		{
			if (childrens1.Length != list.Count)
				return false;

			foreach (var node in childrens1)
			{
				var flag = false;
				foreach (var name in list)
				{
					if (string.Compare(name, node.FileName) == 0)
					{
						flag = true;
						break;
					}
				}		
				if (!flag)
					return false;
			}
			return true;
		}
	}
}