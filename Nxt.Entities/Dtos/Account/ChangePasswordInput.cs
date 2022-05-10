﻿using Nxt.Common.Models;
using System.ComponentModel.DataAnnotations;

namespace Nxt.Entities.Dtos.Account
{
    public class ChangePasswordInput : Input<ChangePasswordInput>
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
    }
}
