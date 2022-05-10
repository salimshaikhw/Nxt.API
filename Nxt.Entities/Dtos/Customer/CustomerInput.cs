using Nxt.Common.Models;
using System.ComponentModel.DataAnnotations;

namespace Nxt.Entities.Dtos.Customer
{
    public class CustomerInput : Input<CustomerInput>
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        public string Phone { get; set; }
    }
}
