using Core.Interfaces;
using Core.Models;
using Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
	public class DictionaryScannerService : IDictionaryScannerService
	{
		private IFileTreeRepository _fileTreeRepository;
		private readonly IThreadPoolScanner _threadPoolScanner;

		public DictionaryScannerService()
		{
			_fileTreeRepository = new FileTreeRepository();
			_threadPoolScanner = new ThreadPoolScanner();
		}

		public bool IsFinished => _threadPoolScanner.IsFinished;

		public TreeNode GetResult()
		{
			return _fileTreeRepository.GetRootNode();
		}

		public void StartScan(string startDir)
		{
			_threadPoolScanner.StartScanner();

			var scanData = new FileScanData
			{
				ParentId = null,
				FileName = startDir
			};

			_threadPoolScanner.AddTask(ScanFile, scanData);
		}

		private void ScanFile(FileScanData data)
		{
			string filePath;
			TreeNode? parentNode;
			FileType fileType;
			long? fileSize;

			#region File initialization
			if (data.ParentId.HasValue)
			{
				parentNode = _fileTreeRepository.GetNodeById(data.ParentId.Value);
				filePath = parentNode.Route + "/" + data.FileName;
			}
			else
				filePath = data.FileName;

			FileAttributes attr = File.GetAttributes(filePath);

			if (attr.HasFlag(FileAttributes.Directory))
			{
				fileSize = null;
				fileType = FileType.Folder;
			}
			else
			{
				fileSize = (new FileInfo(filePath)).Length;
				fileType = FileType.File;
			}

			Console.WriteLine($" \n 1) {{\n FileName : {filePath}, \n fileType : {fileType} \n}} \n");

			#endregion

			var node = new TreeNode
			{
				FileName = data.FileName,
				FileTreeType = fileType,
				FileSize = fileSize
			};

			_fileTreeRepository.AddNode(data.ParentId, node);

			Console.WriteLine($" \n 2) {{\n Id: {node.Id} \n FileName : {node.FileName} \n fileType : {node.FileTreeType} \n Route : {node.Route} \n}} \n");

			if (fileType == FileType.Folder)
			{
				var subFiles = Directory.GetFileSystemEntries(filePath);

				foreach (var file in subFiles)
				{
					var scanData = new FileScanData
					{
						FileName = Path.GetFileName(file),
						ParentId = node.Id
					};

					_threadPoolScanner.AddTask(ScanFile, scanData);
				}
			}

		}

		public void StopScan()
		{
			_threadPoolScanner.StopScanner();
		}
	}
}
