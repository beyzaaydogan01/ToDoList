using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Models.Dtos.ToDos.Requests;

namespace ToDoList.Service.Validations.ToDos;

public class UpdateToDoRequestValidator : AbstractValidator<UpdateToDoRequest>
{
    public UpdateToDoRequestValidator()
    {
        RuleFor(x => x.Title).NotEmpty().WithMessage("Başlık alanı boş geçilemez")
            .Length(2, 50).WithMessage("Başlık alanı en az 2 en fazla 50 karakterli olmalıdır.");

        RuleFor(x => x.Description).NotEmpty().WithMessage("Açıklama alanı boş geçilemez.");

        RuleFor(x => x.EndDate).NotEmpty().WithMessage("Bitiş tarihi belirlenmelidir.");
    }
}
