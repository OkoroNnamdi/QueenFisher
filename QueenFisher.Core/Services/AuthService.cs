using AspNetCoreHero.Results;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using QueenFisher.Core.DTO;
using QueenFisher.Core.Interfaces.IRepositories;
using QueenFisher.Core.Interfaces.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace QueenFisher.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthenticationRepository _authrepository;

        public AuthService(IAuthenticationRepository authrepository)
        {
            _authrepository = authrepository;
        }
        public async Task<Result<string>>ChangePassword(ChangePasswordDTO model)
        {
            if (model.ConfirmNewPassword != model.NewPassword) return await Result<string>.FailAsync("Password does not match");
            var response = await _authrepository.ChangePassword(model);
            return response;
        }

        public async Task<Result<string>> Confirmemail(string email, string token)
        {
            try
            {
                var response = await _authrepository.Confirmemail(email, token);
                if (response.Succeeded)
                {
                    return await Result<string>.SuccessAsync("email verified successfully");
                }
                return await Result<string>.FailAsync("email not verified");
            }
            catch (Exception ex)
            {
                return await Result<string>.FailAsync("an error occured while verifying email");

            }
        }

        public async Task<Result<string>>ForgottenPassword(ResetPasswordDTO model)
        {
            var response = await _authrepository.ForgottenPassword(model);
            return response;
        }

        public async Task<Result<LoginUserDTO>> Login(LoginDTO model)
        {
            return await _authrepository.Login(model);
        }

        public async Task<Result<string>> RefreshToken()
        {

            return await _authrepository.RefreshToken();
        }

        public async Task<Result<string>> Register(RegisterDTO user)
        {
            var result = await _authrepository.Register(user);
            //var response = new Result<string>();
            if (result.Succeeded)
            {
                //response.Succeeded = true;
                // response.Message = result.Message;
                //response.Data = result.Data;
                //return await Result<string>.SuccessAsync(result.Message);
                return new Result<string> { Succeeded = true, Data = result.Data, Message = result.Message };
            }
            else
            {
               // response.Succeeded = false;
               // response.Message = "Failed to register, please change check the email, username and password.";
                return await Result<string>.FailAsync("Failed to register, please change check the email, username and password.");
            }

           // return Result;
        }

        public async Task<Result<string>>ResetPassword(UpdatePasswordDTO model)
        {
            var response = await _authrepository.ResetPassword(model);
            return response;
        }

        public async Task<Result<string>> ConfirmEmail(string email, string token)
        {
            try
            {
                var response = await _authrepository.Confirmemail(email, token);
                if (response.Succeeded)
                {
                    return await Result<string>.SuccessAsync("email verified successfully");
                }
                return await Result<string>.FailAsync("email not verified");
            }
            catch (Exception ex)
            {
                return await Result<string>.FailAsync("an error occured while verifying email");

            }
        }
        public async Task Signout()
        {
            await _authrepository.Signout();
        }
    }
}
