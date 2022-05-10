using Nxt.Common.Models;

namespace Nxt.Entities.Dtos.Account
{
    public class RevokeTokenInput : Input<RevokeTokenInput>
    {
        public string Token { get; set; }
    }
}
