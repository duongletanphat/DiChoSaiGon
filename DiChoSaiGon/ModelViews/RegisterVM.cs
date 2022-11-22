﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DiChoSaiGon.ModelViews
{
    public class RegisterVM
    {
        [Key]
        public string CustomerID { get; set; }
        [Display(Name = "Họ và Tên")]
        [Required(ErrorMessage = "Vui lòng nhập họ và tên!")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Email!")]
        [MaxLength(150)]
        [DataType(DataType.EmailAddress)]
        [Remote(action: "ValidateEmail", controller: "Account")]
        public string Email { get; set; }

        [MaxLength(11)]
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại!")]
        [Display(Name = "Điện thoại")]
        [DataType(DataType.PhoneNumber)]
        [Remote(action: "ValidatePhone", controller: "Account")]
        public string Phone { get; set; }

        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [MinLength(6, ErrorMessage = "Vui lòng nhập tối thiểu 6 ký tự")]
        public string Password { get; set; }

        [MinLength(6, ErrorMessage = "Bạn cần đặt mật khẩu tối thiểu 6 ký tự")]
        [Display(Name = "Nhập lại mật khẩu")]
        [Compare("Password", ErrorMessage = "Vui lòng nhập mật khẩu giống nhau")]
        public string ConfirmPassword { get; set; }
    }
}
