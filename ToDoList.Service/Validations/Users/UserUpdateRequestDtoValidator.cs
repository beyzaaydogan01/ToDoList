using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Models.Dtos.Users.Requests;

namespace ToDoList.Service.Validations.Users;

public class UserUpdateRequestDtoValidator : AbstractValidator<UserUpdateRequestDto>
{
    public UserUpdateRequestDtoValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().WithMessage("İsim alanı boş geçilemez")
            .Length(2, 50).WithMessage("İsim alanı en az 2 en fazla 50 karakterli olmalıdır.");

        RuleFor(x => x.LastName).NotEmpty().WithMessage("Soyisim alanı boş geçilemez")
            .Length(2, 50).WithMessage("Soyisim alanı en az 2 en fazla 50 karakterli olmalıdır.");
    }
}
