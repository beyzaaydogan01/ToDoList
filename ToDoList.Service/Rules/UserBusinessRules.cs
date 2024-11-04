using Core.Exceptions;
using Microsoft.AspNetCore.Identity;
using ToDoList.Models.Entities;

namespace ToDoList.Service.Rules;

public class UserBusinessRules
{
    public void UserIsNullCheck(User user)
    {
        if (user is null)
        {
            throw new NotFoundException("Kullanıcı bulunamadı.");
        }
    }

    public void CheckForIdentityResult(IdentityResult result)
    {
        if (!result.Succeeded)
        {
            throw new BusinessException(result.Errors.ToList().First().Description);
        }
    }
}
