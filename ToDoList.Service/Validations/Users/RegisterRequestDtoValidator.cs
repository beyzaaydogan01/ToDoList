using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Models.Dtos.Users.Requests;

namespace ToDoList.Service.Validations.Users;

public class RegisterRequestDtoValidator : AbstractValidator<RegisterRequestDto>
{
    public RegisterRequestDtoValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().WithMessage("İsim alanı boş geçilemez")
            .Length(2, 50).WithMessage("İsim alanı en az 2 en fazla 50 karakterli olmalıdır.");

        RuleFor(x => x.LastName).NotEmpty().WithMessage("Soyisim alanı boş geçilemez")
            .Length(2, 50).WithMessage("Soyisim alanı en az 2 en fazla 50 karakterli olmalıdır.");

        RuleFor(x => x.Email).NotEmpty().WithMessage("Email alanı boş geçilemez.")
            .EmailAddress().WithMessage("Geçerli bir email adresi giriniz.");

        RuleFor(x => x.Password).NotEmpty().WithMessage("Şifre alanı boş geçilemez.")
           .MinimumLength(8).WithMessage("Şifre en az 8 karakter olmalıdır.")
           .Matches("[A-Z]").WithMessage("Şifre en az bir büyük harf içermelidir.")
           .Matches("[a-z]").WithMessage("Şifre en az bir küçük harf içermelidir.")
           .Matches("[0-9]").WithMessage("Şifre en az bir rakam içermelidir.");
    }
}
