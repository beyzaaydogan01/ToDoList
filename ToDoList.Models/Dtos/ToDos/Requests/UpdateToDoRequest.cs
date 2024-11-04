namespace ToDoList.Models.Dtos.ToDos.Requests;

public sealed record UpdateToDoRequest(
    string Title,
    string Description,
    string StartDate,
    string EndDate,
    int CategoryId,
    string Priority,
    string Completed
    );
