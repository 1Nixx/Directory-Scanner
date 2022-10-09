using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    internal interface IThreadPoolScanner
    {
        bool IsFinished { get; }
        void StartScanner();
        void AddTask(Action<FileScanData> task, FileScanData scanData);
        void StopScanner();
    }
}
