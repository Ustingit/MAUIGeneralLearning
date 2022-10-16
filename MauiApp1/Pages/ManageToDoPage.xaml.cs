using MauiApp1.Models;
using MauiApp1.Services;
using System.Diagnostics;

namespace MauiApp1.Pages;

[QueryProperty(nameof(ToDo), "ToDo")]
public partial class ManageToDoPage : ContentPage
{
	private readonly IRestDataService dataService;
	ToDo _toDo;
	bool _isNew;

	public ManageToDoPage(IRestDataService dataService)
	{
		InitializeComponent();

		this.dataService = dataService;
		BindingContext = this;
	}

	public ToDo ToDo
	{
		get => _toDo;
		set
		{
			_isNew = IsNew(value);
			_toDo = value;
			OnPropertyChanged();
		}
	}

	bool IsNew(ToDo todo) => todo.Id == 0;

	#region Ui-handlers

	async void OnCancelButtonClicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("..");
	}

	async void OnDeleteButtonClicked(object sender, EventArgs e)
	{
		await dataService.DeleteAsync(ToDo.Id);
		await Shell.Current.GoToAsync("..");
	}

	async void OnSaveButtonClicked(object sender, EventArgs e)
	{
		Debug.WriteLine($"---> Saving item {ToDo.Id}, {ToDo.Name}.");
		var updatedItem = await dataService.UpdateAsync(ToDo);
		await Shell.Current.GoToAsync("..");
	}

	#endregion
}