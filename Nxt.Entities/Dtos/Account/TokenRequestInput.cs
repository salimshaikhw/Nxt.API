using Nxt.Common.Models;
using System.ComponentModel.DataAnnotations;

namespace Nxt.Entities.Dtos.Account
{
    public class TokenRequestInput : Input<TokenRequestInput>
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
