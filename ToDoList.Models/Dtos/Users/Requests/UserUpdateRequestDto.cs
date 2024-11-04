namespace ToDoList.Models.Dtos.Users.Requests;

public sealed record UserUpdateRequestDto(
    string FirstName,
    string LastName,
    string Username
    );
