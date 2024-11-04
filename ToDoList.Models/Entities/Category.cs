using Core.Entities;

namespace ToDoList.Models.Entities;

public class Category : Entity<int>
{
    public string Name { get; set; }
    public List<ToDo> ToDos { get; set; }
}
