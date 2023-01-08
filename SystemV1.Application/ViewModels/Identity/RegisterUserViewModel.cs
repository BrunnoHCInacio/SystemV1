using System.ComponentModel.DataAnnotations;
using SystemV1.Application.Resources;

namespace SystemV1.Application.ViewModels.Identity
{
    public class RegisterUserViewModel
    {
        [Required(ErrorMessage = ConstantMessages.FieldIsRequired)]
        [EmailAddress(ErrorMessage = ConstantMessages.InvalidValueField)]
        public string Email { get; set; }

        [Required(ErrorMessage = ConstantMessages.FieldIsRequired)]
        [StringLength(100, ErrorMessage = ConstantMessages.InvalidLengthField, MinimumLength = 8)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = ConstantMessages.PasswordNotEquals)]
        public string ConfirmPassword { get; set; }
    }
}