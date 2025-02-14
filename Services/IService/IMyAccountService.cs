using Unilever.CDExcellent.API.Models.Dto;

namespace Unilever.CDExcellent.API.Services.IService
{
    public interface IMyAccountService
    {
        Task<MyAccountDto> GetMyAccountAsync(int userId);
        Task<bool> UpdateUserInfoAsync(int userId, UpdateUserDto updateUserDto);
        Task<bool> ChangePasswordAsync(int userId, ChangePasswordDto changePasswordDto);
    }
}
