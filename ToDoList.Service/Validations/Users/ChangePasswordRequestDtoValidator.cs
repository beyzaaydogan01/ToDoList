using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Models.Dtos.Users.Requests;

namespace ToDoList.Service.Validations.Users;

public class ChangePasswordRequestDtoValidator : AbstractValidator<ChangePasswordRequestDto>
{
    public ChangePasswordRequestDtoValidator()
    {
        RuleFor(x => x.CurrentPassword).NotEmpty().WithMessage("Mevcut şifre alanı boş geçilemez.");

        RuleFor(x => x.NewPassword)
           .NotEmpty().WithMessage("Yeni şifre alanı boş geçilemez.")
           .MinimumLength(8).WithMessage("Yeni şifre en az 8 karakter olmalıdır.")
           .NotEqual(x => x.CurrentPassword).WithMessage("Yeni şifre mevcut şifreyle aynı olamaz.");

        RuleFor(x => x.NewPasswordAgain).NotEmpty().WithMessage("Yeni şifre alanı boş geçilemez.")
            .Equal(x => x.NewPassword).WithMessage("Yeni şifreler birbiriyle uyuşmuyor.");
    }
}
