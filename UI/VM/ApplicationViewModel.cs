using Core.Interfaces;
using Core.Models;
using Core.Services;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Forms;
using UI.Helpers;

namespace UI.VM
{
	public class ApplicationViewModel : INotifyPropertyChanged
	{
		private bool _isSearchEnabled = false;
		public bool IsSearchEnabled
		{
			get
			{
				return _isSearchEnabled;
			}
			set
			{
				_isSearchEnabled = value;
				OnPropertyChanged("IsStartEnable");
				OnPropertyChanged("IsSearchEnabled");
			}
		}

		private bool _isFileChosen = false;
		public bool IsFileChosen
		{
			get
			{
				return _isFileChosen;
			}
			set
			{
				_isFileChosen = value;
				OnPropertyChanged("IsStartEnable");
				OnPropertyChanged("IsFileChosen");
			}
		}

		public bool IsStartEnable
		{
			get
			{
				return IsFileChosen && !IsSearchEnabled;
			}
		}

		private string? _filePath;
		public string FilePath
		{
			get
			{
				return _filePath ?? "";
			}
			set
			{
				_filePath = value;
				OnPropertyChanged("FilePath");
			}
		}

		private IDictionaryScannerService _dictionaryScannerService;
		private ObservableCollection<TreeViewNode> _treeNodes;
		public ObservableCollection<TreeViewNode> TreeViewList
		{
			get
			{
				return _treeNodes;
			}
			set { 
				_treeNodes = value; 
			}
		}

		public Core.Models.TreeNode TreeResult
		{
			set
			{
				if (value != null)
				{
					_treeNodes = new ObservableCollection<TreeViewNode>();
					_treeNodes.Add(value.ToTreeViewNode());
					OnPropertyChanged("TreeViewList");
				}
			}
		}



		private RelayCommand _chooseFile;
		public RelayCommand ChooseFile
		{
			get
			{
				return _chooseFile ??= new RelayCommand(obj =>
				{
					using var dialog = new FolderBrowserDialog();

					if( dialog.ShowDialog() == DialogResult.OK)
					{
						FilePath = dialog.SelectedPath;
						IsFileChosen = true;
					}
				});
			}
		}		

		private RelayCommand _startSearch;
		public RelayCommand StartSearch
		{
			get
			{
				return _startSearch ??= new RelayCommand(obj =>
				{
					_dictionaryScannerService = new DictionaryScannerService();
					
					Task.Run(() =>
					{
						IsSearchEnabled = true;
						_dictionaryScannerService.StartScan(FilePath);

						while (!_dictionaryScannerService.IsFinished){}

						_dictionaryScannerService.StopScan();
						var result = _dictionaryScannerService.GetResult();

						IsSearchEnabled = false;

						TreeResult = result;
					});
				});
			}
		}

		private RelayCommand _stopScan;
		public RelayCommand StopScan
		{
			get
			{
				return _stopScan ??= new RelayCommand(obj =>
				{
					_dictionaryScannerService?.StopScan();
				});
			}
		}

		public event PropertyChangedEventHandler? PropertyChanged;
		public void OnPropertyChanged(string prop = "")
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(prop));
		}
	}
}
