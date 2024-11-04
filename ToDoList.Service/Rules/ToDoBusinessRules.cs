using Core.Exceptions;
using FluentValidation;
using ToDoList.Models.Entities;

namespace ToDoList.Service.Rules;

public class ToDoBusinessRules
{
    public virtual void ToDoIsNullCheck(ToDo toDo)
    {
        if (toDo is null)
        {
            throw new NotFoundException("İlgili todo bulunamadı.");
        }
    }

    public virtual void ToDoEndDateMustBeValid(DateTime endDate)
    {
        if (endDate <= DateTime.Now)
        {
            throw new ValidationException("Bitiş tarihi bugünden daha eski veya aynı olamaz.");
        }
    }
}
