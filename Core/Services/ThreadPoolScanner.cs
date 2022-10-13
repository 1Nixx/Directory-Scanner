using Core.Interfaces;
using Core.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
	internal class ThreadPoolScanner : IThreadPoolScanner
	{
		public const int MaxThreads = 5;
		private readonly Semaphore _semaphore;
		private int count = MaxThreads;
		private bool scannerStarted;
		private Thread _queueHandler;
		private readonly ConcurrentQueue<TaskInfo> _taskQueue;
		private readonly CancellationTokenSource _tokenSource;

		public bool IsFinished => _taskQueue.IsEmpty && count == MaxThreads && !scannerStarted;

		public ThreadPoolScanner()
		{
			_semaphore = new Semaphore(MaxThreads, MaxThreads);
			_taskQueue = new ConcurrentQueue<TaskInfo>();
			_tokenSource = new CancellationTokenSource();
		}

		public void StartScanner()
		{
			count = MaxThreads;
			scannerStarted = true;
			_queueHandler = new Thread(QueueHandler);
			_queueHandler.Start(_tokenSource.Token);
		}

		public void AddTask(Action<FileScanData> task, FileScanData scanData)
		{
			scannerStarted = false;
			var info = new TaskInfo
			{
				Task = task,
				TaskData = scanData
			};

			_taskQueue.Enqueue(info);
		}

		private void QueueHandler(object? obj)
		{
			var token = (CancellationToken)obj;

			while (!token.IsCancellationRequested)
			{
				if (_taskQueue.IsEmpty)
					continue;

				_semaphore.WaitOne();
				count--;
				
				_taskQueue.TryDequeue(out var taskInfo);

				ThreadPool.QueueUserWorkItem(TaskWrapper, taskInfo, true);
			}
		}

		private void TaskWrapper(TaskInfo data)
		{
			data.Task(data.TaskData);
			count++;
			_semaphore.Release();		
		}

		public void StopScanner()
		{
			_tokenSource.Cancel();
		}

		class TaskInfo
		{
			public Action<FileScanData> Task { get; set; }
			public FileScanData TaskData { get; set; }
		}
		
	}
}
