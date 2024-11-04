using Core.Repositories;
using ToDoList.Models.Entities;

namespace ToDoList.DataAccess.Abstracts;

public interface ICategoryRepository : IRepository<Category, int>
{
}
