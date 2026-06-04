using Microsoft.EntityFrameworkCore.Query.Internal;
using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required( ErrorMessage="Email Is Required")]
        [EmailAddress(ErrorMessage ="Email must be Valid and in Email format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password Is Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password",ErrorMessage = "Password Must be Match")]

        [DataType(DataType.Password)]
        public string ConfirmPassword {  get; set; }

    }
}
