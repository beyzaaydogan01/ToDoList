using Microsoft.AspNetCore.Identity;

namespace ToDoList.Models.Entities;

public class User : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public List<ToDo> ToDos { get; set; }
}
