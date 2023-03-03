using AspNetCoreHero.Results;
using QueenFisher.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueenFisher.Core.Interfaces.IServices
{
    public interface IAuthService
    {
        Task<Result<LoginUserDTO>> Login(LoginDTO model);
        Task<Result<string>> Register(RegisterDTO user);
        Task<Result<string>> RefreshToken();
        public Task<Result<string>>ChangePassword(ChangePasswordDTO changePasswordDTO);
        public Task<Result<string>>ResetPassword(UpdatePasswordDTO resetPasswordDTO);
        public Task<Result<string>>ForgottenPassword(ResetPasswordDTO model);
        Task<Result<string>> Confirmemail(string email, string token);
        Task Signout();
    }
}
