using Nxt.Common.Models;
using System.ComponentModel.DataAnnotations;

namespace Nxt.Entities.Dtos.Account
{
    public class RoleInput: Input<RoleInput>
    {
        [Required]
        public string Email { get; set; }
        
        [Required]
        public string Role { get; set; }
    }
}
