using Core.Exceptions;
using ToDoList.DataAccess.Abstracts;
using ToDoList.Models.Entities;

namespace ToDoList.Service.Rules;

public class CategoryBusinessRules
{
    public virtual void CategoryIsNullCheck(Category category)
    {
        if (category is null)
        {
            throw new NotFoundException("İlgili kategori bulunamadı.");
        }
    }
}
