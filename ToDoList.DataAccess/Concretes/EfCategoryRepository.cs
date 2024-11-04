using Core.Repositories;
using Microsoft.EntityFrameworkCore;
using ToDoList.DataAccess.Abstracts;
using ToDoList.DataAccess.Contexts;
using ToDoList.Models.Entities;

namespace ToDoList.DataAccess.Concretes;

public class EfCategoryRepository : EfRepositoryBase<BaseDbContext, Category, int>, ICategoryRepository
{
    public EfCategoryRepository(BaseDbContext context) : base(context)
    {
    }
}
