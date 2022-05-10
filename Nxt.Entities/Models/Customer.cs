using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nxt.Entities.Models
{
    [Table("Customer")]
    public class Customer : BaseEntity
    {
        [Key]
        public int CustomerId { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
    }
}
