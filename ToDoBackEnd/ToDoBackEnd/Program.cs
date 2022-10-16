using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var todos = new List<ToDo>()
{
	new ToDo
	{
		Id = 1,
		Name = "buy groceries"
	},
	new ToDo
	{
		Id = 2,
		Name = "do morning routine"
	},
	new ToDo
	{
		Id = 3,
		Name = "write 5 tests"
	},
	new ToDo
	{
		Id = 4,
		Name = "go to a walk"
	},
	new ToDo
	{
		Id = 5,
		Name = "watch a file"
	},
};

app.MapGet("Todos/get", () =>
{
	var items = todos;
	return Results.Ok(items);
});

app.MapPost("Todos/creatOrUpdate", (ToDo toDo) =>
{
	if (toDo.Id == 0)
	{
		toDo.Id = todos.Max(x => x.Id) + 1;
		todos.Add(toDo);
	}
	else
	{
		var item = todos.FirstOrDefault(x => x.Id == toDo.Id);

		if (item != null && item.Name != toDo.Name)
		{
			item.Name = toDo.Name;
		}
		else
		{
			todos.Add(toDo);
		}
	}

	return Results.Ok(toDo);
});

app.MapDelete("Todos/delete", (int id) =>
{
	var item = todos.FirstOrDefault(x => x.Id == id);

	if (item == null)
	{
		return Results.NotFound(id);
	}

	todos.Remove(item);

	return Results.Ok(true);
});

app.Run();




public class ToDo
{
	public int Id { get; set; }

	public string Name { get; set; }
}