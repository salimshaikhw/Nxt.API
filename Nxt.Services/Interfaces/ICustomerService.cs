using Nxt.Entities.Dtos.Customer;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nxt.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<CustomerDetails> CreateCustomer(CustomerInput customerInput);
        Task<bool> DeleteCustomer(int customerId);
        Task<CustomerDetails> GetCustomer(int customerId);
        Task<IEnumerable<CustomerDetails>> GetCustomers();
        Task<CustomerDetails> UpdateCustomer(int customerId, CustomerInput customerInput);
    }
}
