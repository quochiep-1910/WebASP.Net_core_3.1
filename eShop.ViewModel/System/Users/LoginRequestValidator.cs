using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.ViewModels.System.Users
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>

    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Phải nhập tên tài khoản");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Phải nhập mật khẩu")
                .MinimumLength(6).WithMessage("Phải nhập ít nhất 6 kí tự");
        }
    }
}