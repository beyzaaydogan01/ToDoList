using Core.Repositories;
using ToDoList.Models.Entities;

namespace ToDoList.DataAccess.Abstracts;

public interface IToDoRepository : IRepository<ToDo, Guid>
{
}
