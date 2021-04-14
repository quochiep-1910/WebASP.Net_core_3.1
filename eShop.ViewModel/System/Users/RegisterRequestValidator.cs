using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.ViewModels.System.Users
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("Phải nhập Họ")
                .MaximumLength(200).WithMessage("Họ không được quá 200 kí tự");

            RuleFor(x => x.LastName).NotEmpty().WithMessage("Phải nhập tên")
                .MaximumLength(200).WithMessage("Tên không được quá 200 kí tự");

            RuleFor(x => x.Dob).NotEmpty().WithMessage("Phải nhập Ngày sinh")
                .GreaterThan(DateTime.Now.AddYears(-100)).WithMessage("Vui lòng nhập đúng ngày sinh!!!");

            RuleFor(x => x.Email).NotEmpty().WithMessage("Phải nhập Email")
                .Matches(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")
                .WithMessage("Định dạng email không khớp ");

            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Phải nhập Số điện thoại");

            RuleFor(x => x.UserName).NotEmpty().WithMessage("Phải nhập tên tài khoản");

            RuleFor(x => x.Password).NotEmpty().WithMessage("Phải nhập Password")
                .MinimumLength(6).WithMessage("Mật khẩu ít nhất có 6 kí tự");

            RuleFor(x => x).Custom((request, context) =>
            {
                if (request.Password != request.ConfirmPassword)
                {
                    context.AddFailure("Xác nhận mật khẩu không khớp");
                }
            });
        }
    }
}