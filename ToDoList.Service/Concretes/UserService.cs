using Core.Exceptions;
using Microsoft.AspNetCore.Identity;
using ToDoList.Models.Dtos.Users.Requests;
using ToDoList.Models.Entities;
using ToDoList.Service.Abstracts;
using ToDoList.Service.Rules;

namespace ToDoList.Service.Concretes;

public sealed class UserService(UserManager<User> _userManager,
    UserBusinessRules businessRules) : IUserService
{
    public async Task<User> ChangePasswordAsync(string id, ChangePasswordRequestDto requestDto)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(id);
            businessRules.UserIsNullCheck(user);


            var result = await _userManager.ChangePasswordAsync(user, requestDto.CurrentPassword, requestDto.NewPassword);
            businessRules.CheckForIdentityResult(result);

            return user;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<string> DeleteAsync(string id)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(id);

            businessRules.UserIsNullCheck(user);

            var result = await _userManager.DeleteAsync(user);
            businessRules.CheckForIdentityResult(result);

            return "Kullanıcı Silindi.";
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

    }

    public async Task<User> GetByEmailAsync(string email)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(email);

            businessRules.UserIsNullCheck(user);

            return user;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

    }

    public async Task<User> LoginAsync(LoginRequestDto dto)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);

            businessRules.UserIsNullCheck(user);

            bool checkPassword = await _userManager.CheckPasswordAsync(user, dto.Password);

            if (checkPassword is false)
            {
                throw new BusinessException("Parolanız yanlış.");
            }

            return user;
        }
        catch(Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<User> RegisterAsync(RegisterRequestDto dto)
    {
        try
        {
            User user = new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                UserName = dto.Username,

            };
            var result = await _userManager.CreateAsync(user, dto.Password);
            businessRules.CheckForIdentityResult(result);

            var addRole = await _userManager.AddToRoleAsync(user, "User");
            businessRules.CheckForIdentityResult(addRole);

            return user;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<User> UpdateAsync(string id, UserUpdateRequestDto dto)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(id);
            businessRules.UserIsNullCheck(user);

            user.UserName = dto.Username;
            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;

            var result = await _userManager.UpdateAsync(user);
            businessRules.CheckForIdentityResult(result);

            return user;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

}
