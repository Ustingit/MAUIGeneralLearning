using System.ComponentModel;

namespace MauiApp1.Models
{
	public class ToDo : INotifyPropertyChanged
	{
		int _id;

		string _todoname;

		public int Id { 
			get => _id; 
			set
			{
				if (_id == value) return;

				_id = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Id)));
			} 
		}

		public string Name { 
			get => _todoname;
			set
			{
				if (_todoname == value) return;

				_todoname = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
			} 
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
