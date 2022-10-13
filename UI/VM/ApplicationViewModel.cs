using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
				OnPropertyChanged("IsFileChosen");
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





		private RelayCommand _chooseFile;
		public RelayCommand ChooseFile
		{
			get
			{
				return _chooseFile ??= new RelayCommand(obj =>
				{
					FilePath = "asffdsf";
					IsFileChosen = true;
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
