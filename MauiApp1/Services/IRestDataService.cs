using MauiApp1.Models;

namespace MauiApp1.Services
{
	public interface IRestDataService
	{
		Task<List<ToDo>> GetAllToDosAsync();

		Task<ToDo> UpdateAsync(ToDo todo);

		Task<bool> DeleteAsync(int id);
	}
}
