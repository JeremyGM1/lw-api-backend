using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Identity.Models
{
    public class RegistrationRequest
    {
        [Required(ErrorMessage = "The FirstName field is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "The LastName field is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "The Email field is required")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "The Password field is required")]
        [MinLength(6)]
        public string Password { get; set; }
    }
}