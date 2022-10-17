using MauiApp1.Models;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace MauiApp1.Services
{
	public class RestDataService : IRestDataService
	{
		private readonly HttpClient _httpClient;
		private readonly string _baseAddress;
		private readonly Uri _url;
		private readonly JsonSerializerOptions _jsonOptions;

		public RestDataService(HttpClient httpClient)
		{
			_httpClient = httpClient;
			_baseAddress = DeviceInfo.Platform == DevicePlatform.Android
				? "http://10.0.2.2:5151"
				: "https://localhost:7151";

			_url = new Uri($"{_baseAddress}/Todos");
			_jsonOptions = new JsonSerializerOptions
			{
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase
			};
		}

		public async Task<bool> DeleteAsync(int id)
		{
			if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
			{
				System.Diagnostics.Debug.WriteLine($"---> No internet connection during deleting item with id: {id} !");
				return false;
			}

			try
			{
				HttpResponseMessage response = await _httpClient.DeleteAsync($"{_url}/delete?id={id}");

				if (response.IsSuccessStatusCode)
				{
					string content = await response.Content.ReadAsStringAsync();
					return JsonSerializer.Deserialize<bool>(content);
				}
				else
				{
					System.Diagnostics.Debug.WriteLine($"---> Non http 2xx response during deleting item with id: {id} !");
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"---> Exception during deleting item with id: {id} : {ex.Message}, {ex.StackTrace}!");
				return false;
			}

			return true;
		}

		public async Task<List<ToDo>> GetAllToDosAsync()
		{
			var todos = new List<ToDo>();

			if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
			{
				System.Diagnostics.Debug.WriteLine("---> No internet connection during fetching items!!");
				return todos;
			}

			try
			{
				HttpResponseMessage response = await _httpClient.GetAsync($"{_url}/get");
				if (response.IsSuccessStatusCode)
				{
					string content = await response.Content.ReadAsStringAsync();
					todos = JsonSerializer.Deserialize<List<ToDo>>(content, _jsonOptions);
				} else
				{
					System.Diagnostics.Debug.WriteLine("---> Non http 2xx response during fetching items!!");
				}
			} catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"---> Exception during fetching items: {ex.Message}, {ex.StackTrace}!");
			}

			return todos;
		}

		public async Task<ToDo> UpdateAsync(ToDo todo)
		{
			if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
			{
				System.Diagnostics.Debug.WriteLine($"---> No internet connection during updating item with id: {todo.Id} !");
				return null;
			}

			try
			{
				var todoAsJson = JsonSerializer.Serialize<ToDo>(todo, _jsonOptions);
				HttpResponseMessage response = await _httpClient.PostAsync($"{_url}/creatOrUpdate", new StringContent(todoAsJson, Encoding.UTF8, "application/json"));

				if (response.IsSuccessStatusCode)
				{
					string content = await response.Content.ReadAsStringAsync();
					return JsonSerializer.Deserialize<ToDo>(content, _jsonOptions);
				}
				else
				{
					System.Diagnostics.Debug.WriteLine($"---> Non http 2xx response during updating item with id: {todo.Id} !");
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"---> Exception during updating item with id: {todo.Id} : {ex.Message}, {ex.StackTrace}!");
			}

			return null;
		}
	}
}
