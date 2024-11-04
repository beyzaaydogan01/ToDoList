namespace ToDoList.Models.Dtos.ToDos.Responses;

public sealed record ToDoResponseDto
{
    public Guid Id { get; init; }
    public string Title { get; init; }
    public string Description { get; init; }
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
    public string Priority { get; init; }
    public string Category { get; init; }
    public string Completed { get; init; }
}
