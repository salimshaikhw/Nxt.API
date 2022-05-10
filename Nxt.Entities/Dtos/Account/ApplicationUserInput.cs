using Nxt.Common.Models;
using System.ComponentModel.DataAnnotations;

namespace Nxt.Entities.Dtos.Account
{
    public class ApplicationUserInput : Input<ApplicationUserInput>
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
    }
}
