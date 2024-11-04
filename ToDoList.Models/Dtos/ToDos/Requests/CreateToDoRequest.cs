namespace ToDoList.Models.Dtos.ToDos.Requests;

public sealed record CreateToDoRequest(
    string Title, 
    string Description, 
    string StartDate,
    string EndDate,
    int CategoryId, 
    string Priority
    );
