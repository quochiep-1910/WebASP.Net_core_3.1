using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.ViewModels.System.Users
{
    public class WorkingScheduleRequestValidator : AbstractValidator<WorkingscheduleViewModel>
    {
        public WorkingScheduleRequestValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Phải nhập Tên Đăng kí")
                   .MaximumLength(200).WithMessage("Tên Đăng kí không được quá 200 kí tự");
            RuleFor(x => x.StartDate).NotEmpty().WithMessage("Phải nhập Ngày bắt đầu");
            RuleFor(x => x.EndDate).NotEmpty().WithMessage("Phải nhập Ngày bắt đầu");
            RuleFor(x => x.LyDo).NotEmpty().WithMessage("Phải nhập Lý Do")
                  .MaximumLength(200).WithMessage("Lý Do không được quá 200 kí tự");
        }
    }
}