﻿using System.ComponentModel.DataAnnotations;
using backend.Services;

namespace backend.Validators;

[AttributeUsage(AttributeTargets.Property)]
public class UniqueUsernameAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var userService = (IAuthService)validationContext.GetService(typeof(IAuthService))!;
        var username = (string)value;

        if (userService.IsUsernameTaken(username))
        {
            return new ValidationResult("Username already exists.");
        }

        return ValidationResult.Success!;
    }
}